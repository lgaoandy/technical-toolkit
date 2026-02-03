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

    public Task Notify(NotificationType notificationType, TaskItem task, TaskItem? oldTask)
    {
        // Generate message based on operation
        string message = notificationType switch
        {
            NotificationType.TaskCreated => $"Task '{task.Title}' created",
            NotificationType.TaskUpdated => $"Task '{task.Title}' updated:",
            NotificationType.TaskDeleted => $"Task '{task.Title}' deleted",
            _ => throw new InvalidOperationException(),
        };

        // Generate new Guid for notification
        string id = Guid.NewGuid().ToString()[0..8];

        // Identify changes and generate descriptions
        if (oldTask is not null)
        {
            if (oldTask.Type != task.Type)
                message += $"\n\t- Type: '{oldTask.Type}' -> '{task.Type}'";
            if (oldTask.Title != task.Title)
                message += $"\n\t- Title: '{oldTask.Title}' -> '{task.Title}'";
            if (oldTask.Description != task.Description)
                message += $"\n\t- Description: '{oldTask.Description}' -> '{task.Description}'";
        }

        // Generate notification
        Notification notification = new(id, _currentTenantId, message);

        // Store notification to tenant
        _notifications[_currentTenantId].Add(notification);

        // Post notification
        Console.WriteLine($"[Notification {notification.Id} for {_currentTenantId}]: {notification.Message}");

        // Finish
        return Task.CompletedTask;
    }

    public Task Notify(NotificationType notificationType, TaskItem task)
    {
        return Notify(notificationType, task, null);
    }
}