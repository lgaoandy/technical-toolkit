using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class NotificationService : INotificationService
{
    private static readonly Dictionary<string, List<Notification>> _notifications = new();
    private readonly ITenantProvider _tenantProvider;
    private readonly string _currentTenantId;
    private static readonly object _lock = new object();

    public NotificationService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _currentTenantId = _tenantProvider.GetTenantId();

        // Ensure the current tenant
        lock (_lock)
            _notifications.TryAdd(_currentTenantId, []);
    }

    public Task SendNotification(TaskItem task)
    {
        // Generate new notification message
        Notification notification = new ($"Sending notification: Task '{task.Title}' created for '{_currentTenantId}");

        // Store notification to tenant
        _notifications[_currentTenantId].Add(notification);

        // Post notification
        Console.WriteLine(notification.Message);
        return Task.CompletedTask;
    }
}