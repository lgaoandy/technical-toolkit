namespace DependencyInjection.Interfaces;

public interface INotificationServiceFactory
{
    INotificationService GetNotificationService(string tenantId);
}