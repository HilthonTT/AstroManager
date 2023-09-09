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
    private static IDbConnection _connection;
    private static IDbTransaction _transaction;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
        _connectionString = GetConnectionString();
    }

    private string GetConnectionString()
    {
        return _config.GetConnectionString(DatabaseName);
    }

    private static void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();

        _transaction = null;
        _connection = null;
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

    public async Task SaveDataInTransactionAsync(string storedProcedure, DynamicParameters parameters = default)
    {
        await _connection?.ExecuteAsync(storedProcedure, parameters, 
            commandType: CommandType.StoredProcedure, transaction: _transaction);
    }

    public async Task<List<T>> LoadDataInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters = default)
    {
        var rows = await _connection?.QueryAsync<T>(storedProcedure, parameters, 
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return rows.ToList();
    }

    public async Task<T> LoadFirstOrDefaultInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters = null)
    {
        var element = await _connection?.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return element;
    }

    public void StartTransaction()
    {
        _connection = new SqlConnection(_connectionString);
        _connection.Open();

        _transaction = _connection.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();

        Dispose();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();

        Dispose();
    }
}
