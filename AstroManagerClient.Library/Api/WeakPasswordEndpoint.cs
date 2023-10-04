using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class WeakPasswordEndpoint : IWeakPasswordEndpoint
{
    private const string CacheName = nameof(WeakPasswordEndpoint);
    private const string ApiUrl = "api/WeakPassword";
    private readonly IApiHelper _apiHelper;
    private readonly ISecureStorageWrapper _storage;

    public WeakPasswordEndpoint(IApiHelper apiHelper, ISecureStorageWrapper storage)
    {
        _apiHelper = apiHelper;
        _storage = storage;
    }

    public async Task<List<string>> GetWeakPasswordAsync()
    {
        var weakPasswords = await _storage.GetRecordAsync<List<string>>(CacheName);
        if (weakPasswords is not null)
        {
            return weakPasswords;
        }

        using var response = await _apiHelper.HttpClient.GetAsync(ApiUrl);
        if (response.IsSuccessStatusCode)
        {
            weakPasswords = await response.Content.ReadFromJsonAsync<List<string>>();
            await _storage.SetRecordAsync(CacheName, weakPasswords, TimeSpan.FromHours(5));

            return weakPasswords;
        }

        return _apiHelper.ServerError<List<string>>(response);
    }
}
