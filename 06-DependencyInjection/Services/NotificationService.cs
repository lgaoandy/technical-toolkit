using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class NotificationService : INotificationService
{
    private static readonly Dictionary<string, List<Notification>> _notifications = new();
    private readonly ITenantProvider _tenantProvider;
    private readonly string _currentTenantId;
    private static readonly object _lock = new();

    public NotificationService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _currentTenantId = _tenantProvider.GetTenantId();

        // Ensure the current tenant
        lock (_lock)
            _notifications.TryAdd(_currentTenantId, []);
    }

    public Task Notify(Operation operation, TaskItem task)
    {
        // Generate message based on operation
        string message = operation switch
        {
            Operation.CreateTask => $"Task '{task.Title}' created for '{_currentTenantId}'",
            Operation.UpdateTask=> $"Task '{task.Title}' updated for '{_currentTenantId}'",
            Operation.DeleteTask => $"Task '{task.Title}' deleted for '{_currentTenantId}'",
            _ => throw new InvalidOperationException(),
        };

        // Generate notification
        Notification notification = new (operation, message);

        // Store notification to tenant
        _notifications[_currentTenantId].Add(notification);

        // Post notification
        Console.WriteLine($"[Notification {notification.Id}]: {notification.Message}");
        return Task.CompletedTask;
    }
}