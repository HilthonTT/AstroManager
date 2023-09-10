using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttributeController : ControllerBase
{
    private readonly ILogger<AttributeController> _logger;
    private readonly IAttributeData _attributeData;

    public AttributeController(ILogger<AttributeController> logger, IAttributeData attributeData)
    {
        _logger = logger;
        _attributeData = attributeData;
    }
    private ObjectResult LogError(string exceptionMessage, Operation operation)
    {
        string errorMessage = operation switch
        {
            Operation.GetAll => "There has been an issue while fetching the attributes",
            Operation.Create => "There has been an issue while creating the attribute",
            _ => "There has been an issue",
        };

        _logger.LogError("{message}: {e}", errorMessage, exceptionMessage);
        return StatusCode(500, errorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAttributes()
    {
        try
        {
            var attributes = await _attributeData.GetAllAttributesAsync();
            return Ok(attributes);
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.GetAll);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAttribute([FromBody] AttributeModel attribute)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            await _attributeData.CreateAttributeAsync(attribute);

            return Ok("The attribute has been created.");
        }
        catch (Exception ex)
        {
            return LogError(ex.Message, Operation.Create);
        }
    }
}
