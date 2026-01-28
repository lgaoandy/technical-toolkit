using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskValidator
{
    ValidationResult Validate(TaskItem task);
}