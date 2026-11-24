# ADR-0001: Architectural Style Selection

## Status

Accepted

## Date

2026-11-24

## Decision Owners

EventFlow architecture and development team.

## Context

EventFlow is an online event discovery and ticket booking platform. Its main capabilities include event catalogue management, interactive seat selection, temporary reservations, payment integration, ticket issuance, identity management, and anti-scalping controls.

The system must preserve booking correctness during concurrent requests, handle traffic peaks during popular ticket releases, integrate with external services, and remain maintainable as business capabilities evolve.

The initial project must also avoid unnecessary operational complexity. The team needs an architecture that supports clear module boundaries, automated testing, controlled deployment, and future scaling.

The following architectural drivers are particularly important:

- NFR-01 and NFR-02: booking transaction correctness and workflow completeness.
- NFR-03 and NFR-04: response time and concurrent user capacity.
- NFR-07 and NFR-08: availability and fault recovery.
- NFR-11: modularity and testability.
- NFR-12: scalability and deployment adaptability.
- QAS-01: concurrent seat booking.
- QAS-02: peak ticket release load.
- QAS-03: payment provider failure.

## Decision

EventFlow will use a **Modular Monolith architecture with Clean/Hexagonal Architecture principles inside the application**.

The application will be organized into business modules with explicit responsibilities and controlled dependencies. Each module will expose defined interfaces and own its application-level business rules.

The initial logical modules are:

1. Identity and Access Management.
2. Event Catalogue.
3. Venue and Seat Inventory.
4. Booking and Reservation.
5. Payment Integration.
6. Ticket Issuance.
7. Notifications.
8. Administration.

The architecture will follow these principles:

- Business logic will be separated from infrastructure and user interface concerns.
- Modules will communicate through explicit application interfaces.
- Direct access to another module's internal implementation will be avoided.
- The Booking module will own reservation and booking state transitions.
- Database transactions and integrity constraints will protect critical booking invariants.
- External integrations will be isolated behind adapters.
- Asynchronous operations may use a transactional outbox and a message broker when justified by implementation needs.
- The initial deployment may run as one application unit, while allowing multiple application instances behind a load balancer.

The architecture will not require a separate deployable microservice for every business module.

## Alternatives Considered

### Alternative A: Layered Monolithic Architecture

A traditional layered monolith separates presentation, business logic, and data access into technical layers.

**Advantages:**
- Simple to understand and implement.
- Low initial infrastructure and deployment complexity.
- Suitable for a small development team.

**Disadvantages:**
- Business capabilities may become tightly coupled across layers.
- Module ownership and boundaries can become unclear.
- Changes in one business area may affect unrelated areas.
- Large shared service classes can reduce testability and maintainability.

**Reason for rejection:** A traditional layered structure alone does not provide sufficiently explicit business module boundaries for the EventFlow domain.

### Alternative B: Microservices Architecture

Each business capability is implemented and deployed as an independent service.

**Advantages:**
- Independent deployment and scaling of services.
- Clear service ownership when organizational boundaries are established.
- Failure isolation can be improved through service-level resilience patterns.
- Different modules may evolve independently.

**Disadvantages:**
- Higher infrastructure and operational complexity.
- Network communication introduces latency and additional failure modes.
- Distributed transactions and eventual consistency complicate booking workflows.
- Requires mature observability, service discovery, deployment automation, and incident management.
- Higher implementation and maintenance cost for an initial platform.

**Reason for rejection:** The expected benefits do not currently justify the additional distributed-systems complexity and operational overhead.

### Alternative C: Modular Monolith with Clean/Hexagonal Architecture

A single deployable application contains explicitly separated business modules and isolates domain logic from infrastructure.

**Advantages:**
- Clear business boundaries without mandatory distributed deployment.
- Easier transaction management for booking operations.
- Lower operational complexity than microservices.
- Supports automated testing and replacement of infrastructure adapters.
- Provides a migration path toward independent services if future evidence justifies it.

**Disadvantages:**
- Modules still share the same deployment lifecycle.
- Scaling an individual module independently is limited.
- Strong architectural discipline is required to prevent internal coupling.

**Reason for selection:** It provides the best balance between transactional consistency, maintainability, development cost, and future flexibility for the initial EventFlow release.

## Consequences

### Positive Consequences

- Booking and reservation operations can use local database transactions.
- Business responsibilities are separated into explicit modules.
- External providers can be replaced through adapters without rewriting core business logic.
- The initial deployment and operations remain relatively simple.
- Automated unit and integration testing can focus on module boundaries.
- The system retains an evolution path toward independently deployable services.

### Negative Consequences and Trade-offs

- All modules initially share one application deployment lifecycle.
- A failure in a shared application process may affect multiple modules.
- Independent scaling of a single business module is not available without additional architectural work.
- Module boundaries must be enforced through code review, dependency rules, and automated tests.

### Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Modules become tightly coupled | Enforce explicit interfaces and dependency rules |
| Application becomes too large | Monitor module complexity and define extraction criteria |
| Shared deployment affects availability | Use multiple application instances and health checks |
| Booking logic becomes dependent on infrastructure | Keep domain rules isolated from database and provider adapters |

## Quality Attribute Impact

| Quality attribute | Expected impact |
|---|---|
| Functional Suitability | Supports explicit ownership of booking and event workflows |
| Performance Efficiency | Avoids unnecessary internal network calls between modules |
| Compatibility | Supports external integrations through adapters |
| Interaction Capability | Keeps presentation concerns separate from business logic |
| Reliability | Allows transactional booking logic and controlled failure handling |
| Security | Supports centralized enforcement points and module-level authorization checks |
| Maintainability | Provides explicit module boundaries and testable business logic |
| Flexibility | Allows future extraction of selected modules when justified |

## Traceability

- NFR-01, NFR-02, NFR-11, NFR-12.
- QAS-01, QAS-02, QAS-03, QAS-05.
- Architecture viewpoints: VP-02 Functional and Logical, VP-06 Deployment and Operational, VP-07 Quality Attribute.

## Review Conditions

This decision should be reconsidered if:
- Independent deployment of business modules becomes a strong business requirement.
- Measured workloads show that individual modules require fundamentally different scaling strategies.
- The development organization grows enough to support independent service ownership.
- Operational capabilities mature sufficiently to manage distributed services reliably.
