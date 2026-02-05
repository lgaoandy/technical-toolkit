using System.Security.Cryptography.X509Certificates;
using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("tasks")]
public class TasksController : ControllerBase
{
    // Setup services
    private readonly ICacheService _cacheService;
    private readonly ITaskValidator _validator;
    private readonly ITaskRepository _repository;
    private readonly INotificationService _notification;
    private string _currentTenantId;

    // Constructor
    public TasksController(
        ITenantProvider tenantProvider,
        ITaskValidator validator,
        ITaskRepository repository,
        INotificationService notification,
        ICacheService cacheService
    )
    {
        _validator = validator;
        _repository = repository;
        _notification = notification;
        _cacheService = cacheService;
        _currentTenantId = tenantProvider.GetTenantId();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
    {
        // Validate task
        ValidationResult result = _validator.ValidateTask(task);

        // If invalid, console each error, then throw error
        if (!result.IsValid)
            return BadRequest(result.Errors);

        // Save to repository
        int id = await _repository.CreateAsync(task);

        // Send notification
        _notification.Send(NotificationType.TaskCreated, task);

        // Return created response with the task
        return CreatedAtAction(nameof(GetTask), new { id }, task);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {

        // Try getting from cache
        string key = _cacheService.GenKey(_currentTenantId, id);

        // If task not in cache, get task from repository
        if (!_cacheService.TryGet(key, out object? task))
        {
            task = await _repository.GetByIdAsync(id);
        }

        // If task is null, return 
        if (task == null)
        {
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        // Return task
        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        List<TaskItem> tasks = (List<TaskItem>)await _repository.GetAllAsync();
        return Ok(tasks);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
    {
        // Validate to-be-updated task
        ValidationResult result = _validator.ValidateTask(task);

        // If invalid, throw BadRequest
        if (!result.IsValid)
            return BadRequest(result.Errors);

        // Update task
        TaskItem oldTask = await _repository.UpdateAsync(task);

        // Send notification
        _notification.Send(NotificationType.TaskUpdated, task, oldTask);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        // Delete task
        TaskItem? task = await _repository.DeleteAsync(id);

        // If return task is null, return NotFound
        if (task == null)
            return NotFound(new { message = $"Task with ID {id} not found" });

        // Send notification
        _notification.Send(NotificationType.TaskDeleted, task);
        return Ok();
    }
}