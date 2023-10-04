using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Storage.Interfaces;
using System.Net.Http.Json;

namespace AstroManagerClient.Library.Api;
public class TypeEndpoint : ITypeEndpoint
{
    private const string CacheName = nameof(TypeEndpoint);
    private const string Uri = "api/type";
    private readonly IApiHelper _api;
    private readonly ISecureStorageWrapper _storage;

    public TypeEndpoint(IApiHelper api, ISecureStorageWrapper storage)
    {
        _api = api;
        _storage = storage;
    }

    public async Task<List<TypeModel>> GetAllTypesAsync()
    {
        var output = await _storage.GetRecordAsync<List<TypeModel>>(CacheName);
        if (output is not null)
        {
            return output;
        }

        using var response = await _api.HttpClient.GetAsync(Uri);
        if (response.IsSuccessStatusCode)
        {
            output = await response.Content.ReadFromJsonAsync<List<TypeModel>>();
            await _storage.SetRecordAsync(CacheName, output, TimeSpan.FromDays(1));

            return output;
        }

        return _api.ServerError<List<TypeModel>>(response);
    }

    public async Task<TypeModel> CreateTypeAsync(TypeModel type)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, type);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TypeModel>();
        }

        return _api.ServerError<TypeModel>(response);
    }
}
