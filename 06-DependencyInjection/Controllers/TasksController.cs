using System.Security.Cryptography.X509Certificates;
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

    // Constructor
    public TasksController(
        ITaskValidator validator,
        ITaskRepository repository
    )
    {
        _validator = validator;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
    {
        // Validate task
        ValidationResult result = _validator.Validate(task);

        // If invalid, console each error, then throw error
        if (!result.IsValid)
            return BadRequest(result.Errors);

        // Save to repository
        int id = await _repository.CreateAsync(task);

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
}