using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
            Operation.Create => $"Task '{task.Title}' created for '{_currentTenantId}'",
            Operation.Update => $"Task '{task.Title}' has been updated for '{_currentTenantId}'",
            Operation.Delete => $"Task '{task.Title}' has been deleted for '{_currentTenantId}'",
            _ => throw new InvalidOperationException(),
        };

        // Generate notification
        Notification notification = new (operation, message);

        // Store notification to tenant
        _notifications[_currentTenantId].Add(notification);

        // Post notification
        Console.WriteLine(notification.Message);
        return Task.CompletedTask;
    }
}