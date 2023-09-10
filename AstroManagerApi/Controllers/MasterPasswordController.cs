using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Models;
using AstroManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MasterPasswordController : ControllerBase
{
    private readonly ILogger<MasterPasswordController> _logger;
    private readonly IMasterPasswordData _passwordData;
    private readonly IRecoveryKeyData _recoveryKeyData;
    private readonly ITextHasher _hasher;

    public MasterPasswordController(
        ILogger<MasterPasswordController> logger,
        IMasterPasswordData passwordData,
        IRecoveryKeyData recoveryKeyData,
        ITextHasher hasher)
    {
        _logger = logger;
        _passwordData = passwordData;
        _recoveryKeyData = recoveryKeyData;
        _hasher = hasher;
    }

    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetByUserId => "There has been an issue while verifying the password",
            Operation.Create => "There has been an issue while creating the master password",
            Operation.Update => "There has been an issue while updating the master password",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet("{password}/{userId}")]
    public async Task<IActionResult> VerifyMasterPassword(string password, int userId)
    {
        try
        {
            var masterPassword = await _passwordData.GetMasterPasswordByUserIdAsync(userId);
            if (masterPassword is null)
            {
                return NotFound("Not master password has been found");
            }

            bool isCorrect = _hasher.VerifyPassword(password, masterPassword.HashedPassword);
            return Ok(isCorrect);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetByUserId);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMasterPassword(MasterPasswordModel masterPassword)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _passwordData.CreateMasterPasswordAsync(masterPassword);
            return Ok("The master password has been created");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMasterPassword([FromBody] UpdatePasswordRequestModel request)
    {
        try
        {
            if (request.RecoveryKeys?.Count != 3)
            {
                return BadRequest("You must provide exactly 3 recovery keys");
            }

            var recoveryKeys = await _recoveryKeyData.GetRecoveryKeysByUserIdAsync(request.UserId);
            var hashedDatabaseKeys = new HashSet<string>(recoveryKeys.Select(r => r.HashedKey));

            int matchedCount = 0; // Count of matched recovery keys

            foreach (string k in request.RecoveryKeys)
            {
                string hashedKey = _hasher.HashPlainText(k);

                if (hashedDatabaseKeys.Contains(hashedKey))
                {
                    matchedCount++;
                }
            }

            if (matchedCount == 3)
            {
                var masterPassword = await _passwordData.GetMasterPasswordByUserIdAsync(request.UserId);
                masterPassword.HashedPassword = request.NewPassword;

                await _passwordData.UpdateMasterPasswordAsync(masterPassword);

                return Ok("The master password has been updated");
            }

            return BadRequest("The provided recovery keys do not match the keys in the database");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Update);
        }
    }
}
