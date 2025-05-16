namespace LifxMcp.Api.Models;

public class LifxSettings
{
    public required string ApiToken { get; set; }
    public string BaseUrl { get; set; } = "https://api.lifx.com/v1/";
} 