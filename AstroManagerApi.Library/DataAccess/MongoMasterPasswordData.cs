﻿using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Extensions.Interfaces;
using AstroManagerApi.Library.Models;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoMasterPasswordData : IMasterPasswordData
{
    private const string CacheNamePrefix = $"{nameof(MongoMasterPasswordData)}_";
    private readonly IMongoCollection<MasterPasswordModel> _passwords;
    private readonly IDbConnection _db;
    private readonly IRecoveryKeyGenerator _keyGenerator;
    private readonly IDistributedCacheHelper _cache;

    public MongoMasterPasswordData(
        IDbConnection db,
        IRecoveryKeyGenerator keyGenerator,
        IDistributedCacheHelper cache)
    {
        _passwords = db.MasterPasswordCollection;
        _db = db;
        _keyGenerator = keyGenerator;
        _cache = cache;
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
        var client = _db.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var passwordInTransaction = db.GetCollection<MasterPasswordModel>(_db.MasterPasswordCollectionName);

            await passwordInTransaction.InsertOneAsync(session, password);

            var recoveryKeyInTransaction = db.GetCollection<RecoveryKeyModel>(_db.RecoveryKeyCollectionName);
            var recoveryRequest = _keyGenerator.GenerateRequest();
            recoveryRequest.Recovery.User = password.User;
            var recoveryKey = new RecoveryKeyModel(recoveryRequest);

            await recoveryKeyInTransaction.InsertOneAsync(session, recoveryKey);

            await session.CommitTransactionAsync();

            return password;
        }
        catch (Exception)
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task UpdateMasterPasswordAsync(MasterPasswordModel password)
    {
        string key = CacheNamePrefix + password.User.Id;

        await _passwords.ReplaceOneAsync(p => p.Id == password.Id, password);
        await _cache.RemoveAsync(key);
    }
}
