using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskProcessor
{
    TaskType ProcessorType { get; }
    void Process(TaskItem task);
}

public interface ITaskProcessorFactory
{
    ITaskProcessor CreateProcessor(string TaskType);
}