using DependencyInjection.Interfaces;

namespace DependencyInjection.Services;

public class WelcomeService : IWelcomeService
{
    private DateTime _serviceCreated;
    private Guid _serviceId;

    public WelcomeService()
    {
        _serviceCreated = DateTime.Now;
        _serviceId = Guid.NewGuid();
    }

    public string GetWelcomeMessage()
    {
        return $"Welcome to Contoso! The current time {_serviceCreated}. This service instance has an ID of {_serviceId}";
    }
}