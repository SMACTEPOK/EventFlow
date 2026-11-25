namespace EventFlow.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAtUtc { get; }
    Guid AggregateId { get; }
}

public sealed record BookingCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid AggregateId,
    Guid CustomerId,
    IReadOnlyCollection<Guid> SeatIds) : IIntegrationEvent;

public sealed record PaymentProcessedEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid AggregateId,
    Guid PaymentId,
    decimal Amount,
    string Currency) : IIntegrationEvent;
