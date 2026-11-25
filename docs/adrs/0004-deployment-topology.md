# ADR-0004: Deployment Topology for the Initial EventFlow Prototype

## Status
Accepted for the Lab 6 prototype

## Date
2026-11-25

## Context
The preceding laboratory work established a modular monolith with Clean Architecture boundaries, a REST API, integration-event contracts, and an in-memory event bus with two independent idempotent consumers.

The current source code contains:
- ASP.NET Core WebAPI targeting .NET 8.
- Domain and Application projects.
- In-memory repository and seat-availability adapters.
- An in-memory event bus.
- Notification and analytics consumers.
- An in-memory processed-event store.

The current source code does not contain configured PostgreSQL, Redis, RabbitMQ, Kafka, an OIDC provider, or working payment/email adapters. The deployment view must describe deployable artifacts and actual runtime relationships rather than presenting planned technologies as running services.

The deployment must remain simple enough for a laboratory demonstration and preserve the modular-monolith decision from ADR-0001.

## Decision
Deploy the EventFlow prototype as one Docker container named `eventflow-api`.

The container runs the .NET 8 ASP.NET Core application and hosts:
1. REST controllers.
2. Application use cases.
3. Domain model.
4. In-memory infrastructure adapters.
5. In-memory event bus.
6. Booking notification consumer.
7. Booking analytics consumer.
8. In-memory idempotency store.

The host publishes TCP port 8080 and maps it to container port 8080. The application protocol is HTTP with JSON payloads for the local laboratory demonstration.

The event bus dispatches subscribers asynchronously inside the same process. Both consumers are registered before the application begins accepting requests. Each consumer uses the shared processed-event store and checks EventId before processing.

The deployment uses a multi-stage Docker build:
- SDK image for restore and publish.
- ASP.NET Core 8 runtime image for execution.

The Docker Compose topology contains only the `eventflow-api` service because no other infrastructure service is currently implemented and connected.

PostgreSQL, Redis, identity provider, payment provider, and email provider remain target or external systems. They are not represented as running Compose services and are not claimed as operational integrations.

## Alternatives Considered

### Alternative A: One application container with in-process messaging
Advantages:
- Matches the current code and modular-monolith architecture.
- No broker or distributed deployment dependency.
- Simple local demonstration.
- Consumers can be tested independently.
Disadvantages:
- Event delivery is volatile and process-local.
- Consumers cannot scale independently.
- Process failure interrupts API and consumers.
- In-memory idempotency state is lost on restart.

### Alternative B: Separate API and worker containers with a message broker
Advantages:
- Independent worker deployment and scaling.
- Broker can buffer messages and support durable delivery.
- Better isolation between API and background processing.
Disadvantages:
- Requires a real broker, serialization, connection configuration, retries, and operational monitoring.
- The current implementation has no broker adapter.
- Introducing a broker would exceed the current prototype implementation.

### Alternative C: API with PostgreSQL and Redis
Advantages:
- Persistent authoritative data.
- Better alignment with the target storage strategy.
- More realistic recovery and concurrency guarantees.
Disadvantages:
- Database adapters, schema migrations, configuration, and integration tests are not present in the current code.
- Redis must not become the authoritative source of seat ownership.
- Adding these services without implementing adapters would create a misleading deployment view.

## Consequences

### Positive
- Deployment diagram corresponds to actual executable code and Compose topology.
- One container is sufficient to demonstrate REST booking creation and asynchronous event handling.
- Clean Architecture boundaries remain intact.
- No fictitious broker, database, or worker process is introduced.

### Negative
- Data and processed-event identifiers are volatile.
- A restart loses bookings and idempotency history.
- Consumers share CPU and memory with the API.
- The local HTTP demonstration is not a production security configuration.

## Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Loss of data after restart | Explicitly document prototype limitation; use persistent database adapter before production |
| Lost events during process termination | Adopt transactional outbox and durable broker before production |
| Duplicate event processing | Keep EventId-based idempotency; use durable processed-event storage in production |
| Consumer exception | Isolate consumer execution and retain retry semantics |
| Unencrypted local HTTP | Use TLS termination and HTTPS for a real deployment |
| Single-container failure | Add health monitoring and redundant instances in a production topology |

## Traceability
- ADR-0001: Modular Monolith and Clean/Hexagonal Architecture.
- ADR-0002: PostgreSQL authoritative storage and Redis supporting cache (target).
- ADR-0003: OIDC/OAuth 2.0 security strategy (target; not configured in prototype).
- Lab 4: Clean Architecture and dependency injection.
- Lab 5: integration events, asynchronous consumers, and idempotency.
- Lab 6: deployment diagram, Compose topology, and ADD.

## Review Conditions
Revisit this decision when persistent database adapters, durable event transport, production authentication, or independent consumer scaling become implemented requirements.
