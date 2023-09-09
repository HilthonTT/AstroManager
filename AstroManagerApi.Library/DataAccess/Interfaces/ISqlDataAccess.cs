using Dapper;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ISqlDataAccess
{
    Task<List<T>> LoadDataAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    Task<T> LoadFirstOrDefaultDataAsync<T>(string storedProcedure, DynamicParameters parameters = null);
    Task SaveDataAsync(string storedProcedure, DynamicParameters parameters = null);
}