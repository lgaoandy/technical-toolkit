using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class TaskValidator : ITaskValidator
{
    public ValidationResult Validate(TaskItem task)
    {
        var result = new ValidationResult { IsValid = true };

        if (string.IsNullOrEmpty(task.Title))
        {
            result.IsValid = false;
            result.Errors.Add("Title is empty");
        }

        if (string.IsNullOrEmpty(task.Description))
        {
            result.IsValid = false;
            result.Errors.Add("Description is empty");
        }

        List<string> validTypes = ["urgent", "scheduled", "recurring"];
        if (validTypes.IndexOf(task.Type) < 0)
        {
            result.IsValid = false;
            result.Errors.Add("Type is invalid");
        }

        return result;
    }
}