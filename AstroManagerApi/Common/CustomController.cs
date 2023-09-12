using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Common;
public class CustomController<T> : ControllerBase
{
    protected readonly ILogger<T> _logger;

    public CustomController(ILogger<T> logger)
    {
        _logger = logger;
    }

    protected void LogRequestSource()
    {
        _logger.LogInformation("Received request: {Method} {Path} from {RemoteIpAddress} at {DateTime}",
            HttpContext.Request.Method,
            HttpContext.Request.Path,
            HttpContext.Connection.RemoteIpAddress,
            DateTime.UtcNow);
    }

    protected ObjectResult ServerErrorCode(Exception ex)
    {
        _logger.LogError("An error occured: {ex}.", ex.Message);
        return StatusCode(500, "An error occured while processing the request.");
    }
}
