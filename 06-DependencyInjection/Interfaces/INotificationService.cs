using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    public void Send(NotificationOperation operation, TaskItem task, TaskItem? oldTask = null);
}