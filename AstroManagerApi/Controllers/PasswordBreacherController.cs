using AstroManagerApi.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PasswordBreacherController : CustomController<PasswordBreacherController>
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public PasswordBreacherController(
        IHttpClientFactory http,
        IConfiguration config,
        ILogger<PasswordBreacherController> logger) : base(logger)
    {
        _httpClient = http.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AstroManager");
        _config = config;
    }

    private static string ComputeSHA1Hash(string input)
    {
        byte[] hashBytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    private async Task<bool> CheckPasswordBreachAsync(string sha1HashPrefix, string sha1HashSuffix)
    {
        try
        {
            string apiUrl = $"{_config["HIBPApiBaseUrl"]}/range/{sha1HashPrefix}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                string suffixToCheck = sha1HashSuffix.ToUpper();
                return responseContent.Contains(suffixToCheck);
            }

            _logger.LogError("HIBP API Error: {r}", response.ReasonPhrase);
            return false;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("HTTP Error: {ex}", e.Message);
            return false;
        }
        catch (Exception ex)
        {
            ServerErrorCode(ex);
            return false;
        }
    }

    [HttpGet("{password}")]
    public async Task<IActionResult> CheckBreachAsync(string password)
    {
        try
        {
            LogRequestSource();

            string sha1Hash = ComputeSHA1Hash(password);
            string sha1HashPrefix = sha1Hash.Substring(0, 5);
            string sha1HashSuffix = sha1Hash.Substring(5);

            bool isPasswordCompromised = await CheckPasswordBreachAsync(sha1HashPrefix, sha1HashSuffix);

            if (isPasswordCompromised)
            {
                return Ok("This password has been compromised.");
            }

            return Ok("This password has not been compromised.");
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
