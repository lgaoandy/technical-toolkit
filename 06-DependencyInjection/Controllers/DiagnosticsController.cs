using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("diagnostics")]
public class DiagnosticsController : ControllerBase
{
    private readonly ITenantProvider _tenantProvider;

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
            tenantId = tenantId,
            message = "TenantProvider is working!",
            timestamp = DateTime.UtcNow
        });
    }
}