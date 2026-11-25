using EventFlow.Application;
using EventFlow.Domain;
using EventFlow.Events;
using EventFlow.Messaging;
using Xunit;

namespace EventFlow.UnitTests;

public sealed class EventDrivenArchitectureTests
{
    [Fact]
    public async Task CreateBookingUseCase_PublishesEventAfterSuccessfulSave()
    {
        var repository = new TestBookingRepository();
        var availability = new TestAvailabilityPort(true);
        var publisher = new RecordingEventPublisher();

        var useCase = new CreateBookingUseCase(
            repository,
            availability,
            publisher);

        var command = new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new[] { Guid.NewGuid() });

        var result = await useCase.ExecuteAsync(command);

        Assert.NotNull(repository.SavedBooking);
        Assert.NotNull(publisher.PublishedEvent);
        Assert.IsType<BookingCreatedEvent>(publisher.PublishedEvent);

        var published = (BookingCreatedEvent)publisher.PublishedEvent!;
        Assert.Equal(result.BookingId, published.AggregateId);
        Assert.Equal(result.CustomerId, published.CustomerId);
        Assert.NotEqual(Guid.Empty, published.EventId);
        Assert.NotEqual(default, published.OccurredAtUtc);
    }

    [Fact]
    public async Task CreateBookingUseCase_DoesNotPublishWhenSaveFails()
    {
        var repository = new FailingBookingRepository();
        var availability = new TestAvailabilityPort(true);
        var publisher = new RecordingEventPublisher();

        var useCase = new CreateBookingUseCase(
            repository,
            availability,
            publisher);

        var command = new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new[] { Guid.NewGuid() });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(command));

        Assert.Null(publisher.PublishedEvent);
    }

    [Fact]
    public async Task EventBus_ProcessesDuplicateOnlyOncePerConsumer()
    {
        var bus = new InMemoryEventBus();
        var store = new InMemoryProcessedEventStore();

        var first = new BookingNotificationConsumer(store);
        var second = new BookingAnalyticsConsumer(store);

        bus.Subscribe(first);
        bus.Subscribe(second);

        var integrationEvent = CreateEvent();

        await bus.PublishAsync(integrationEvent);
        await bus.DrainAsync();

        await bus.PublishAsync(integrationEvent);
        await bus.DrainAsync();

        Assert.Equal(1, first.ProcessedCount);
        Assert.Equal(1, second.ProcessedCount);
    }

    [Fact]
    public async Task EventBus_FailureOfOneConsumerDoesNotBlockAnother()
    {
        var bus = new InMemoryEventBus();
        var successfulConsumer = new BookingNotificationConsumer(
            new InMemoryProcessedEventStore());

        bus.Subscribe(new FailingConsumer());
        bus.Subscribe(successfulConsumer);

        await bus.PublishAsync(CreateEvent());
        await bus.DrainAsync();

        Assert.Equal(1, successfulConsumer.ProcessedCount);
    }

    [Fact]
    public async Task IdempotentConsumer_DoesNotMarkEventWhenProcessingFails()
    {
        var store = new InMemoryProcessedEventStore();
        var consumer = new RetryableConsumer(store);
        var integrationEvent = CreateEvent();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => consumer.HandleAsync(integrationEvent));

        Assert.False(store.IsProcessed(
            nameof(RetryableConsumer),
            integrationEvent.EventId));

        await consumer.HandleAsync(integrationEvent);

        Assert.Equal(2, consumer.Attempts);
        Assert.True(store.IsProcessed(
            nameof(RetryableConsumer),
            integrationEvent.EventId));
    }

    private static BookingCreatedEvent CreateEvent() =>
        new(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid(),
            new[] { Guid.NewGuid() });

    private sealed class TestBookingRepository : IBookingRepository
    {
        public Booking? SavedBooking { get; private set; }

        public Task AddAsync(
            Booking booking,
            CancellationToken cancellationToken = default)
        {
            SavedBooking = booking;
            return Task.CompletedTask;
        }
    }

    private sealed class FailingBookingRepository : IBookingRepository
    {
        public Task AddAsync(
            Booking booking,
            CancellationToken cancellationToken = default) =>
            Task.FromException(
                new InvalidOperationException("Simulated repository failure."));
    }

    private sealed class TestAvailabilityPort : ISeatAvailabilityPort
    {
        private readonly bool _available;

        public TestAvailabilityPort(bool available)
        {
            _available = available;
        }

        public Task<bool> AreAvailableAsync(
            Guid eventId,
            IReadOnlyCollection<Guid> seatIds,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_available);
    }

    private sealed class RecordingEventPublisher : IEventPublisher
    {
        public IIntegrationEvent? PublishedEvent { get; private set; }

        public Task PublishAsync(
            IIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            PublishedEvent = integrationEvent;
            return Task.CompletedTask;
        }
    }

    private sealed class FailingConsumer
        : IEventConsumer<BookingCreatedEvent>
    {
        public Task HandleAsync(
            BookingCreatedEvent integrationEvent,
            CancellationToken cancellationToken = default) =>
            Task.FromException(
                new InvalidOperationException("Simulated consumer failure."));
    }

    private sealed class RetryableConsumer
        : IdempotentConsumer<BookingCreatedEvent>
    {
        public int Attempts { get; private set; }

        public RetryableConsumer(IProcessedEventStore store)
            : base(nameof(RetryableConsumer), store)
        {
        }

        protected override Task ProcessAsync(
            BookingCreatedEvent integrationEvent,
            CancellationToken cancellationToken)
        {
            Attempts++;

            if (Attempts == 1)
                throw new InvalidOperationException("Simulated temporary failure.");

            return Task.CompletedTask;
        }
    }
}
