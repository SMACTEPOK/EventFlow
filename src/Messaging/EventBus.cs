using System.Collections.Concurrent;
using EventFlow.Application;
using EventFlow.Events;

namespace EventFlow.Messaging;

public interface IEventConsumer<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task HandleAsync(
        TEvent integrationEvent,
        CancellationToken cancellationToken = default);
}

public interface IEventBus : IEventPublisher
{
    void Subscribe<TEvent>(IEventConsumer<TEvent> consumer)
        where TEvent : IIntegrationEvent;

    Task DrainAsync();
}

public sealed class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type,
        ConcurrentBag<Func<IIntegrationEvent, CancellationToken, Task>>> _handlers = new();

    private readonly ConcurrentBag<Task> _pendingTasks = new();

    public void Subscribe<TEvent>(IEventConsumer<TEvent> consumer)
        where TEvent : IIntegrationEvent
    {
        ArgumentNullException.ThrowIfNull(consumer);

        var handlers = _handlers.GetOrAdd(
            typeof(TEvent),
            _ => new ConcurrentBag<Func<IIntegrationEvent, CancellationToken, Task>>());

        handlers.Add((integrationEvent, cancellationToken) =>
            consumer.HandleAsync((TEvent)integrationEvent, cancellationToken));
    }

    public Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        Console.WriteLine(
            $"[PUBLISH] {integrationEvent.GetType().Name}; EventId={integrationEvent.EventId}; AggregateId={integrationEvent.AggregateId}");

        if (!_handlers.TryGetValue(integrationEvent.GetType(), out var handlers))
            return Task.CompletedTask;

        // Each subscriber is dispatched independently.
        foreach (var handler in handlers)
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    await handler(integrationEvent, cancellationToken);
                }
                catch (Exception exception)
                {
                    // A failing consumer does not prevent other consumers from running.
                    Console.WriteLine(
                        $"[CONSUMER-ERROR] EventId={integrationEvent.EventId}; Error={exception.GetType().Name}: {exception.Message}");
                }
            }, CancellationToken.None);

            _pendingTasks.Add(task);
        }

        // Dispatch is asynchronous; consumer execution is not awaited here.
        return Task.CompletedTask;
    }

    public async Task DrainAsync()
    {
        while (_pendingTasks.TryTake(out var task))
            await task;
    }
}

public interface IProcessedEventStore
{
    bool IsProcessed(string consumerName, Guid eventId);
    void MarkProcessed(string consumerName, Guid eventId);
}

public sealed class InMemoryProcessedEventStore : IProcessedEventStore
{
    private readonly ConcurrentDictionary<string, byte> _processed = new();

    private static string Key(string consumerName, Guid eventId) =>
        $"{consumerName}:{eventId:D}";

    public bool IsProcessed(string consumerName, Guid eventId) =>
        _processed.ContainsKey(Key(consumerName, eventId));

    public void MarkProcessed(string consumerName, Guid eventId) =>
        _processed.TryAdd(Key(consumerName, eventId), 0);
}

public abstract class IdempotentConsumer<TEvent> : IEventConsumer<TEvent>
    where TEvent : IIntegrationEvent
{
    private readonly IProcessedEventStore _store;

    // A lock per consumer and event prevents concurrent duplicate execution.
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

    protected IdempotentConsumer(
        string consumerName,
        IProcessedEventStore store)
    {
        ConsumerName = consumerName;
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public string ConsumerName { get; }

    public async Task HandleAsync(
        TEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        var eventLock = _locks.GetOrAdd(
            integrationEvent.EventId,
            _ => new SemaphoreSlim(1, 1));

        await eventLock.WaitAsync(cancellationToken);

        try
        {
            if (_store.IsProcessed(ConsumerName, integrationEvent.EventId))
            {
                Console.WriteLine(
                    $"[DUPLICATE-SKIPPED] consumer={ConsumerName}; EventId={integrationEvent.EventId}");
                return;
            }

            // Mark only after successful processing. Failed processing can be retried.
            await ProcessAsync(integrationEvent, cancellationToken);

            _store.MarkProcessed(ConsumerName, integrationEvent.EventId);

            Console.WriteLine(
                $"[PROCESSED] consumer={ConsumerName}; EventId={integrationEvent.EventId}");
        }
        finally
        {
            eventLock.Release();
        }
    }

    protected abstract Task ProcessAsync(
        TEvent integrationEvent,
        CancellationToken cancellationToken);
}

public sealed class BookingNotificationConsumer
    : IdempotentConsumer<BookingCreatedEvent>
{
    private int _processedCount;

    public BookingNotificationConsumer(IProcessedEventStore store)
        : base(nameof(BookingNotificationConsumer), store)
    {
    }

    public int ProcessedCount => Volatile.Read(ref _processedCount);

    protected override Task ProcessAsync(
        BookingCreatedEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _processedCount);
        Console.WriteLine(
            $"[NOTIFICATION] Booking={integrationEvent.AggregateId}; Customer={integrationEvent.CustomerId}");
        return Task.CompletedTask;
    }
}

public sealed class BookingAnalyticsConsumer
    : IdempotentConsumer<BookingCreatedEvent>
{
    private int _processedCount;

    public BookingAnalyticsConsumer(IProcessedEventStore store)
        : base(nameof(BookingAnalyticsConsumer), store)
    {
    }

    public int ProcessedCount => Volatile.Read(ref _processedCount);

    protected override Task ProcessAsync(
        BookingCreatedEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _processedCount);
        Console.WriteLine(
            $"[ANALYTICS] Booking={integrationEvent.AggregateId}; Seats={integrationEvent.SeatIds.Count}");
        return Task.CompletedTask;
    }
}
