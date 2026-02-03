using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    Task Notify(NotificationType type, TaskItem task);
    Task Notify(NotificationType type, TaskItem task, TaskItem oldTask);
}