using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    Task SendNotification(TaskItem task);
}