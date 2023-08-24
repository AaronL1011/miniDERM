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

    [HttpGet("getResources/{username}")]
    public async Task<IActionResult> GetAllUserResourceInfo(string username)
    {
        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Unable to get User");
        }

        var resources = await userGrain.GetEnergyResourceInfo();

        return Ok(JsonSerializer.Serialize(resources));
    }

    [HttpPost("create/{username}")]
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

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteEnergyResource([FromBody] DeleteEnergyResource request, string username)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }

        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Failed to get User");
        }

        await userGrain.RemoveEnergyResource(resourceId);
        
        return Ok();
    }

    [HttpPost]
    [Route("connect/{username}")]
    public async Task<IActionResult> ConnectToGrid([FromBody] ConnectToGrid request, string username)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }

        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Failed to get User");
        }

        var resourceGrain = await userGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.ConnectToGrid();
        
        return Ok();
    }

    [HttpPost]
    [Route("disconnect/{username}")]
    public async Task<IActionResult> DisconnectFromGrid([FromBody] DisconnectFromGrid request, string username)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }
        
        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Failed to get User");
        }

        var resourceGrain = await userGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.DisconnectFromGrid();
        
        return Ok();
    }

    [HttpPost]
    [Route("setOutput/{username}")]
    public async Task<IActionResult> SetEnergyResourceOutput([FromBody] SetEnergyResourceOutput request, string username)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }
        
        var userGrain = _grains.GetGrain<IUserGrain>(username);
        if (userGrain == null)
        {
            return BadRequest("Failed to get User");
        }

        var resourceGrain = await userGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.SetEnergyOutput(request.EnergyOutput);
        
        return Ok();
    }

}
