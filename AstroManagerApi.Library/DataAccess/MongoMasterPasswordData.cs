using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Models;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoMasterPasswordData : IMasterPasswordData
{
    private const string CacheNamePrefix = $"{nameof(MongoMasterPasswordData)}_";
    private readonly IMongoCollection<MasterPasswordModel> _passwords;
    private readonly IDistributedCache _cache;
    private readonly ITextHasher _hasher;

    public MongoMasterPasswordData(
        IDbConnection db,
        IDistributedCache cache,
        ITextHasher hasher)
    {
        _passwords = db.MasterPasswordCollection;
        _cache = cache;
        _hasher = hasher;
    }

    public async Task<MasterPasswordModel> GetUsersMasterPasswordAsync(string userId)
    {
        string key = CacheNamePrefix + userId;
        var output = await _cache.GetRecordAsync<MasterPasswordModel>(key);
        if (output is null)
        {
            var results = await _passwords.FindAsync(p => p.User.Id == userId);
            output = await results.FirstOrDefaultAsync();

            await _cache.SetRecordAsync(key, output, TimeSpan.FromHours(10), TimeSpan.FromHours(5));
        }

        return output;
    }

    public async Task<MasterPasswordModel> CreateMasterPasswordAsync(MasterPasswordModel password)
    {
        password.HashedPassword = _hasher.HashPlainText(password.HashedPassword);
        await _passwords.InsertOneAsync(password);

        return password;
    }

    public async Task UpdateMasterPasswordAsync(MasterPasswordModel password)
    {
        string key = CacheNamePrefix + password.User.Id;

        await _passwords.ReplaceOneAsync(p => p.Id == password.Id, password);
        await _cache.RemoveAsync(key);
    }
}
