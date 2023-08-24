using DERMS.Interfaces;
using DERMS.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DERMS.Silo.Controllers;

[ApiController]
[Route("[controller]")]
public class OperatorController : ControllerBase
{
    private readonly ILogger<OperatorController> _logger;
    private readonly IGrainFactory _grains;

    public OperatorController(ILogger<OperatorController> logger, IGrainFactory grains)
    {
        _logger = logger;
        _grains = grains;
    }

    [HttpGet("{operatorName}/getResources")]
    public async Task<IActionResult> GetAllOperatorResourceInfo(string operatorName)
    {
        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Unable to get Operator");
        }

        var resources = await operatorGrain.GetEnergyResourceInfo();

        return Ok(JsonSerializer.Serialize(resources));
    }

    [HttpPost("{operatorName}/create")]
    public async Task<IActionResult> CreateEnergyResource([FromBody] CreateEnergyResource request, string operatorName)
    {
        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Failed to get Operator");
        }

        var resourceId = await operatorGrain.AddEnergyResource();
        var resourceGrain = await operatorGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to create Energy Resource");
        }

        await resourceGrain.SetOwner(operatorName);
        await resourceGrain.SetName(request.Name);
        await resourceGrain.SetEnergyOutput(request.EnergyOutput);
        
        return Ok();
    }

    [HttpDelete("{operatorName}")]
    public async Task<IActionResult> DeleteEnergyResource([FromBody] DeleteEnergyResource request, string operatorName)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }

        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Failed to get Operator");
        }

        await operatorGrain.RemoveEnergyResource(resourceId);
        
        return Ok();
    }

    [HttpPost]
    [Route("{operatorName}/connect")]
    public async Task<IActionResult> ConnectToGrid([FromBody] ConnectToGrid request, string operatorName)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }

        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Failed to get Operator");
        }

        var resourceGrain = await operatorGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.ConnectToGrid();
        
        return Ok();
    }

    [HttpPost]
    [Route("{operatorName}/disconnect")]
    public async Task<IActionResult> DisconnectFromGrid([FromBody] DisconnectFromGrid request, string operatorName)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }
        
        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Failed to get Operator");
        }

        var resourceGrain = await operatorGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.DisconnectFromGrid();
        
        return Ok();
    }

    [HttpPost]
    [Route("{operatorName}/setOutput")]
    public async Task<IActionResult> SetEnergyResourceOutput([FromBody] SetEnergyResourceOutput request, string operatorName)
    {
        if (!Guid.TryParse(request.Id, out var resourceId))
        {
            return BadRequest("Invalid resource Id");
        }
        
        var operatorGrain = _grains.GetGrain<IOperatorGrain>(operatorName);
        if (operatorGrain == null)
        {
            return BadRequest("Failed to get Operator");
        }

        var resourceGrain = await operatorGrain.GetEnergyResource(resourceId);
        if (resourceGrain == null)
        {
            return BadRequest("Failed to get Energy Resource");
        }

        await resourceGrain.SetEnergyOutput(request.EnergyOutput);
        
        return Ok();
    }

}
