using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class TaskProcessorFactory : ITaskProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _tenantId;

    public TaskProcessorFactory(IServiceProvider serviceProvider, ITenantProvider tenantProvider)
    {
        _serviceProvider = serviceProvider;
        _tenantId = tenantProvider.GetTenantId();
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