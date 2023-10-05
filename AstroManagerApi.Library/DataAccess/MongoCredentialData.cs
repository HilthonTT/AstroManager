namespace AstroManagerApi.Library.DataAccess;
public class MongoCredentialData : ICredentialData
{
    private const string CacheName = nameof(MongoCredentialData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly IMongoCollection<CredentialModel> _credentials;
    private readonly IDistributedCacheHelper _cache;
    private readonly IAesEncryptor _aes;
    private readonly ILogger<MongoCredentialData> _logger;

    public MongoCredentialData(
        IDbConnection db,
        IDistributedCacheHelper cache,
        IAesEncryptor aes,
        ILogger<MongoCredentialData> logger)
    {
        _credentials = db.CredentialCollection;
        _cache = cache;
        _aes = aes;
        _logger = logger;
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
        try
        {
            field.Value = await _aes.DecryptAsync(field.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error decrypting field: {ex}", ex.Message);
        }
    }

    private async Task<List<CredentialModel>> DecryptFieldValuesAsync(List<CredentialModel> credentials)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = CancellationToken.None
        };

        await Parallel.ForEachAsync(credentials, async (credential, token) =>
        {
            await Parallel.ForEachAsync(credential.Fields, async (field, token) =>
            {
                await DecryptFieldAsync(field);
            });
        });

        return credentials;
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
        credential.DateModified = DateTime.UtcNow;

        await _credentials.ReplaceOneAsync(c => c.Id == credential.Id, credential);
        await _cache.RemoveAsync(key);
    }

    public async Task<CredentialModel> CreateCredentialAsync(CredentialModel credential)
    {
        string key = CacheNamePrefix + credential.User.Id;

        credential.Fields = await EncryptFieldsAsync(credential);
        await _credentials.InsertOneAsync(credential);

        await _cache.RemoveAsync(key);

        return credential;
    }

    public async Task DeleteCredentialAsync(string id)
    {
        var credential = await GetCredentialAsync(id);
        string key = CacheNamePrefix + credential.User.Id;

        await _cache.RemoveAsync(key);

        await _credentials.DeleteOneAsync(x => x.Id == id);
    }
}
