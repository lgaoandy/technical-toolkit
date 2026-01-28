using DependencyInjection.Interfaces;

namespace DependencyInjection.Services
{
    // Service implementations - all use the same class with different lifetimes
    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
    {
        public Guid OperationId { get; }

        public Operation()
        {
            OperationId = Guid.NewGuid();
        }
    }

    // A service that uses all three types
    public class OperationService
    {
        public IOperationTransient TransientOperation { get; }
        public IOperationScoped ScopedOperation { get; }
        public IOperationSingleton SingletonOperation { get; }

        public OperationService(
            IOperationTransient transientOperation,
            IOperationScoped scopedOperation,
            IOperationSingleton singletonOperation)
        {
            TransientOperation = transientOperation;
            ScopedOperation = scopedOperation;
            SingletonOperation = singletonOperation;
        }
    }

}