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
    private readonly ITaskRepository _cachedRepository;
    private readonly IAuditLogger _auditLogger;
    private readonly INotificationServiceFactory _notificationFactory;
    private readonly string _tenantId;

    // Constructor
    public TasksController(
        ITenantProvider tenantProvider,
        ITaskValidator validator,
        IAuditLogger auditLogger,
        ITaskRepository cachedRepository,
        INotificationServiceFactory notificationFactory
    )
    {
        _validator = validator;
        _cachedRepository = cachedRepository;
        _auditLogger = auditLogger;
        _notificationFactory = notificationFactory;
        _tenantId = tenantProvider.GetTenantId();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
    {
        // Validate task
        ValidationResult result = _validator.ValidateTask(task);

        // If invalid, console each error, then throw error
        if (!result.IsValid)
        {
            _auditLogger.Log(AuditEvent.InvalidFormat);
            return BadRequest(result.Errors);
        }

        // Save to repository
        await _cachedRepository.CreateAsync(task);

        // Log task created
        _auditLogger.Log(AuditEvent.TaskCreated);

        // Get correct notification service for this tenant
        var notificationService = _notificationFactory.GetNotificationService(_tenantId);
        notificationService.Send(TaskOperation.TaskCreated, task);

        // Return created response with the task
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        TaskItem? task = await _cachedRepository.GetByIdAsync(id);

        if (task == null) // If task is null, return NotFound
        {
            _auditLogger.Log(AuditEvent.NotFound);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }
        _auditLogger.Log(AuditEvent.TaskRetrieved);
        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        List<TaskItem> tasks = (List<TaskItem>)await _cachedRepository.GetAllAsync();
        _auditLogger.Log(AuditEvent.TaskGroupRetrieved);
        return Ok(tasks);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
    {
        ValidationResult result = _validator.ValidateTask(task);

        if (!result.IsValid)
        {
            _auditLogger.Log(AuditEvent.InvalidFormat);
            return BadRequest(result.Errors);
        }

        // Update task
        TaskItem outdatedTask = await _cachedRepository.UpdateAsync(task);
        _auditLogger.Log(AuditEvent.TaskUpdated);

        // Send notification
        var notificationService = _notificationFactory.GetNotificationService(_tenantId);
        notificationService.Send(TaskOperation.TaskUpdated, task, outdatedTask);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        TaskItem? task = await _cachedRepository.DeleteAsync(id);

        if (task == null)
        {
            _auditLogger.Log(AuditEvent.NotFound);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        // Send notification
        var notificationService = _notificationFactory.GetNotificationService(_tenantId);
        notificationService.Send(TaskOperation.TaskDeleted, task);
        return Ok();
    }
}