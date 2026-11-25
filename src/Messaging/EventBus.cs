using System.Collections.Concurrent;
using EventFlow.Events;
using EventFlow.Application;

namespace EventFlow.Messaging;

public interface IEventBus : IEventPublisher
{
    void Subscribe<TEvent>(
        string consumerName,
        Func<TEvent, CancellationToken, Task> handler)
        where TEvent : IIntegrationEvent;

    Task DrainAsync(CancellationToken cancellationToken = default);
}

public interface IProcessedEventStore
{
    bool TryMarkAsProcessed(string consumerName, Guid eventId);
}

public sealed class InMemoryProcessedEventStore : IProcessedEventStore
{
    private readonly ConcurrentDictionary<string, byte> _processed = new();

    public bool TryMarkAsProcessed(string consumerName, Guid eventId)
    {
        var key = $"{consumerName}:{eventId}";
        return _processed.TryAdd(key, 0);
    }
}

public sealed class InMemoryEventBus : IEventBus
{
    private sealed record Subscription(
        string ConsumerName,
        Func<IIntegrationEvent, CancellationToken, Task> Handler);

    private readonly ConcurrentDictionary<Type, List<Subscription>> _subscriptions = new();
    private readonly ConcurrentBag<Task> _pending = new();
    private readonly object _subscriptionLock = new();

    public void Subscribe<TEvent>(
        string consumerName,
        Func<TEvent, CancellationToken, Task> handler)
        where TEvent : IIntegrationEvent
    {
        lock (_subscriptionLock)
        {
            var list = _subscriptions.GetOrAdd(typeof(TEvent), _ => new List<Subscription>());
            list.Add(new Subscription(
                consumerName,
                (e, ct) => handler((TEvent)e, ct)));
        }
    }

    public Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        Console.WriteLine(
            $"[PUBLISH] {typeof(TEvent).Name}; EventId={integrationEvent.EventId}; " +
            $"AggregateId={integrationEvent.AggregateId}");

        Subscription[] subscribers;
        lock (_subscriptionLock)
        {
            subscribers = _subscriptions.TryGetValue(typeof(TEvent), out var list)
                ? list.ToArray()
                : Array.Empty<Subscription>();
        }

        foreach (var subscriber in subscribers)
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    await subscriber.Handler(integrationEvent, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"[CONSUMER-ERROR] {subscriber.ConsumerName}: {ex.Message}");
                }
            }, CancellationToken.None);

            _pending.Add(task);
        }

        // Не очікуємо завершення обробників: видавець лише передає подію шині.
        return Task.CompletedTask;
    }

    public async Task DrainAsync(CancellationToken cancellationToken = default)
    {
        // Метод використовується демонстрацією та тестами для очікування фонових задач.
        while (!_pending.IsEmpty)
        {
            var tasks = _pending.ToArray();
            if (tasks.Length == 0) break;
            await Task.WhenAll(tasks).WaitAsync(cancellationToken);

            while (_pending.TryTake(out _)) { }
        }
    }
}

public abstract class IdempotentConsumer<TEvent>
    where TEvent : IIntegrationEvent
{
    private readonly IProcessedEventStore _store;
    protected string ConsumerName { get; }

    protected IdempotentConsumer(
        string consumerName,
        IProcessedEventStore store)
    {
        ConsumerName = consumerName;
        _store = store;
    }

    public async Task HandleAsync(
        TEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        if (!_store.TryMarkAsProcessed(ConsumerName, integrationEvent.EventId))
        {
            Console.WriteLine(
                $"[DUPLICATE-SKIPPED] consumer={ConsumerName}; " +
                $"EventId={integrationEvent.EventId}");
            return;
        }

        await ProcessAsync(integrationEvent, cancellationToken);

        Console.WriteLine(
            $"[PROCESSED] consumer={ConsumerName}; EventId={integrationEvent.EventId}");
    }

    protected abstract Task ProcessAsync(
        TEvent integrationEvent,
        CancellationToken cancellationToken);
}

public sealed class BookingNotificationConsumer
    : IdempotentConsumer<BookingCreatedEvent>
{
    private int _notificationsSent;

    public int NotificationsSent => _notificationsSent;

    public BookingNotificationConsumer(IProcessedEventStore store)
        : base("BookingNotificationConsumer", store) { }

    protected override async Task ProcessAsync(
        BookingCreatedEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        await Task.Delay(80, cancellationToken);
        Interlocked.Increment(ref _notificationsSent);
        Console.WriteLine(
            $"[NOTIFICATION] Надіслано сповіщення клієнту {integrationEvent.CustomerId}");
    }
}

public sealed class BookingAnalyticsConsumer
    : IdempotentConsumer<BookingCreatedEvent>
{
    private int _bookingsCounted;

    public int BookingsCounted => _bookingsCounted;

    public BookingAnalyticsConsumer(IProcessedEventStore store)
        : base("BookingAnalyticsConsumer", store) { }

    protected override async Task ProcessAsync(
        BookingCreatedEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        await Task.Delay(120, cancellationToken);
        Interlocked.Increment(ref _bookingsCounted);
        Console.WriteLine(
            $"[ANALYTICS] Бронювання {integrationEvent.AggregateId} враховано в аналітиці");
    }
}
