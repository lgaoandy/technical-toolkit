namespace DependencyInjection.Interfaces
{
    public interface IOperationTransient
    {
        Guid OperationId { get; }
    }

    public interface IOperationScoped
    {
        Guid OperationId { get; }
    }

    public interface IOperationSingleton
    {
        Guid OperationId { get; }
    }
}