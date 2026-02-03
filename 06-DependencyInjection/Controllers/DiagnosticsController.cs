using Microsoft.AspNetCore.Mvc;
using DependencyInjection.Interfaces;
using DependencyInjection.Enums;

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
    public async Task<IActionResult> GetDiagnostics()
    {
        Dictionary<Operation, int> entryCount = await _logger.ActivityCount(_currentTenantId);

        return Ok(new
        {
            tenantId = _currentTenantId,
            message = "TenantProvider is working!",
            entryCount,
            timestamp = DateTime.UtcNow
        });
    }
}