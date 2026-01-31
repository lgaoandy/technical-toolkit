using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class NotificationService : INotificationService
{
    private static readonly Dictionary<string, List<Notificat>
    private readonly ITenantProvider _tenantProvider;
    private readonly string _currentTenantId;

    public NotificationService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _currentTenantId = _tenantProvider.GetTenantId();
    }

    public Task SendNotification(TaskItem task)
    {
        Console.WriteLine($"Sending notification: Task '{task.Title}' created for '{tenant}");
        return Task.CompletedTask;
    }
}