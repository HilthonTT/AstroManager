using AstroManagerApi.Common;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Models;
using AstroManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MasterPasswordController : CustomController<MasterPasswordController>
{
    private readonly IMasterPasswordData _passwordData;
    private readonly IRecoveryKeyData _recoveryKeyData;
    private readonly ITextHasher _hasher;

    public MasterPasswordController(
        ILogger<MasterPasswordController> logger,
        IMasterPasswordData passwordData,
        IRecoveryKeyData recoveryKeyData,
        ITextHasher hasher) : base(logger)
    {
        _passwordData = passwordData;
        _recoveryKeyData = recoveryKeyData;
        _hasher = hasher;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUsersMastersPasswordAsync(string userId)
    {
        try
        {
            LogRequestSource();

            var masterPassword = await _passwordData.GetUsersMasterPasswordAsync(userId);
            return Ok(masterPassword);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMasterPasswordAsync([FromBody] MasterPasswordModel password)
    {
        try
        {
            LogRequestSource();

            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var existingMasterPassword = await _passwordData.GetUsersMasterPasswordAsync(password.User.Id);
            if (existingMasterPassword is not null)
            {
                return BadRequest("You already have a master password defined.");
            }

            var createdPassword = await _passwordData.CreateMasterPasswordAsync(password);
            return Ok(createdPassword);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMasterPassswordAsync([FromBody] PasswordResetModel reset)
    {
        try
        {
            LogRequestSource();

            var recoveryKey = await _recoveryKeyData.GetUsersRecoveryKeyAsync(reset.Master.User.Id);
            var hashedResetRecoveryKeys = new HashSet<string>();

            foreach (var resetRecoveryKey in reset.RecoveryKeys)
            {
                hashedResetRecoveryKeys.Add(_hasher.HashPlainText(resetRecoveryKey));
            }

            bool recoveryKeysMatch = HashSet<string>
                .CreateSetComparer()
                .Equals(hashedResetRecoveryKeys, recoveryKey.RecoveryKeys);

            if (recoveryKeysMatch)
            {
                await _passwordData.UpdateMasterPasswordAsync(reset.Master);
                return Ok("Master password resetted.");
            }
            else
            {
                return BadRequest("Your recovery keys do not match.");
            }
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
