namespace EventFlow.Domain;

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public sealed class Booking
{
    public Guid Id { get; }
    public Guid EventId { get; }
    public Guid CustomerId { get; }
    public IReadOnlyCollection<Guid> SeatIds { get; }
    public BookingStatus Status { get; }
    public DateTime CreatedAtUtc { get; }

    private Booking(Guid id, Guid eventId, Guid customerId,
        IReadOnlyCollection<Guid> seatIds)
    {
        Id = id;
        EventId = eventId;
        CustomerId = customerId;
        SeatIds = seatIds;
        Status = BookingStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Booking Create(Guid eventId, Guid customerId,
        IReadOnlyCollection<Guid> seatIds)
    {
        if (eventId == Guid.Empty)
            throw new DomainException("EventId is required.");
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId is required.");
        if (seatIds is null || seatIds.Count == 0 || seatIds.Count > 10)
            throw new DomainException("Booking must contain between 1 and 10 seats.");
        if (seatIds.Any(x => x == Guid.Empty))
            throw new DomainException("Seat identifiers cannot be empty.");
        if (seatIds.Distinct().Count() != seatIds.Count)
            throw new DomainException("Duplicate seats are not allowed.");

        return new Booking(Guid.NewGuid(), eventId, customerId, seatIds.ToArray());
    }
}

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
}
