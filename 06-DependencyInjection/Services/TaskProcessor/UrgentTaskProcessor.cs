using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class UrgentTaskProcess(IAuditLogger audioLogger) : ITaskProcessor
{
    private readonly IAuditLogger _logger = audioLogger;

    public void Process(TaskItem task)
    {
        string description = $"URGENT: Processing task '{task.Title}' immediately with HIGH priority";
        Console.WriteLine(description);
        _logger.Log(AuditEvent.TaskProcessed, description);
    }

    public TaskType GetProcessorType() => TaskType.Urgent;
}