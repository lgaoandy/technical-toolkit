using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("diagnostics")]
public class DiagnosticsController : ControllerBase
{
    // Setup Services
    private readonly IAudioLogger _logger;
    private string _currentTenantId;

    // Constructor
    public DiagnosticsController(IAudioLogger audioLogger, ITenantProvider tenantProvider)
    {
        _logger = audioLogger;
        _currentTenantId = tenantProvider.GetTenantId();
    }

    [HttpGet]
    public IActionResult GetDiagnostics()
    {
        return Ok(new
        {
            tenantId = _currentTenantId,
            message = "TenantProvider is working!",
            timestamp = DateTime.UtcNow
        });
    }
}