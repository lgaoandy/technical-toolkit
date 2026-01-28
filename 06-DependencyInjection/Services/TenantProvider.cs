using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _tenantId;

    // Constructor sets the httpContext on creation
    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetTenantId()
    {
        // Return _tenantId if already set (caching)
        if (_tenantId is not null)
            return _tenantId;

        // If httpContext is null, throw exception
        if (_httpContextAccessor.HttpContext is null)
        {
            Console.WriteLine("GetTenantId() Error - HttpContext not found");
            throw new Exception("Internal Server Error");
        }

        // Read from httpContextAccessor
        try
        {
            _tenantId = _httpContextAccessor.HttpContext.Request.Headers["X-Tenant-Id"];

            if (string.IsNullOrEmpty(_tenantId))
            {
                Console.WriteLine("GetTenantId() Error - No tenant found");
                throw new Exception("Internal Server Error");
            }
            return _tenantId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetTenantId() Error - ${ex}");
            throw new Exception("Internal Server Error");
        }
    }
}