using LifxMcp.Api.Models;

namespace LifxMcp.Api.Services;

public class McpService
{
    private readonly LifxService _lifxService;
    private readonly McpCommandService _commandService;

    public McpService(LifxService lifxService, McpCommandService commandService)
    {
        _lifxService = lifxService;
        _commandService = commandService;
    }

    public async Task ProcessCommand(string command)
    {
        command = command.ToLower().Trim();

        if (command.Contains("on"))
        {
            await ExecuteCommand("turn_on_lights", new Dictionary<string, object>());
        }
        else if (command.Contains("off"))
        {
            await ExecuteCommand("turn_off_lights", new Dictionary<string, object>());
        }
        else if (command.Contains("dim") || command.Contains("%") || command.Contains("percent"))
        {
            var brightness = ExtractBrightnessValue(command);
            if (brightness.HasValue)
            {
                await ExecuteCommand("set_brightness", new Dictionary<string, object>
                {
                    ["brightness"] = brightness.Value * 100
                });
            }
        }
    }

    public async Task ExecuteCommand(string commandName, Dictionary<string, object> parameters)
    {
        var command = _commandService.GetCommand(commandName);
        if (command == null)
        {
            throw new ArgumentException($"Unknown command: {commandName}");
        }

        switch (commandName)
        {
            case "turn_on_lights":
                await _lifxService.SetLightState(power: "on");
                break;
            case "turn_off_lights":
                await _lifxService.SetLightState(power: "off");
                break;
            case "set_brightness":
                if (parameters.TryGetValue("brightness", out var brightnessObj) && 
                    brightnessObj is double brightness)
                {
                    await _lifxService.SetLightState(brightness: brightness / 100.0);
                }
                break;
            default:
                throw new ArgumentException($"Unhandled command: {commandName}");
        }
    }

    private double? ExtractBrightnessValue(string command)
    {
        // Handle "dim the lights half way" or "50%" or "light 20 percent"
        if (command.Contains("half"))
        {
            return 0.5;
        }

        var words = command.Split(' ');
        foreach (var word in words)
        {
            if (word.EndsWith("%"))
            {
                if (double.TryParse(word.TrimEnd('%'), out double percentage))
                {
                    return percentage / 100.0;
                }
            }
            else if (double.TryParse(word, out double number))
            {
                if (command.Contains("percent"))
                {
                    return number / 100.0;
                }
            }
        }

        return null;
    }
} 