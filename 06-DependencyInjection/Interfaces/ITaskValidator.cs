using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskValidator
{
    ValidationResult validate(TaskItem task);
}