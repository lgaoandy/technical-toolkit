using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class PushNotificationService(ITenantProvider tenantProvider)
    : NotificationServiceBase(tenantProvider), INotificationService
{
    protected override string GetNotificationPrefix()
    {
        return "Push Notification";
    }
}