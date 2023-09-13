using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class UserEndpoint : IUserEndpoint
{
    private const string CacheName = nameof(UserEndpoint);
    private const string Uri = "user";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public UserEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var output = await _storage.GetRecordAsync<List<UserModel>>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync(Uri);
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<List<UserModel>>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromHours(1));
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }

        return output;
    }

    public async Task<UserModel> GetUserAsync(string id)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<UserModel> GetUserFromAuthAsync(string oid)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/auth{oid}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<string> UpdateUserAsync(UserModel user)
    {
        using var response = await _api.HttpClient.PutAsJsonAsync(Uri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
