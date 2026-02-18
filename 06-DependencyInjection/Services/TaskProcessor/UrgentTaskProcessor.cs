using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class UrgentTaskProcess(IAuditLogger audioLogger, ITenantProvider tenantProvider) : ITaskProcessor
{
    public TaskType ProcessorType => TaskType.Urgent;
    private readonly IAuditLogger _logger = audioLogger;
    private readonly string _tenantId = tenantProvider.GetTenantId();

    public void Process(TaskItem task)
    {
        string description = $"URGENT: Processing task '{task.Title}' immediately with HIGH priority";
        Console.WriteLine(description);
        _logger.Log(AuditEvent.TaskProcessed, description, _tenantId);
    }

}