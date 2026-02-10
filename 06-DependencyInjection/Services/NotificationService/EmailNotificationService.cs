using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class EmailNotificationService(ITenantProvider tenantProvider)
    : NotificationServiceBase(tenantProvider), INotificationService
{
    protected override string GetNotificationPrefix()
    {
        return "Email Notification";
    }
}