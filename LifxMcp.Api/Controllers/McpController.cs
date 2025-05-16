using Microsoft.AspNetCore.Mvc;
using LifxMcp.Api.Services;
using LifxMcp.Api.Models;

namespace LifxMcp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class McpController : ControllerBase
{
    private readonly McpService _mcpService;
    private readonly LifxService _lifxService;
    private readonly McpCommandService _commandService;

    public McpController(McpService mcpService, LifxService lifxService, McpCommandService commandService)
    {
        _mcpService = mcpService;
        _lifxService = lifxService;
        _commandService = commandService;
    }

    [HttpPost("command")]
    public async Task<IActionResult> ProcessCommand([FromBody] string command)
    {
        await _mcpService.ProcessCommand(command);
        return Ok();
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteCommand([FromBody] ExecuteCommandRequest request)
    {
        await _mcpService.ExecuteCommand(request.Command, request.Parameters);
        return Ok();
    }

    [HttpGet("schema")]
    public IActionResult GetSchema()
    {
        return Content(_commandService.GetCommandSchema(), "application/json");
    }

    [HttpGet("lights")]
    public async Task<IActionResult> GetLights()
    {
        var lights = await _lifxService.GetLights();
        return Ok(lights);
    }
}

public class ExecuteCommandRequest
{
    public required string Command { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
} 