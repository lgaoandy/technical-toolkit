namespace DependencyInjection.Interfaces;

public interface ITaskProcessorFactory
{
    ITaskProcessor CreateProcessor(string TaskType);
}