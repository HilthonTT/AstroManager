using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EntityAttributeController : ControllerBase
{
    private readonly ILogger<EntityAttributeController> _logger;
    private readonly IEntityAttributeData _entityAttributeData;

    public EntityAttributeController(
        ILogger<EntityAttributeController> logger,
        IEntityAttributeData entityAttributeData)
    {
        _logger = logger;
        _entityAttributeData = entityAttributeData;
    }

    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetByUserId => "There has been an issue while fetching the entity attributes using the UserID",
            Operation.GetById => "There has been an issue while fetching the entity attribute using the ID",
            Operation.Create => "There has been an issue while creating the entity attribute",
            Operation.Update => "There has been an issue while updating the entity attribute",
            Operation.Delete => "There has been an issue while deleting the entity attribute",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetEntityAttributesByUserId(int userId)
    {
        try
        {
            var entityAttributes = await _entityAttributeData.GetEntityAttributesByUserIdAsync(userId);
            return Ok(entityAttributes);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetByUserId);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEntityAttributeById(int id)
    {
        try
        {
            var entityAttribute = await _entityAttributeData.GetEntityAttributeByIdAsync(id);
            if (entityAttribute is null)
            {
                return NotFound("No entity attribute has been found.");
            }

            return Ok(entityAttribute);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetById);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEntityAttribute([FromBody] EntityAttributeModel entity)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _entityAttributeData.CreateEntityAttributeAsync(entity);

            return Ok("The entity attribute has been created");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEntityAttribute([FromBody] EntityAttributeModel entity)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _entityAttributeData.UpdateEntityAttributeAsync(entity);

            return Ok("The entity attribute has been updated");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Update);
        }
    }

    [HttpDelete("{id}/{userId}")]
    public async Task<IActionResult> DeleteEntityAttribute(int id, int userId)
    {
        try
        {
            await _entityAttributeData.DeleteEntityAttributeAsync(id, userId);

            return Ok("The entity attribute has been deleted");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Delete);
        }
    }
}
