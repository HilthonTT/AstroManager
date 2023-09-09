using AstroManagerApi.Library.DataAccess.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AstroManagerApi.Library.DataAccess;
public class SqlDataAccess : ISqlDataAccess
{
    private const string DatabaseName = "AstroManagerData";
    private readonly string _connectionString;
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
        _connectionString = GetConnectionString();
    }

    private string GetConnectionString()
    {
        return _config.GetConnectionString(DatabaseName);
    }

    public async Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters = default)
    {
        using var connection = new SqlConnection(_connectionString);

        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task<T> LoadFirstOrDefaultDataAsync<T>(string storedProcedure, DynamicParameters parameters = default)
    {
        using var connection = new SqlConnection(_connectionString);

        var element = await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return element;
    }

    public async Task SaveDataAsync(string storedProcedure, DynamicParameters parameters = default)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }
}
