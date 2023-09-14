using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace AstroManagerClient.Library.Api;
public class ApiHelper : IApiHelper
{
    private readonly IConfiguration _config;
    private readonly ILoggedInUser _loggedInUser;

    public ApiHelper(IConfiguration config, ILoggedInUser loggedInUser)
    {
        _config = config;
        _loggedInUser = loggedInUser;
        InitializeClient();
    }

    public HttpClient HttpClient { get; private set; } = new();

    private void SetClientDefaultRequestHeadersJson()
    {
        HttpClient.DefaultRequestHeaders.Accept.Clear();
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private void InitializeClient()
    {
        string apiUrl = _config["api"];

        HttpClient.BaseAddress = new Uri(apiUrl);
        SetClientDefaultRequestHeadersJson();
    }

    public void AcquireHeaders(string token)
    {
        HttpClient.DefaultRequestHeaders.Clear();
        SetClientDefaultRequestHeadersJson();

        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public void Logout()
    {
        HttpClient.DefaultRequestHeaders.Clear();

        _loggedInUser.Id = "";
        _loggedInUser.ObjectIdentifier = "";
        _loggedInUser.FirstName = "";
        _loggedInUser.LastName = "";
        _loggedInUser.DisplayName = "";
        _loggedInUser.EmailAddress = "";
    }

    public T NotFoundError<T>(HttpResponseMessage response)
    {
        if (response.ReasonPhrase.Contains("Not Found"))
        {
            return default;
        }
        else
        {
            throw new Exception(response.ReasonPhrase);
        }
    }
}
