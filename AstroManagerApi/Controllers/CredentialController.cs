namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CredentialController : CustomController<CredentialController>
{
    private readonly ICredentialData _credentialData;

    public CredentialController(
        ILogger<CredentialController> logger,
        ICredentialData credentialData) : base(logger)
    {
        _credentialData = credentialData;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUsersCredentialsAsync(string userId)
    {
        try
        {
            LogRequestSource();

            var credentials = await _credentialData.GetUsersCredentialsAsync(userId);
            if (credentials is null)
            {
                return NotFound("Your credentials do not exist.");
            }

            return Ok(credentials);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCredentialAsync(string id)
    {
        try
        {
            LogRequestSource();

            var credential = await _credentialData.GetCredentialAsync(id);
            if (credential is null)
            {
                return NotFound("The credential has not been found.");
            }

            return Ok(credential);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCredentialAsync([FromBody] CredentialModel credential)
    {
        try
        {
            LogRequestSource();
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var createdCredential = await _credentialData.CreateCredentialAsync(credential);
            return Ok(createdCredential);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCredentialAsync([FromBody] CredentialModel credential)
    {
        try
        {
            LogRequestSource();

            await _credentialData.UpdateCredentialAsync(credential);
            return Ok("Updated the credential.");
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCredentialAsycn(string id)
    {
        try
        {
            LogRequestSource();

            await _credentialData.DeleteCredentialAsync(id);
            return Ok("Deleted credential.");
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
