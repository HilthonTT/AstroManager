using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Helpers;
using AstroManagerApi.Library.Models;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;

namespace AstroManagerApi.Library.DataAccess;
public class EntityAttributeData : IEntityAttributeData
{
    private const string CacheNamePrefix = $"{nameof(EntityAttributeData)}_";
    private readonly ISqlDataAccess _sql;
    private readonly IDistributedCache _cache;
    private readonly IAesEncryptor _aes;

    public EntityAttributeData(
        ISqlDataAccess sql,
        IDistributedCache cache,
        IAesEncryptor aes)
    {
        _sql = sql;
        _cache = cache;
        _aes = aes;
    }

    private static DynamicParameters GetIdParameters(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        return parameters;
    }

    private static DynamicParameters GetUserIdParameters(int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        return parameters;
    }

    private async Task<DynamicParameters> GetCreateParametersAsync(EntityAttributeModel entity)
    {
        string hashedValue = await _aes.EncryptAsync(entity.Value);

        var parameters = new DynamicParameters();
        parameters.Add("UserId", entity.UserId);
        parameters.Add("EntityId", entity.EntityId);
        parameters.Add("AttributeId", entity.AttributeId);
        parameters.Add("Value", hashedValue);

        return parameters;
    }

    private async Task<DynamicParameters> GetUpdateParametersAsync(EntityAttributeModel entity)
    {
        string hashedValue = await _aes.EncryptAsync(entity.Value);

        var parameters = new DynamicParameters();
        parameters.Add("Id", entity.Id);
        parameters.Add("Value", hashedValue);

        return parameters;
    }

    private async Task RemoveCacheAsync(int userId)
    {
        string key = CacheNamePrefix + userId;
        await _cache.RemoveAsync(key);
    }

    private async Task<List<EntityAttributeModel>> DecryptEntitiesAsync(List<EntityAttributeModel> entities)
    {
        var decryptedEntities = new List<EntityAttributeModel>();

        foreach (var e in entities)
        {
            e.Value = await _aes.DecryptAsync(e.Value);
            decryptedEntities.Add(e);
        }

        return decryptedEntities;
    }

    public async Task<List<EntityAttributeModel>> GetEntityAttributesByUserIdAsync(int userId)
    {
        string key = CacheNamePrefix + userId;
        var output = await _cache.GetRecordAsync<List<EntityAttributeModel>>(key);
        if (output is null)
        {
            string storedProcedure = SqlHelper.GetStoredProcedure(DataType.EntityAttribute, Operation.GetByUserId);
            var parameters = GetUserIdParameters(userId);

            output = await _sql.LoadDataAsync<EntityAttributeModel>(storedProcedure, parameters);
            await _cache.SetRecordAsync(key, output, TimeSpan.FromHours(1), TimeSpan.FromMinutes(30));
        }

        return await DecryptEntitiesAsync(output);
    }

    public async Task<EntityAttributeModel> GetEntityAttributeByIdAsync(int id)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.EntityAttribute, Operation.GetById);
        var parameters = GetIdParameters(id);

        var entity = await _sql.LoadFirstOrDefaultDataAsync<EntityAttributeModel>(storedProcedure, parameters);
        entity.Value = await _aes.DecryptAsync(entity.Value);

        return entity;
    }

    public async Task CreateEntityAttributeAsync(EntityAttributeModel entity)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.EntityAttribute, Operation.Create);
        var parameters = await GetCreateParametersAsync(entity);

        await _sql.SaveDataAsync(storedProcedure, parameters);
        await RemoveCacheAsync(entity.UserId);
    }

    public async Task UpdateEntityAttributeAsync(EntityAttributeModel entity)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.EntityAttribute, Operation.Update);
        var parameters = await GetUpdateParametersAsync(entity);

        await _sql.SaveDataAsync(storedProcedure, parameters);
        await RemoveCacheAsync(entity.UserId);
    }

    public async Task DeleteEntityAttributeAsync(int id, int userId)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.EntityAttribute, Operation.Delete);
        var parameters = GetIdParameters(id);

        await _sql.SaveDataAsync(storedProcedure, parameters);
        await RemoveCacheAsync(userId);
    }
}
