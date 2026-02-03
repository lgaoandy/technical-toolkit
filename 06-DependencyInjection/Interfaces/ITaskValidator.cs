using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskValidator
{
    ValidationResult ValidateTask(TaskItem task);
}