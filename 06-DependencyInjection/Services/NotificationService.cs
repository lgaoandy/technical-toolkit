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

    public Task Notify(Operation operation, TaskItem task, TaskItem? oldTask)
    {
        // Generate message based on operation
        string message = operation switch
        {
            Operation.CreateTask => $"Task '{task.Title}' created",
            Operation.UpdateTask=> $"Task '{task.Title}' updated",
            Operation.DeleteTask => $"Task '{task.Title}' deleted",
            _ => throw new InvalidOperationException(),
        };

        // Generate new Guid for notification
        string id = Guid.NewGuid().ToString()[0..8];

        // Identify changes and generate descriptions
        List<string> changes = [];
        string descriptions = string.Empty;
        if (oldTask is not null)
        { 
            if (oldTask.Type != task.Type)
                changes.Add($"Type '{oldTask.Type}' change to '{task.Type}'");
            if (oldTask.Title != task.Title)
                changes.Add($"Title '{oldTask.Title}' change to '{task.Title}'");
            if (oldTask.Description != task.Description)
                changes.Add($"Description '{oldTask.Description}' change to '{task.Description}'");
            descriptions = string.Join(". ", changes);
        }

        // Generate notification
        Notification notification = new (id, _currentTenantId, message, descriptions);

        // Store notification to tenant
        _notifications[_currentTenantId].Add(notification);

        // Post notification
        if (descriptions.Length > 0)
            Console.WriteLine($"[Notification {notification.Id} for {_currentTenantId}]: {notification.Message} - {notification.Description}");
        else
            Console.WriteLine($"[Notification {notification.Id} for {_currentTenantId}]: {notification.Message}");

        // Finish
        return Task.CompletedTask;
    }

    public Task Notify(Operation operation, TaskItem task)
    {
        return Notify(operation, task, null);
    }
}