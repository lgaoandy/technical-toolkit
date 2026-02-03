using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAudioLogger
{
    public Task Log(Operation operation);
}