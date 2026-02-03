using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    public void Send(NotificationType type, TaskItem task, TaskItem? oldTask = null);
}