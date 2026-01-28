using DependencyInjection.Interfaces;
using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

// Add HttpContextAccessor - required for tenantProvider
builder.Services.AddHttpContextAccessor();

// Register providers
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

var app = builder.Build();

app.MapGet("/", () => "Hello!");

app.MapPost("/tasks", (ITenantProvider tenantProvider) =>
{
    string tenantId = tenantProvider.GetTenantId();
    return $"Successful tenant retrieval - {tenantId}";
});

app.Run();
