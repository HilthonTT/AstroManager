﻿namespace AstroManagerClient.Library.Api;
public class CredentialTemplateEndpoint : ICredentialTemplateEndpoint
{
    private const string CacheName = nameof(CredentialTemplateEndpoint);
    private const string Uri = "api/credentialTemplate";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public CredentialTemplateEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<List<CredentialTemplateModel>> GetAllTemplatesAsync()
    {
        var output = await _storage.GetRecordAsync<List<CredentialTemplateModel>>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync(Uri);
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<List<CredentialTemplateModel>>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromDays(1));

            return output;
        }

        return _api.ServerError<List<CredentialTemplateModel>>(response);
    }

    public async Task<CredentialTemplateModel> GetTemplateAsync(string id)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CredentialTemplateModel>();
        }
        
        return _api.ServerError<CredentialTemplateModel>(response);
    }

    public async Task<CredentialTemplateModel> CreateTemplateAsync(CredentialTemplateModel template)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, template);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CredentialTemplateModel>();
        }

        return _api.ServerError<CredentialTemplateModel>(response);
    }
}
