using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAudioLogger
{
    public Task Log(string tenantId, Operation operation);
    public Task<Dictionary<Operation, int>> ActivityCount(string tenantId);
}