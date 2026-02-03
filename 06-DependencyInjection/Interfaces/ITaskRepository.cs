using DependencyInjection.Models;

namespace DependencyInjection.Interfaces;

public interface ITaskRepository
{
    Task<int> CreateAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem> UpdateAsync(TaskItem updatedTask);
    Task<TaskItem?> DeleteAsync(int id);
}