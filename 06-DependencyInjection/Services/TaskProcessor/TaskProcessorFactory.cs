using DependencyInjection.Enums;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class TaskProcessorFactory : ITaskProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TaskProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITaskProcessor CreateProcessor(TaskType taskType) => taskType switch
    {
        TaskType.Urgent => _serviceProvider.GetRequiredService<UrgentTaskProcessor>(),
        TaskType.Scheduled => _serviceProvider.GetRequiredService<ScheduledTaskProcessor>(),
        TaskType.Recurring => _serviceProvider.GetRequiredService<RecurringTaskProcessor>(),
        _ => throw new ArgumentException($"Unknown task type: {taskType}")
    };
}