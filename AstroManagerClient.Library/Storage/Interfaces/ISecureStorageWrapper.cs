namespace AstroManagerClient.Library.Storage.Interfaces;

public interface ISecureStorageWrapper
{
    Task<T> GetRecordAsync<T>(string recordId);
    Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null);
}