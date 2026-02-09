using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class UrgentTaskProcess : ITaskProcessor
{
    private readonly IAuditLogger _logger;
    private readonly ITenantProvider _tenantProvider;

    public UrgentTaskProcess(IAuditLogger audioLogger, ITenantProvider tenantProvider)
    {
        _logger = audioLogger;
        _tenantProvider = tenantProvider;
    }

    public void Process(TaskItem task)
    {
        string tenantId = _tenantProvider.GetTenantId();

        Console.WriteLine($"URGENT: Processing task '{task.Title}' immediately with HIGH priority");
        _logger.Log(tenantId, Enums.AuditEvent.TaskProcessed);
    }

    public TaskType GetProcessorType() => TaskType.Urgent;
}