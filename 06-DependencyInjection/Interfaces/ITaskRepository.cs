using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskRepository
{
    Task CreateAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllASync();
    Task UpdateAsync(int id, TaskItem task);
    Task DeleteAsync(int id);
}