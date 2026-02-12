using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class TaskValidator : ITaskValidator
{
    private readonly string _tenantId;

    public TaskValidator(ITenantProvider tenantProvider)
    {
        _tenantId = tenantProvider.GetTenantId();
    }

    public ValidationResult ValidateTask(TaskItem task)
    {
        var result = new ValidationResult { IsValid = true };
        if (string.IsNullOrEmpty(task.Title))
        {
            result.IsValid = false;
            result.Errors.Add("Task title is required");
        }

        if (string.IsNullOrEmpty(task.Description))
        {
            result.IsValid = false;
            result.Errors.Add("Task description is required");
        }
        return result;
    }
}