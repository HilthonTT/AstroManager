using Dapper;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ISqlDataAccess
{
    void CommitTransaction();
    Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    Task<List<T>> LoadDataInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    Task<T> LoadFirstOrDefaultDataAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    Task<T> LoadFirstOrDefaultInTransactionAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    void RollbackTransaction();
    Task SaveDataAsync(string storedProcedure, DynamicParameters parameters = null);
    Task SaveDataInTransactionAsync(string storedProcedure, DynamicParameters parameters = null);
    void StartTransaction();
}