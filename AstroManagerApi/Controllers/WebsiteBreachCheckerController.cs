using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WebsiteBreachCheckerController : ControllerBase
{
    private readonly IHttpClientFactory _http;

    public WebsiteBreachCheckerController(IHttpClientFactory http)
    {
        _http = http;
    }
}
