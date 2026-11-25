using EventFlow.Events;
using EventFlow.Messaging;

Console.OutputEncoding = new System.Text.UTF8Encoding(false);

var eventBus = new InMemoryEventBus();
var processedStore = new InMemoryProcessedEventStore();

var notificationConsumer = new BookingNotificationConsumer(processedStore);
var analyticsConsumer = new BookingAnalyticsConsumer(processedStore);

eventBus.Subscribe(notificationConsumer);
eventBus.Subscribe(analyticsConsumer);

var bookingId = Guid.NewGuid();

var bookingCreated = new BookingCreatedEvent(
    Guid.NewGuid(),
    DateTimeOffset.UtcNow,
    bookingId,
    Guid.NewGuid(),
    new[] { Guid.NewGuid(), Guid.NewGuid() });

Console.WriteLine("=== EVENT-DRIVEN ARCHITECTURE DEMO ===");

await eventBus.PublishAsync(bookingCreated);
await eventBus.DrainAsync();

Console.WriteLine("=== RE-PUBLISHING THE SAME EVENT ===");

await eventBus.PublishAsync(bookingCreated);
await eventBus.DrainAsync();

Console.WriteLine("=== RESULTS ===");
Console.WriteLine($"Notification consumer count: {notificationConsumer.ProcessedCount}");
Console.WriteLine($"Analytics consumer count: {analyticsConsumer.ProcessedCount}");

if (notificationConsumer.ProcessedCount != 1 ||
    analyticsConsumer.ProcessedCount != 1)
{
    throw new InvalidOperationException("Idempotency verification failed.");
}

Console.WriteLine("[PASS] Both consumers processed the event exactly once.");
Console.WriteLine("[PASS] Duplicate delivery was skipped.");
