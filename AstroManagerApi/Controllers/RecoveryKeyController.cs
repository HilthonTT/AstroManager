namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecoveryKeyController : CustomController<RecoveryKeyController>
{
    private readonly IRecoveryKeyData _recoveryKeyData;

    public RecoveryKeyController(
        ILogger<RecoveryKeyController> logger,
        IRecoveryKeyData recoveryKeyData) : base(logger)
    {
        _recoveryKeyData = recoveryKeyData;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUsersRecoveryKeyAsync(string userId)
    {
        try
        {
            LogRequestSource();

            var recoveryKey = await _recoveryKeyData.GetUsersRecoveryKeyAsync(userId);
            if (recoveryKey is null)
            {
                return NotFound("The recovery key has not been found.");
            }

            return Ok(recoveryKey);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecoveryKeyAsync([FromBody] UserModel user)
    {
        try
        {
            LogRequestSource();

            var createdRecoveryKeys = await _recoveryKeyData.CreateRecoveryKeysAsync(user);

            return Ok(createdRecoveryKeys);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
