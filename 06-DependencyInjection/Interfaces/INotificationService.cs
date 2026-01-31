using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    Task NotifyCreated(TaskItem task);
    Task NotifyUpdated(TaskItem task);
    Task NotifyDeleted(TaskItem task);
}