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
    private readonly ICacheService _cache;
    private readonly string _tenantId;

    // Constructor
    public DiagnosticsController(IAuditLogger audioLogger, ICacheService cacheService, ITenantProvider tenantProvider)
    {
        _audioLogger = audioLogger;
        _cache = cacheService;
        _tenantId = tenantProvider.GetTenantId();
    }

    [HttpGet]
    public async Task<IActionResult> GetDiagnostics()
    {
        return Ok(new
        {
            tenantId = _tenantId,
            totalAuditOperations = _audioLogger.GetTotalOperations(_tenantId),
            cacheStatistics = new {
                hits = _cache.GetCacheHits(),
                misses = _cache.GetCacheMisses()
            },
            timestamp = DateTime.UtcNow
        });
    }
}