using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class TaskValidator : ITaskValidator
{
    private readonly IAudioLogger _audioLogger;
    private readonly string _currentTenantId;

    public TaskValidator(IAudioLogger audioLogger, ITenantProvider tenantProvider)
    {
        _audioLogger = audioLogger;
        _currentTenantId = tenantProvider.GetTenantId();
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

        // If invalid format, log it
        if (result.IsValid == false)
            _audioLogger.Log(_currentTenantId, AuditEvent.InvalidFormat);

        return result;
    }
}