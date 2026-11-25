using EventFlow.Application;
using EventFlow.Domain;
using EventFlow.Infrastructure;
using EventFlow.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<ISeatAvailabilityPort, InMemorySeatAvailabilityAdapter>();

// The prototype hosts the event bus and both independent consumers
// in the same ASP.NET Core process. No external broker is deployed.
builder.Services.AddSingleton<InMemoryEventBus>();
builder.Services.AddSingleton<IEventBus>(
    services => services.GetRequiredService<InMemoryEventBus>());
builder.Services.AddSingleton<IEventPublisher>(
    services => services.GetRequiredService<InMemoryEventBus>());

builder.Services.AddSingleton<IProcessedEventStore, InMemoryProcessedEventStore>();
builder.Services.AddSingleton<BookingNotificationConsumer>();
builder.Services.AddSingleton<BookingAnalyticsConsumer>();

builder.Services.AddScoped<CreateBookingUseCase>();

var app = builder.Build();

// Register both independent subscribers before accepting HTTP requests.
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe(
    app.Services.GetRequiredService<BookingNotificationConsumer>());
eventBus.Subscribe(
    app.Services.GetRequiredService<BookingAnalyticsConsumer>());

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    application = "EventFlow",
    runtime = ".NET 8",
    eventBus = "in-memory"
}));

app.Run();

public partial class Program { }
