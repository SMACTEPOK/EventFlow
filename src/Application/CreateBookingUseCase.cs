using EventFlow.Domain;

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

    public CreateBookingUseCase(
        IBookingRepository repository,
        ISeatAvailabilityPort availability)
    {
        _repository = repository;
        _availability = availability;
    }

    public async Task<BookingResult> ExecuteAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = Booking.Create(
            command.EventId, command.CustomerId, command.SeatIds);

        var available = await _availability.AreAvailableAsync(
            booking.EventId, booking.SeatIds, cancellationToken);

        if (!available)
            throw new SeatUnavailableException("One or more seats are unavailable.");

        await _repository.AddAsync(booking, cancellationToken);

        return new BookingResult(
            booking.Id, booking.EventId, booking.CustomerId,
            booking.SeatIds, booking.Status.ToString());
    }
}
