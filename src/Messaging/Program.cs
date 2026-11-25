using EventFlow.Events;
using EventFlow.Messaging;

Console.WriteLine("=== EventFlow Lab 5: Event-Driven Architecture ===");

var store = new InMemoryProcessedEventStore();
var bus = new InMemoryEventBus();

var notificationConsumer = new BookingNotificationConsumer(store);
var analyticsConsumer = new BookingAnalyticsConsumer(store);

bus.Subscribe<BookingCreatedEvent>(
    "BookingNotificationConsumer",
    notificationConsumer.HandleAsync);

bus.Subscribe<BookingCreatedEvent>(
    "BookingAnalyticsConsumer",
    analyticsConsumer.HandleAsync);

var eventId = Guid.NewGuid();
var aggregateId = Guid.NewGuid();
var customerId = Guid.NewGuid();

var bookingEvent = new BookingCreatedEvent(
    eventId,
    DateTimeOffset.UtcNow,
    aggregateId,
    customerId,
    new[] { Guid.NewGuid(), Guid.NewGuid() });

Console.WriteLine("\n--- Перша публікація ---");
await bus.PublishAsync(bookingEvent);

Console.WriteLine("[PUBLISHER] Подію передано шині. Видавець не очікує обробників.");

await bus.DrainAsync();

Console.WriteLine("\n--- Повторна доставка тієї самої події ---");
await bus.PublishAsync(bookingEvent);
await bus.DrainAsync();

Console.WriteLine("\n--- Підсумок ---");
Console.WriteLine($"Сповіщень надіслано: {notificationConsumer.NotificationsSent}");
Console.WriteLine($"Записів аналітики: {analyticsConsumer.BookingsCounted}");

if (notificationConsumer.NotificationsSent != 1 ||
    analyticsConsumer.BookingsCounted != 1)
{
    throw new Exception("Помилка: повторна доставка змінила стан більше одного разу.");
}

Console.WriteLine("[PASS] Обидва споживачі обробили подію рівно один раз.");
Console.WriteLine("[PASS] Повторна доставка пропущена завдяки EventId.");
