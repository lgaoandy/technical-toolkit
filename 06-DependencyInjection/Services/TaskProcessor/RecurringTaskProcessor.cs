using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class RecurringTaskProcessor(IAuditLogger auditLogger) : ITaskProcessor
{
    private readonly IAuditLogger _logger = auditLogger;

    public void Process(TaskItem task)
    {
        string description = $"RECURRING: Processing task '{task.Title}' queued for execution";
        Console.WriteLine(description);
        _logger.Log(AuditEvent.TaskProcessed, description);
    }

    public TaskType GetProcessorType() => TaskType.Recurring;
}