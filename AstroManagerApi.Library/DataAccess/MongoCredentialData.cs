using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Models;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoCredentialData : ICredentialData
{
    private const string CacheName = nameof(MongoCredentialData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly IMongoCollection<CredentialModel> _credentials;
    private readonly IDistributedCache _cache;
    private readonly IAesEncryptor _aes;

    public MongoCredentialData(
        IDbConnection db,
        IDistributedCache cache,
        IAesEncryptor aes)
    {
        _credentials = db.CredentialCollection;
        _cache = cache;
        _aes = aes;
    }

    private async Task<List<FieldModel>> EncryptFieldsAsync(CredentialModel credential)
    {
        var encryptedFields = new List<FieldModel>();
        foreach (var f in credential.Fields)
        {
            f.Value = await _aes.EncryptAsync(f.Value);
            encryptedFields.Add(f);
        }

        credential.Fields = encryptedFields.ToList();

        return encryptedFields;
    }

    private async Task DecryptFieldAsync(FieldModel field)
    {
        field.Value = await _aes.DecryptAsync(field.Value);
    }

    private async Task<List<CredentialModel>> DecryptFieldValuesAsync(List<CredentialModel> credentials)
    {
        var tasks = new List<Task>();

        foreach (var credential in credentials)
        {
            foreach (var field in credential.Fields)
            {
                tasks.Add(DecryptFieldAsync(field));
            }
        }

        await Task.WhenAll(tasks);

        return credentials;
    }    

    public async Task<List<CredentialModel>> GetAllCredentialsAsync()
    {
        var output = await _cache.GetRecordAsync<List<CredentialModel>>(CacheName);
        if (output is null)
        {
            var results = await _credentials.FindAsync(_ => true);
            output = await results.ToListAsync();

            await _cache.SetRecordAsync(CacheName, output, TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10));
        }

        return output;
    }

    public async Task<List<CredentialModel>> GetUsersCredentialsAsync(string userId)
    {
        string key = CacheNamePrefix + userId;
        var output = await _cache.GetRecordAsync<List<CredentialModel>>(key);
        if (output is null)
        {
            var results = await _credentials.FindAsync(c => c.User.Id == userId);
            output = await results.ToListAsync();

            output = await DecryptFieldValuesAsync(output);

            await _cache.SetRecordAsync(key, output, TimeSpan.FromHours(1), TimeSpan.FromMinutes(40));
        }

        return output;
    }

    public async Task<CredentialModel> GetCredentialAsync(string id)
    {
        var results = await _credentials.FindAsync(c => c.Id == id);
        return await results.FirstOrDefaultAsync();
    }

    public async Task UpdateCredentialAsync(CredentialModel credential)
    {
        string key = CacheNamePrefix + credential.User.Id;

        credential.Fields = await EncryptFieldsAsync(credential);

        await _credentials.ReplaceOneAsync(c => c.Id == credential.Id, credential);
        await _cache.RemoveAsync(key);
    }

    public async Task CreateCredentialAsync(CredentialModel credential)
    {
        string key = CacheNamePrefix + credential.User.Id;

        credential.Fields = await EncryptFieldsAsync(credential);
        await _credentials.InsertOneAsync(credential);

        await _cache.RemoveAsync(key);
    }
}
