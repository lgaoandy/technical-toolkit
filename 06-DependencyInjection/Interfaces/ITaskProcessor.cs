using DependencyInjection.Enums;
using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskProcessor
{
    void Process(TaskItem task);
    TaskType GetProcessorType();
}

