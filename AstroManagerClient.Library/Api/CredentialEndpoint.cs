using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class CredentialEndpoint : ICredentialEndpoint
{
    private const string CacheName = nameof(CredentialEndpoint);
    private const string Uri = "api/credential";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public CredentialEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<List<CredentialModel>> GetUsersCredentialsAsync(string userId)
    {
        var output = await _storage.GetRecordAsync<List<CredentialModel>>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync($"{Uri}/user/{userId}");
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<List<CredentialModel>>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromHours(10));

            return output;
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<CredentialModel> GetCredentialAsync(string id)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CredentialModel>();
        }
        else
        {
            return _api.NotFoundError<CredentialModel>(response);
        }
    }

    public async Task<CredentialModel> CreateCredentialAsync(CredentialModel credential)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, credential);
        if (response.IsSuccessStatusCode)
        {
            SecureStorage.Remove(CacheName);
            return await response.Content.ReadFromJsonAsync<CredentialModel>();
        }
        else
        {
            return _api.NotFoundError<CredentialModel>(response);
        }
    }

    public async Task<string> UpdateCredentialAsync(CredentialModel credential)
    {
        using var response = await _api.HttpClient.PutAsJsonAsync(Uri, credential);
        if (response.IsSuccessStatusCode)
        {
            SecureStorage.Remove(CacheName);
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task<string> DeleteCredentialAsync(CredentialModel credential)
    {
        using var response = await _api.HttpClient.DeleteAsync($"{Uri}/{credential.Id}");
        if (response.IsSuccessStatusCode)
        {
            SecureStorage.Remove(CacheName);
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
