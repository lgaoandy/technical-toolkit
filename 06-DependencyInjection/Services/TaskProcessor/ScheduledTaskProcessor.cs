using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class ScheduledTaskProcessor(IAuditLogger auditLogger, ITenantProvider tenantProvider) : ITaskProcessor
{
    private readonly IAuditLogger _logger = auditLogger;
    private readonly string _tenantId = tenantProvider.GetTenantId();

    public void Process(TaskItem task)
    {
        string description = $"SCHEDULE: Processing task '{task.Title}' queued for execution";
        Console.WriteLine(description);
        _logger.Log(AuditEvent.TaskScheduled, description, _tenantId);
    }

    public TaskType GetProcessorType() => TaskType.Scheduled;
}