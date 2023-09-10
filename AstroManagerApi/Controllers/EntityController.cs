using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase
{
    private readonly ILogger<EntityController> _logger;
    private readonly IEntityData _entityData;

    public EntityController(ILogger<EntityController> logger, IEntityData entityData)
    {
        _logger = logger;
        _entityData = entityData;
    }

    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetAll => "There has been an issue while fetching the entities",
            Operation.Create => "There has been an issue while creating the entities",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEntities()
    {
        try
        {
            var entities = await _entityData.GetAllEntitiesAsync();
            return Ok(entities);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetAll);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEntity([FromBody] EntityModel entity)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _entityData.CreateEntityAsync(entity);

            return Ok("The entity has been created");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }
}
