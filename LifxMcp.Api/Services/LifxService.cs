using System.Text.Json;
using LifxMcp.Api.Models;
using Microsoft.Extensions.Options;

namespace LifxMcp.Api.Services;

public class LifxService
{
    private readonly HttpClient _httpClient;
    private readonly LifxSettings _settings;

    public LifxService(HttpClient httpClient, IOptions<LifxSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiToken}");
    }

    public async Task SetLightState(string selector = "all", double? brightness = null, string? power = null)
    {
        var payload = new Dictionary<string, object>();
        
        if (brightness.HasValue)
        {
            payload["brightness"] = brightness.Value;
        }
        
        if (!string.IsNullOrEmpty(power))
        {
            payload["power"] = power;
        }

        var response = await _httpClient.PutAsJsonAsync($"lights/{selector}/state", payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<dynamic>> GetLights(string selector = "all")
    {
        var response = await _httpClient.GetAsync($"lights/{selector}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<dynamic>>(content)!;
    }
} 