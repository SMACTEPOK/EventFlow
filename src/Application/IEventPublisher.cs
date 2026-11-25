using EventFlow.Events;

namespace EventFlow.Application;

public interface IEventPublisher
{
    Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);
}

public sealed class NullEventPublisher : IEventPublisher
{
    public Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);
        return Task.CompletedTask;
    }
}
