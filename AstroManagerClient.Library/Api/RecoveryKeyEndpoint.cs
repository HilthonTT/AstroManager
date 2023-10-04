using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class RecoveryKeyEndpoint : IRecoveryKeyEndpoint
{
    private const string CacheName = nameof(RecoveryKeyEndpoint);
    private const string Uri = "api/recoveryKey";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public RecoveryKeyEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId)
    {
        var output = await _storage.GetRecordAsync<RecoveryKeyModel>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync($"{Uri}/{userId}");
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<RecoveryKeyModel>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromDays(1));

            return output;
        }

        return _api.ServerError<RecoveryKeyModel>(response);
    }

    public async Task<RecoveryKeyModel> CreateRecoveryKeyAsync(UserModel user)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<RecoveryKeyModel>();
        }

        return _api.ServerError<RecoveryKeyModel>(response);
    }
}
