using AstroManagerApi.Common;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : CustomController
{
    private readonly IUserData _userData;

    public UserController(
        ILogger<UserController> logger,
        IUserData userData) : base(logger)
    {
        _userData = userData;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        try
        {
            LogRequestSource();

            var users = await _userData.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the all users: {ex}.", ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(string id)
    {
        try
        {
            LogRequestSource();

            var user = await _userData.GetUserAsync(id);
            if (user is null)
            {
                return NotFound("No user found.");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the user: {ex}.", ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpGet("auth/{oid}")]
    public async Task<IActionResult> GetUserFromAuthAsync(string oid)
    {
        try
        {
            LogRequestSource();

            var user = await _userData.GetUserFromAuthAsync(oid);
            if (user is null)
            {
                return NotFound("No user found.");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching the user from auth: {ex}.", ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserModel user)
    {
        try
        {
            LogRequestSource();

            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userData.CreateUserAsync(user);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating the user: {ex}.", ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserModel user)
    {
        try
        {
            LogRequestSource();

            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _userData.UpdateUserAsync(user);
            return Ok("The user has been updated.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating the user: {ex}.", ex.Message);
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
}
