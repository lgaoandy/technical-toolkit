using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class TaskProcessorFactory : ITaskProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TaskProcessorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITaskProcessor CreateProcessor(string TaskType)
    {
        // TODO: Implement
        // Should return:
        // - UrgentTaskProcessor when taskType = urgent
        // - ScheduledTaskProcessor when taskType = scheduled
        // - RecurringTaskProcessor when taskType = recurring
        throw new NotImplementedException();
    }
}