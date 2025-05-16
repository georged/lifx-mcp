using LifxMcp.Api.Models;

namespace LifxMcp.Api.Services;

public class McpCommandService
{
    private readonly List<McpCommand> _commands;

    public McpCommandService()
    {
        _commands = new List<McpCommand>
        {
            new McpCommand
            {
                Name = "turn_on_lights",
                Description = "Turn on all LIFX lights",
                Examples = new[] { "lights on", "turn on the lights", "switch on all lights" },
                Parameters = Array.Empty<McpParameter>()
            },
            new McpCommand
            {
                Name = "turn_off_lights",
                Description = "Turn off all LIFX lights",
                Examples = new[] { "lights off", "turn off the lights", "switch off all lights" },
                Parameters = Array.Empty<McpParameter>()
            },
            new McpCommand
            {
                Name = "set_brightness",
                Description = "Set the brightness of all LIFX lights",
                Examples = new[] { "dim 50%", "set brightness to 75%", "lights at 20 percent" },
                Parameters = new[]
                {
                    new McpParameter
                    {
                        Name = "brightness",
                        Description = "Brightness level as a percentage (0-100)",
                        Type = "number",
                        Required = true,
                        Examples = new[] { "50", "75", "20" }
                    }
                }
            }
        };
    }

    public IEnumerable<McpCommand> GetAllCommands() => _commands;

    public McpCommand? GetCommand(string name) => 
        _commands.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public string GetCommandSchema()
    {
        var schema = new
        {
            version = "1.0",
            commands = _commands.Select(c => new
            {
                name = c.Name,
                description = c.Description,
                examples = c.Examples,
                parameters = c.Parameters.Select(p => new
                {
                    name = p.Name,
                    description = p.Description,
                    type = p.Type,
                    required = p.Required,
                    examples = p.Examples
                })
            })
        };

        return System.Text.Json.JsonSerializer.Serialize(schema, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
} 