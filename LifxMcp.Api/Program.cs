using LifxMcp.Api.Models;
using LifxMcp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure LIFX settings
builder.Services.Configure<LifxSettings>(builder.Configuration.GetSection("Lifx"));

// Add LIFX and MCP services
builder.Services.AddHttpClient<LifxService>();
builder.Services.AddScoped<LifxService>();
builder.Services.AddScoped<McpService>();
builder.Services.AddSingleton<McpCommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
