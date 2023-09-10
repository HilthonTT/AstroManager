using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserData _userData;

    public UserController(ILogger<UserController> logger, IUserData userData)
    {
        _logger = logger;
        _userData = userData;
    }

    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetByOid => "There has been an issue while fetching the user using the OID",
            Operation.Create => "There has been an issue creating the user",
            Operation.Update => "There has been an issue updating the user",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet("{oid}")]
    public async Task<IActionResult> GetUserByOid(string oid)
    {
        try
        {
            var user = await _userData.GetUserByOidAsync(oid);
            if (user is null)
            {
                return NotFound("No user has been found.");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetByOid);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserModel user)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _userData.CreateUserAsync(user);

            return Ok("The user has been created.");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _userData.UpdateUserAsync(user);

            return Ok("The user has been updated.");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Update);
        }
    }
}
