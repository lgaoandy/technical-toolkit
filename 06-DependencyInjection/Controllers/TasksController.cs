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
    private readonly ITaskValidator _validator;
    private readonly ITaskRepository _repository;
    private readonly INotificationService _notification;

    // Constructor
    public TasksController(
        ITaskValidator validator,
        ITaskRepository repository,
        INotificationService notification
    )
    {
        _validator = validator;
        _repository = repository;
        _notification = notification;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
    {
        // Validate task
        ValidationResult result = _validator.ValidateNewTask(task);

        // If invalid, console each error, then throw error
        if (!result.IsValid)
            return BadRequest(result.Errors);

        // Save to repository
        int id = await _repository.CreateAsync(task);

        // Send notification
        await _notification.Notify(Operation.CreateTask, task);

        // Return created response with the task
        return CreatedAtAction(nameof(GetTask), new { id }, task);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        // Get task from repository
        TaskItem? task = await _repository.GetByIdAsync(id);

        if (task == null)
            return NotFound(new { message = $"Task with ID {id} not found" });

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
        ValidationResult result = _validator.ValidateUpdatedTask(task);

        // If invalid, throw BadRequest
        if (!result.IsValid)
            return BadRequest(result.Errors);

        // Update task
        await _repository.UpdateAsync(task);

        // Send notification
        await _notification.Notify(Operation.UpdateTask, task);
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
        await _notification.Notify(Operation.DeleteTask, task);
        return Ok();
    }
}