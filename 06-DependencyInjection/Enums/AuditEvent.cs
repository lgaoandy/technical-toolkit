namespace DependencyInjection.Enums;

public enum AuditEvent
{
    TaskCreated,
    TaskUpdated,
    TaskDeleted,
    TaskRetrieved,
    TaskGroupRetrieved,
    TaskProcessed,
    TaskScheduled,
    NotFound,
    InvalidFormat,
}
