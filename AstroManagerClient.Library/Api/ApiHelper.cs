using AstroManagerClient.Library.Api.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace AstroManagerClient.Library.Api;
public class ApiHelper : IApiHelper
{
    private readonly IConfiguration _config;

    public ApiHelper(IConfiguration config)
    {
        _config = config;
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
}
