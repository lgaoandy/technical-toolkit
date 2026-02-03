using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;
using DependencyInjection.Enums;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("diagnostics")]
public class DiagnosticsController : ControllerBase
{
    // Setup Services
    private readonly IAudioLogger _audioLogger;
    private string _currentTenantId;

    // Constructor
    public DiagnosticsController(IAudioLogger audioLogger, ITenantProvider tenantProvider)
    {
        _audioLogger = audioLogger;
        _currentTenantId = tenantProvider.GetTenantId();
    }

    [HttpGet]
    public async Task<IActionResult> GetDiagnostics()
    {
        Dictionary<AuditEvent, int> entryCount = await _audioLogger.GetActivityCount(_currentTenantId);

        return Ok(new
        {
            tenantId = _currentTenantId,
            message = "TenantProvider is working!",
            entryCount,
            timestamp = DateTime.UtcNow
        });
    }
}