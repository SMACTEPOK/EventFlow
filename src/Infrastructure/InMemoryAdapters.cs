using System.Collections.Concurrent;
using EventFlow.Application;
using EventFlow.Domain;

namespace EventFlow.Infrastructure;

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!_bookings.TryAdd(booking.Id, booking))
            throw new InvalidOperationException("Booking already exists.");
        return Task.CompletedTask;
    }
}

public sealed class InMemorySeatAvailabilityAdapter : ISeatAvailabilityPort
{
    public Task<bool> AreAvailableAsync(
        Guid eventId,
        IReadOnlyCollection<Guid> seatIds,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(true);
    }
}
