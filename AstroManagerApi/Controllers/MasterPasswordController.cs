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
    public async Task<IActionResult> UpdateMasterPasswordAsync([FromBody] PasswordResetModel reset)
    {
        try
        {
            LogRequestSource();

            if (reset.RecoveryKeys.Count <= 0)
            {
                return await ResetMasterPassword(reset.Master);
            }
            else
            {
                return await ResetMasterPasswordWithRecoveryKeys(reset);
            }
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    private async Task<IActionResult> ResetMasterPassword(MasterPasswordModel master)
    {
        var userMasterPassword = await _passwordData.GetUsersMasterPasswordAsync(master.User.Id);
        string inputedPassword = master.HashedPassword;

        userMasterPassword.HashedPassword = _hasher.HashPlainText(inputedPassword);
        await _passwordData.UpdateMasterPasswordAsync(userMasterPassword);

        return Ok("Master Password resetted.");
    }

    private async Task<IActionResult> ResetMasterPasswordWithRecoveryKeys(PasswordResetModel reset)
    {
        var recoveryKey = await _recoveryKeyData.GetUsersRecoveryKeyAsync(reset.Master.User.Id);
        var hashedResetRecoveryKeys = reset.RecoveryKeys.Select(key => _hasher.HashPlainText(key)).ToHashSet();

        if (HashSet<string>.CreateSetComparer().Equals(hashedResetRecoveryKeys, recoveryKey.RecoveryKeys))
        {
            await _passwordData.UpdateMasterPasswordAsync(reset.Master);
            return Ok("Master password resetted.");
        }
        else
        {
            return BadRequest("Your recovery keys do not match.");
        }
    }

    [HttpPost("verify/{userId}/{password}")]
    public async Task<IActionResult> VerifyPasswordAsync(string userId, string password)
    {
        try
        {
            LogRequestSource();

            var hashedMasterPassword = await _passwordData.GetUsersMasterPasswordAsync(userId);
            bool isCorrect = _hasher.VerifyPassword(password, hashedMasterPassword.HashedPassword);

            return Ok(isCorrect);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
