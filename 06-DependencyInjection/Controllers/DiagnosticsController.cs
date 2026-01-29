using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("diagnostics")]
public class DiagnosticsController : ControllerBase
{
    // Setup Services
    private readonly ITenantProvider _tenantProvider;

    // Constructor
    public DiagnosticsController(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    [HttpGet]
    public IActionResult GetDiagnostics()
    {
        var tenantId = _tenantProvider.GetTenantId();

        return Ok(new
        {
            tenantId,
            message = "TenantProvider is working!",
            timestamp = DateTime.UtcNow
        });
    }
}