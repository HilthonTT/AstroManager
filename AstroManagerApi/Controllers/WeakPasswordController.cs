using AstroManagerApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WeakPasswordController : CustomController<WeakPasswordController>
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public WeakPasswordController(
        IHttpClientFactory http,
        IConfiguration config,
        ILogger<WeakPasswordController> logger) : base(logger)
    {
        _httpClient = http.CreateClient();
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeakPasswordsAsync()
    {
        try
        {
            var passwordListUrlsSection = _config.GetSection("PasswordListUrls");

            var passwordListUrls = passwordListUrlsSection
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();

            var passwords = new List<string>();

            await Parallel.ForEachAsync(passwordListUrls, async (url, token) =>
            {
                string passwordListContent = await _httpClient.GetStringAsync(url, token);

                passwords.AddRange(passwordListContent.Split('\n', StringSplitOptions.RemoveEmptyEntries));
            });

            return Ok(passwords.Distinct().ToList());
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
