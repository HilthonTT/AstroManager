using System.Text.Json;
using AstroManagerClient.Library.Storage.Interfaces;

namespace AstroManagerClient.Library.Storage;
public class SecureStorageWrapper : ISecureStorageWrapper
{
    public async Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null)
    {
        absoluteExpireTime ??= TimeSpan.FromMinutes(20);
        var cachedData = new CachedData<T>()
        {
            Data = data,
            Expiration = DateTimeOffset.UtcNow.Add(absoluteExpireTime.GetValueOrDefault()),
        };

        string jsonData = JsonSerializer.Serialize(cachedData);
        await SecureStorage.SetAsync(recordId, jsonData);
    }

    public async Task<T> GetRecordAsync<T>(string recordId)
    {
        string jsonData = await SecureStorage.GetAsync(recordId);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return default;
        }

        var cachedData = JsonSerializer.Deserialize<CachedData<T>>(jsonData);

        if (cachedData?.Expiration > DateTimeOffset.UtcNow)
        {
            return default;
        }

        return cachedData.Data;
    }
}
