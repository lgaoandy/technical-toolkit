using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class SmsNotificationService(ITenantProvider tenantProvider)
    : NotificationServiceBase(tenantProvider), INotificationService
{
    protected override string GetNotificationPrefix()
    {
        return "SMS Notification";
    }
}