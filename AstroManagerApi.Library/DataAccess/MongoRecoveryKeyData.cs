namespace AstroManagerApi.Library.DataAccess;
public class MongoRecoveryKeyData : IRecoveryKeyData
{
    private const string CacheNamePrefix = $"{nameof(MongoRecoveryKeyData)}_";
    private readonly IMongoCollection<RecoveryKeyModel> _recoveryKeys;
    private readonly IDistributedCacheHelper _cache;
    private readonly IRecoveryKeyGenerator _keyGenerator;
    private readonly ITextHasher _hasher;

    public MongoRecoveryKeyData(
        IDbConnection db,
        IDistributedCacheHelper cache,
        IRecoveryKeyGenerator keyGenerator,
        ITextHasher hasher)
    {
        _recoveryKeys = db.RecoveryKeyCollection;
        _cache = cache;
        _keyGenerator = keyGenerator;
        _hasher = hasher;
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

    public async Task<RecoveryKeyModel> CreateRecoveryKeysAsync(UserModel user)
    {
        string key = CacheNamePrefix + user.Id;

        var keyHashSet = _keyGenerator.GenerateBase64Keys();
        var recovery = new RecoveryKeyModel
        {
            User = new(user),
            RecoveryKeys = keyHashSet,
        };

        var hashedRecovery = _hasher.HashRecoveryKeys(recovery);

        await _recoveryKeys.InsertOneAsync(hashedRecovery);
        await _cache.RemoveAsync(key);

        recovery.Id = hashedRecovery.Id;
        return recovery;
    }
}
