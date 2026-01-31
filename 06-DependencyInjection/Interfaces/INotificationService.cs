using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    Task NotifyCreated(TaskItem task);
}