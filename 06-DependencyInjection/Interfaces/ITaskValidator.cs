using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskValidator
{
    ValidationResult ValidateNewTask(TaskItem task);
    ValidationResult ValidateUpdatedTask(TaskItem task);
}