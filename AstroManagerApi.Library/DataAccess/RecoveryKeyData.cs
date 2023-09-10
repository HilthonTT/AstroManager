using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Helpers;
using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;
using Dapper;

namespace AstroManagerApi.Library.DataAccess;
public class RecoveryKeyData : IRecoveryKeyData
{
    private readonly ISqlDataAccess _sql;
    private readonly IRecoveryKeyGenerator _recoveryKeyGenerator;

    public RecoveryKeyData(ISqlDataAccess sql, IRecoveryKeyGenerator recoveryKeyGenerator)
    {
        _sql = sql;
        _recoveryKeyGenerator = recoveryKeyGenerator;
    }

    private static DynamicParameters GetUserIdParameters(int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        return parameters;
    }

    private static DynamicParameters GetCreateParameters(int userId, string hashedKey)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("HashedKey", hashedKey);

        return parameters;
    }

    public async Task<List<RecoveryKeyModel>> GetRecoveryKeysByUserIdAsync(int userId)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.RecoveryKey, Operation.GetByUserId);
        var parameters = GetUserIdParameters(userId);

        return await _sql.LoadDataAsync<RecoveryKeyModel>(storedProcedure, parameters);
    }

    public async Task<RecoveryRequestModel> CreateRecoveryKeysAsync(int userId)
    {
        try
        {
            _sql.StartTransaction();

            string storedProcedure = SqlHelper.GetStoredProcedure(DataType.RecoveryKey, Operation.Create);
            var recoveryKeys = _recoveryKeyGenerator.GenerateRequest();

            foreach (string r in recoveryKeys.HashedRecoveryKeys)
            {
                var parameters = GetCreateParameters(userId, r);
                await _sql.SaveDataAsync(storedProcedure, parameters);
            }

            _sql.CommitTransaction();

            return recoveryKeys;
        }
        catch (Exception ex)
        {
            _sql.RollbackTransaction();
            throw new Exception(ex.Message);
        }
    }
}
