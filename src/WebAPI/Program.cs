using EventFlow.Application;
using EventFlow.Domain;
using EventFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Singleton: thread-safe in-memory repository shared by requests.
// Scoped: use case is created once per HTTP request.
// Singleton: stateless availability adapter has no request-specific state.
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<ISeatAvailabilityPort, InMemorySeatAvailabilityAdapter>();
builder.Services.AddScoped<CreateBookingUseCase>();

var app = builder.Build();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();

public partial class Program { }
