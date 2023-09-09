using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Helpers;
using AstroManagerApi.Library.Models;
using Dapper;

namespace AstroManagerApi.Library.DataAccess;
public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;

    public UserData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    private static DynamicParameters GetOidParameters(string oid)
    {
        var parameters = new DynamicParameters();
        parameters.Add("ObjectIdentifier", oid);

        return parameters;
    }

    private static DynamicParameters GetCreateParameters(UserModel user)
    {
        var parameters = GetOidParameters(user.ObjectIdentifier);
        parameters.Add("ObjectIdentifier", user.ObjectIdentifier);
        parameters.Add("FirstName", user.FirstName);
        parameters.Add("LastName", user.LastName);
        parameters.Add("EmailAddress", user.EmailAddress);

        return parameters;
    }

    private static DynamicParameters GetUpdateParameters(UserModel user)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id);
        parameters.Add("FirstName", user.FirstName);
        parameters.Add("LastName", user.LastName);
        parameters.Add("EmailAddress", user.EmailAddress);

        return parameters;
    }

    public async Task<UserModel> GetUserByOidAsync(string oid)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.User, Operation.GetByOid);
        var parameters = GetOidParameters(oid);

        return await _sql.LoadFirstOrDefaultDataAsync<UserModel>(storedProcedure, parameters);
    }

    public async Task CreateUserAsync(UserModel user)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.User, Operation.Create);
        var parameters = GetCreateParameters(user);

        await _sql.SaveDataAsync(storedProcedure, parameters);
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.User, Operation.Update);
        var parameters = GetUpdateParameters(user);

        await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
