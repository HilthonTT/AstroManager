﻿using AstroManagerApi.Common;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PasswordBreacherController : CustomController<PasswordBreacherController>
{
    private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
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

    private static async Task<string> ComputeSHA1HashAsync(string input)
    {
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        byte[] hashBytes = await SHA1.HashDataAsync(memoryStream);
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

    private static string GenerateRandomPassword(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var password = new char[length];
        for (int i = 0; i < length; i++)
        {
            int index = bytes[i] % ValidChars.Length;
            password[i] = ValidChars[index];
        }

        return new string(password);
    }

    private static async Task<(string sha1Hash, string sha1HashPrefix, string sha1HashSuffix)> GenerateSHA1HashAndPrefixSuffixAsync(string input)
    {
        string sha1Hash = await ComputeSHA1HashAsync(input);
        string sha1HashPrefix = sha1Hash.Substring(0, 5);
        string sha1HashSuffix = sha1Hash.Substring(5);

        return (sha1Hash, sha1HashPrefix, sha1HashSuffix);
    }


    [HttpPost]
    public async Task<IActionResult> CheckBreachAsync([FromBody] List<CredentialModel> credentials)
    {
        try
        {
            LogRequestSource();

            var breachedAccounts = new ConcurrentBag<CredentialModel>();

            await Parallel.ForEachAsync(credentials, async (credential, token) =>
            {
                var passwordField = credential.Fields.FirstOrDefault(x => x.Name.Equals(
                    "Password", StringComparison.InvariantCultureIgnoreCase));

                if (string.IsNullOrWhiteSpace(passwordField?.Value) is false)
                {
                    var (sha1Hash, sha1HashPrefix, sha1HashSuffix) = await GenerateSHA1HashAndPrefixSuffixAsync(passwordField.Value);

                    bool isPasswordCompromised = await CheckPasswordBreachAsync(sha1HashPrefix, sha1HashSuffix);

                    if (isPasswordCompromised)
                    {
                        breachedAccounts.Add(credential);
                    }
                }
            });

            return Ok(breachedAccounts.ToList());
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpGet("{length}")]
    public async Task<IActionResult> GeneratePasswordAsync(int length)
    {
        try
        {
           while (true)
           {
                string password = GenerateRandomPassword(length);

                var (sha1Hash, sha1HashPrefix, sha1HashSuffix) = await GenerateSHA1HashAndPrefixSuffixAsync(password);
                bool isPasswordCompromised = await CheckPasswordBreachAsync(sha1HashPrefix, sha1HashSuffix);

                if (isPasswordCompromised is false)
                {
                    return Ok(password);
                }           
           }
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
