using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Helpers;
using AstroManagerApi.Library.Models;
using Dapper;

namespace AstroManagerApi.Library.DataAccess;
public class MasterPasswordData : IMasterPasswordData
{
    private readonly ISqlDataAccess _sql;
    private readonly ITextHasher _hasher;

    public MasterPasswordData(ISqlDataAccess sql, ITextHasher hasher)
    {
        _sql = sql;
        _hasher = hasher;
    }

    private static DynamicParameters GetUserIdParameters(int userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);

        return parameters;
    }

    private DynamicParameters GetCreateParameters(MasterPasswordModel master)
    {
        string hashedPassword = _hasher.HashPlainText(master.HashedPassword);

        var parameters = new DynamicParameters();
        parameters.Add("UserId", master.UserId);
        parameters.Add("HashedPassword", hashedPassword);

        return parameters;
    }

    private DynamicParameters GetUpdateParameters(MasterPasswordModel master)
    {
        string hashedPassword = _hasher.HashPlainText(master.HashedPassword);

        var parameters = new DynamicParameters();
        parameters.Add("Id", master.Id);
        parameters.Add("HashedPassword", hashedPassword);

        return parameters;
    }

    public async Task<MasterPasswordModel> GetMasterPasswordByUserIdAsync(int userId)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.MasterPassword, Operation.GetByUserId);
        var parameters = GetUserIdParameters(userId);

        return await _sql.LoadFirstOrDefaultDataAsync<MasterPasswordModel>(storedProcedure, parameters);
    }

    public async Task CreateMasterPasswordAsync(MasterPasswordModel master)
    {
        var existingMasterPassword = await GetMasterPasswordByUserIdAsync(master.UserId);

        if (existingMasterPassword is not null)
        {
            throw new Exception("The master password already exists.");
        }

        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.MasterPassword, Operation.Create);
        var parameters = GetCreateParameters(master);

        await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task UpdateMasterPasswordAsync(MasterPasswordModel master)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.MasterPassword, Operation.Update);
        var parameters = GetUpdateParameters(master);

        await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
