namespace AstroManagerApi.Library.Extensions.Interfaces;

public interface IDistributedCacheHelper
{
    Task<T> GetRecordAsync<T>(string recordId);
    Task RemoveAsync(string recordId);
    Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
}