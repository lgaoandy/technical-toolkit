using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public abstract class NotificationServiceBase : INotificationService
{
    private static readonly Dictionary<string, List<Notification>> _notifications = [];
    private readonly string _currentTenantId;
    private static readonly Lock _lock = new();

    public NotificationServiceBase(ITenantProvider tenantProvider)
    {
        _currentTenantId = tenantProvider.GetTenantId();

        // Ensure the current tenant
        lock (_lock)
            _notifications.TryAdd(_currentTenantId, []);
    }

    // Abstract method - subclasses must implement this
    protected abstract string GetNotificationPrefix();

    public void Send(TaskOperation operation, TaskItem task, TaskItem? oldTask = null)
    {
        // Generate message based on operation
        string message = operation switch
        {
            TaskOperation.TaskCreated => $"Task '{task.Title}' created",
            TaskOperation.TaskUpdated => $"Task '{task.Title}' updated:",
            TaskOperation.TaskDeleted => $"Task '{task.Title}' deleted",
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
        Console.WriteLine($"[${GetNotificationPrefix()} {notification.Id} for {_currentTenantId}]: {notification.Message}");
    }
}