using DERMS.Interfaces;
using DERMS.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DERMS.Silo.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IGrainFactory _grains;

    public UserController(ILogger<UserController> logger, IGrainFactory grains)
    {
        _logger = logger;
        _grains = grains;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetAllUserResourceInfo(string username)
    {
        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Unable to get User");
        }

        var resourceInfos = await userGrain.GetEnergyResourceNames();

        return Ok(JsonSerializer.Serialize(resourceInfos));
    }

    [HttpPost("{username}")]
    public async Task<IActionResult> CreateEnergyResource([FromBody] CreateEnergyResource request, string username)
    {
        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Failed to get User");
        }

        var resourceId = await userGrain.AddEnergyResource();
        var resourceGrain = await userGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to create Energy Resource");
        }

        await resourceGrain.SetOwner(username);
        await resourceGrain.SetName(request.Name);
        await resourceGrain.SetEnergyOutput(request.EnergyOutput);
        
        return Ok();
    }


}
