using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface INotificationService
{
    public void Send(TaskOperation operation, TaskItem task, TaskItem? oldTask = null);
}