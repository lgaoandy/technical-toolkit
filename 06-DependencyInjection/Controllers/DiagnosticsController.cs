using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;
using DependencyInjection.Enums;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("diagnostics")]
public class DiagnosticsController : ControllerBase
{
    // Setup Services
    private readonly IAuditLogger _audioLogger;
    private readonly string _currentTenantId;

    // Constructor
    public DiagnosticsController(IAuditLogger audioLogger, ITenantProvider tenantProvider)
    {
        _audioLogger = audioLogger;
        _currentTenantId = tenantProvider.GetTenantId();
    }

    [HttpGet]
    public async Task<IActionResult> GetDiagnostics()
    {
        Dictionary<AuditEvent, int> entryCount = _audioLogger.GetActivityCount(_currentTenantId);

        return Ok(new
        {
            tenantId = _currentTenantId,
            message = "TenantProvider is working!",
            entryCount,
            timestamp = DateTime.UtcNow
        });
    }
}