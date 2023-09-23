using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoRecoveryKeyData : IRecoveryKeyData
{
    private const string CacheNamePrefix = $"{nameof(MongoRecoveryKeyData)}_";
    private readonly IMongoCollection<RecoveryKeyModel> _recoveryKeys;
    private readonly IUserData _userData;
    private readonly IDistributedCache _cache;
    private readonly IRecoveryKeyGenerator _keyGenerator;

    public MongoRecoveryKeyData(
        IDbConnection db,
        IUserData userData,
        IDistributedCache cache,
        IRecoveryKeyGenerator keyGenerator)
    {
        _recoveryKeys = db.RecoveryKeyCollection;
        _userData = userData;
        _cache = cache;
        _keyGenerator = keyGenerator;
    }
    
    public async Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId)
    {
        string key = CacheNamePrefix + userId;
        var output = await _cache.GetRecordAsync<RecoveryKeyModel>(key);
        if (output is null)
        {
            var results = await _recoveryKeys.FindAsync(r => r.User.Id == userId);
            output = await results.FirstOrDefaultAsync();

            await _cache.SetRecordAsync(key, output, TimeSpan.FromDays(1), TimeSpan.FromHours(12));
        }

        return output;
    }

    public async Task<RecoveryRequestModel> CreateRecoveryKeysAsync(UserModel user)
    {
        string key = CacheNamePrefix + user.Id;

        var recoveryRequest = _keyGenerator.GenerateRequest();
        recoveryRequest.Recovery.User = new BasicUserModel(user);
        var recoveryKey = new RecoveryKeyModel(recoveryRequest);

        await _recoveryKeys.InsertOneAsync(recoveryKey);
        await _cache.RemoveAsync(key);

        recoveryRequest.Recovery = recoveryKey;

        return recoveryRequest;
    }
}
