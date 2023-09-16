﻿using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class MasterPasswordEndpoint : IMasterPasswordEndpoint
{
    private const string CacheName = nameof(MasterPasswordEndpoint);
    private const string Uri = "api/masterPassword";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public MasterPasswordEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<MasterPasswordModel> GetUsersMasterPasswordAsync(string userId)
    {
        var output = await _storage.GetRecordAsync<MasterPasswordModel>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync(Uri);
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<MasterPasswordModel>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromDays(5));

            return output;
        }
        else
        {
            return _api.NotFoundError<MasterPasswordModel>(response);
        }
    }

    public async Task<MasterPasswordModel> CreateMasterPasswordAsync(MasterPasswordModel password)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, password);
        if (response.IsSuccessStatusCode)
        {
            SecureStorage.Remove(CacheName);
            return await response.Content.ReadFromJsonAsync<MasterPasswordModel>();
        }
        else
        {
            return _api.NotFoundError<MasterPasswordModel>(response);
        }
    }

    public async Task<string> UpdateMasterPasswordAsync(PasswordResetModel password)
    {
        using var response = await _api.HttpClient.PutAsJsonAsync(Uri, password);
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

    public async Task<bool> VerifyPasswordAsync(string userId, string password)
    {
        using var response = await _api.HttpClient.PostAsync($"{Uri}/verify/{userId}/{password}", null);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}