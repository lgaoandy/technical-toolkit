using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    Task Notify(Operation operation, TaskItem task);
    Task Notify(Operation operation, TaskItem task, TaskItem oldTask);
}