namespace LifxMcp.Api.Models;

public class McpCommand
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string[] Examples { get; set; }
    public required McpParameter[] Parameters { get; set; }
}

public class McpParameter
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public bool Required { get; set; }
    public string[]? Examples { get; set; }
} 