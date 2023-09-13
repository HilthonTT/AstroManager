using AstroManagerApi.Common;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CredentialTemplateController : CustomController<CredentialTemplateController>
{
    private readonly ICredentialTemplateData _templateData;

    public CredentialTemplateController(
        ILogger<CredentialTemplateController> logger,
        ICredentialTemplateData templateData) : base(logger)
    {
        _templateData = templateData;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTemplatesAsync()
    {
        try
        {
            LogRequestSource();

            var templates = await _templateData.GetAllTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateAsync(string id)
    {
        try
        {
            LogRequestSource();

            var templates = await _templateData.GetTemplateAsync(id);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplateAsync([FromBody] CredentialTemplateModel template)
    {
        try
        {
            LogRequestSource();
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var createdTemplate = await _templateData.CreateTemplateAsync(template);
            return Ok(createdTemplate);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
