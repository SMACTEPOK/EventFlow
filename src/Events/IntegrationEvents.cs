namespace EventFlow.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
    Guid AggregateId { get; }
}

public sealed record BookingCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AggregateId,
    Guid CustomerId,
    IReadOnlyList<Guid> SeatIds
) : IIntegrationEvent;

public sealed record PaymentProcessedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AggregateId,
    decimal Amount,
    string Currency,
    bool Successful
) : IIntegrationEvent;
