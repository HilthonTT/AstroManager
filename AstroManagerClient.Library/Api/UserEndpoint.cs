namespace AstroManagerClient.Library.Api;
public class UserEndpoint : IUserEndpoint
{
    private const string CacheName = nameof(UserEndpoint);
    private const string Uri = "api/user";
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

            return output;
        }

        return _api.ServerError<List<UserModel>>(response);
    }

    public async Task<UserModel> GetUserAsync(string id)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        return _api.ServerError<UserModel>(response);
    }

    public async Task<UserModel> GetUserFromAuthAsync(string oid)
    {
        using var response = await _api.HttpClient.GetAsync($"{Uri}/auth/{oid}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        return _api.ServerError<UserModel>(response);
    }

    public async Task<UserModel> CreateUserAsync(UserModel user)
    {
        using var response = await _api.HttpClient.PostAsJsonAsync(Uri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        return _api.ServerError<UserModel>(response);
    }

    public async Task<string> UpdateUserAsync(UserModel user)
    {
        using var response = await _api.HttpClient.PutAsJsonAsync(Uri, user);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return _api.ServerError<string>(response);
    }
}
