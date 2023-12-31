﻿namespace AstroManagerClient.Library.Api;
public class PasswordBreacherEndpoint : IPasswordBreacherEndpoint
{
    private const string CacheName = nameof(PasswordBreacherEndpoint);
    private const string ApiUrl = "/api/PasswordBreacher";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public PasswordBreacherEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<List<CredentialModel>> GetBreachedCredentialsAsync(List<CredentialModel> credentials)
    {
        var output = await _storage.GetRecordAsync<List<CredentialModel>>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.PostAsJsonAsync(ApiUrl, credentials);
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<List<CredentialModel>>();
            await _storage.SetRecordAsync(CacheName, output);

            return output;
        }

        return _api.ServerError<List<CredentialModel>>(response);
    }
}
