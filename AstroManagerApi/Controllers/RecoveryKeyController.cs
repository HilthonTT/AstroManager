using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecoveryKeyController : ControllerBase
{
    private readonly ILogger<RecoveryKeyController> _logger;
    private readonly IRecoveryKeyData _recoveryKeyData;

    public RecoveryKeyController(
        ILogger<RecoveryKeyController> logger,
        IRecoveryKeyData recoveryKeyData)
    {
        _logger = logger;
        _recoveryKeyData = recoveryKeyData;
    }

    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetAll => "There has been an issue while fetching the recovery keys",
            Operation.Create => "There has been an issue while creating the recovery keys",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetRecoveryKeysByUserId(int userId)
    {
        try
        {
            var keys = await _recoveryKeyData.GetRecoveryKeysByUserIdAsync(userId);
            return Ok(keys);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetAll);
        }
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateRecoveryKeys(int userId)
    {
        try
        {
            await _recoveryKeyData.CreateRecoveryKeysAsync(userId);

            var keys = await _recoveryKeyData.GetRecoveryKeysByUserIdAsync(userId);
            return Ok(keys);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }
}
