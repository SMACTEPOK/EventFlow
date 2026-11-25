using EventFlow.Domain;
using EventFlow.Events;

namespace EventFlow.Application;

public sealed record CreateBookingCommand(
    Guid EventId,
    Guid CustomerId,
    IReadOnlyCollection<Guid> SeatIds);

public sealed record BookingResult(
    Guid BookingId,
    Guid EventId,
    Guid CustomerId,
    IReadOnlyCollection<Guid> SeatIds,
    string Status);

public interface ISeatAvailabilityPort
{
    Task<bool> AreAvailableAsync(
        Guid eventId,
        IReadOnlyCollection<Guid> seatIds,
        CancellationToken cancellationToken = default);
}

public sealed class SeatUnavailableException : Exception
{
    public SeatUnavailableException(string message) : base(message) { }
}

public sealed class CreateBookingUseCase
{
    private readonly IBookingRepository _repository;
    private readonly ISeatAvailabilityPort _availability;
    private readonly IEventPublisher _eventPublisher;

    // Compatibility constructor for existing Lab 4 tests and callers.
    public CreateBookingUseCase(
        IBookingRepository repository,
        ISeatAvailabilityPort availability)
        : this(repository, availability, new NullEventPublisher())
    {
    }

    public CreateBookingUseCase(
        IBookingRepository repository,
        ISeatAvailabilityPort availability,
        IEventPublisher eventPublisher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _availability = availability ?? throw new ArgumentNullException(nameof(availability));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public async Task<BookingResult> ExecuteAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = Booking.Create(
            command.EventId,
            command.CustomerId,
            command.SeatIds);

        var available = await _availability.AreAvailableAsync(
            booking.EventId,
            booking.SeatIds,
            cancellationToken);

        if (!available)
            throw new SeatUnavailableException("One or more seats are unavailable.");

        // Persist first. An event is never published for a failed save.
        await _repository.AddAsync(booking, cancellationToken);

        // Publish the integration event after successful persistence.
        var integrationEvent = new BookingCreatedEvent(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            booking.Id,
            booking.CustomerId,
            booking.SeatIds.ToArray());

        await _eventPublisher.PublishAsync(
            integrationEvent,
            cancellationToken);

        return new BookingResult(
            booking.Id,
            booking.EventId,
            booking.CustomerId,
            booking.SeatIds,
            booking.Status.ToString());
    }
}
