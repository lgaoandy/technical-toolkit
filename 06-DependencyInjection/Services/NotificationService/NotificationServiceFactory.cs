using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using Microsoft.Extensions.Options;

namespace DependencyInjection.Services;

public class NotificationServiceFactory : INotificationServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TenantNotificationSettings _settings;

    public NotificationServiceFactory(IServiceProvider serviceProvider, IOptions<TenantNotificationSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    public INotificationService GetNotificationService(string tenantId)
    {
        // Get tenant preference from configuration
        if (!_settings.TenantNotificationPreferences.TryGetValue(tenantId, out var preferenceString))
        {
            preferenceString = "Email"; // defaults to email if tenant not configured
        }

        // Convert string to enum
        if (!Enum.TryParse<NotificationType>(preferenceString, out var preference))
        {
            preference = NotificationType.Email; // default
        }

        return _serviceProvider.GetRequiredKeyedService<INotificationService>(preference);
    }
}