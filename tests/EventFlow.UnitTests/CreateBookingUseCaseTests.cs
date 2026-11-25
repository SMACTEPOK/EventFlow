using EventFlow.Application;
using EventFlow.Domain;
using Xunit;

namespace EventFlow.UnitTests;

public sealed class CreateBookingUseCaseTests
{
    private static readonly Guid EventId =
        Guid.Parse("7a1b2c3d-4e5f-6789-abcd-ef0123456789");
    private static readonly Guid CustomerId =
        Guid.Parse("2b3c4d5e-6f70-8192-a3b4-c5d6e7f80910");
    private static readonly Guid SeatId =
        Guid.Parse("11111111-1111-4111-8111-111111111111");

    private sealed class BookingRepositoryStub : IBookingRepository
    {
        public int AddCalls { get; private set; }
        public Booking? SavedBooking { get; private set; }
        public Exception? Error { get; set; }

        public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (Error is not null) throw Error;
            AddCalls++;
            SavedBooking = booking;
            return Task.CompletedTask;
        }
    }

    private sealed class AvailabilityStub : ISeatAvailabilityPort
    {
        public bool Available { get; set; } = true;

        public Task<bool> AreAvailableAsync(
            Guid eventId, IReadOnlyCollection<Guid> seatIds,
            CancellationToken cancellationToken = default)
            => Task.FromResult(Available);
    }

    [Fact]
    public async Task ExecuteAsync_ValidBooking_SavesThroughRepositoryPort()
    {
        var repository = new BookingRepositoryStub();
        var availability = new AvailabilityStub();
        var useCase = new CreateBookingUseCase(repository, availability);

        var result = await useCase.ExecuteAsync(
            new CreateBookingCommand(EventId, CustomerId, new[] { SeatId }));

        Assert.NotEqual(Guid.Empty, result.BookingId);
        Assert.Equal("Pending", result.Status);
        Assert.Equal(1, repository.AddCalls);
        Assert.NotNull(repository.SavedBooking);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidBooking_DoesNotSave()
    {
        var repository = new BookingRepositoryStub();
        var availability = new AvailabilityStub();
        var useCase = new CreateBookingUseCase(repository, availability);

        await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new CreateBookingCommand(EventId, CustomerId, Array.Empty<Guid>())));

        Assert.Equal(0, repository.AddCalls);
    }

    [Fact]
    public async Task ExecuteAsync_RepositoryFailure_PropagatesToCaller()
    {
        var repository = new BookingRepositoryStub
        {
            Error = new InvalidOperationException("Simulated repository failure.")
        };
        var availability = new AvailabilityStub();
        var useCase = new CreateBookingUseCase(repository, availability);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            useCase.ExecuteAsync(
                new CreateBookingCommand(EventId, CustomerId, new[] { SeatId })));

        Assert.Equal("Simulated repository failure.", exception.Message);
        Assert.Equal(0, repository.AddCalls);
    }
}
