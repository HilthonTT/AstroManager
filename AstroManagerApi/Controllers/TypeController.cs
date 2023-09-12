﻿using AstroManagerApi.Common;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroManagerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TypeController : CustomController
{
    private readonly ITypeData _typeData;

    public TypeController(
        ILogger<CustomController> logger,
        ITypeData typeData) : base(logger)
    {
        _typeData = typeData;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTypesAsync()
    {
        try
        {
            var types = await _typeData.GetAllTypesAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTypeAsync([FromBody] TypeModel type)
    {
        try
        {
            if (ModelState.IsValid is false)
            {
                return BadRequest(ModelState);
            }

            var createdType = await _typeData.CreateTypeAsync(type);
            return Ok(createdType);
        }
        catch (Exception ex)
        {
            return ServerErrorCode(ex);
        }
    }
}
