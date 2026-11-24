# ADR-0002: Database and Storage Strategy

## Status

Accepted

## Date

2026-11-24

## Decision Owners

EventFlow architecture and development team.

## Context

EventFlow processes event information, venue layouts, seat availability, temporary reservations, bookings, payments, and issued tickets.

The most important data integrity concern is preventing two customers from obtaining the same seat for the same event. The platform must also support peak traffic, recover from failures, and integrate reliably with external payment services.

The architecture requires a clear distinction between authoritative business data and temporary or derived data.

Relevant architectural drivers include:

- NFR-01: booking transaction correctness.
- NFR-03 and NFR-04: response time and concurrent user capacity.
- NFR-07 and NFR-08: availability and recovery.
- NFR-11 and NFR-12: maintainability and scalability.
- QAS-01: concurrent seat booking.
- QAS-02: peak ticket release load.
- QAS-03: payment provider failure.
- QAS-05: recovery after infrastructure failure.

## Decision

EventFlow will use **PostgreSQL as the primary transactional database**.

Redis may be used as a supporting in-memory data store for caching, rate limiting, and other temporary data. Redis will not be the authoritative source of seat ownership, confirmed bookings, or payment status.

The storage strategy is defined as follows.

### 1. PostgreSQL — Authoritative Business Data

PostgreSQL will store:
- User references and relevant identity metadata.
- Events and event configuration.
- Venues and seat definitions.
- Reservations and booking records.
- Payment transaction references and statuses.
- Issued ticket records.
- Audit records required for business traceability.

The database will enforce critical integrity rules through transactions, foreign keys, unique constraints, and appropriate indexes.

The booking workflow must guarantee that competing requests cannot create multiple active or confirmed allocations for the same seat and event.

The exact implementation may use database constraints, transactional locking, or both, depending on the final data model and concurrency tests.

### 2. Redis — Supporting Ephemeral Storage

Redis may be used for:
- Frequently accessed event catalogue data.
- Cached venue and seat map representations.
- API rate limiting counters.
- Short-lived coordination data where appropriate.
- Temporary data that can be reconstructed or safely discarded.

Redis must not be the only protection against duplicate seat allocation. Final reservation correctness must be enforced by the authoritative transactional database.

Cache invalidation and expiration policies must be documented for data that can change during ticket sales.

### 3. Transactional Outbox — Reliable Event Publication

When a database transaction changes business state and requires an event to be published, the application may store the event in an outbox table within the same PostgreSQL transaction.

A separate publisher can deliver pending events to a message broker or integration adapter.

This approach reduces the risk of committing a booking while losing the corresponding event notification.

Consumers must process messages idempotently because message delivery may be repeated.

A dedicated message broker is an implementation option when asynchronous workload and operational requirements justify it; it is not mandatory for the initial deployment.

### 4. Backups and Recovery

The database must support:
- Automated backups at least once every 24 hours.
- A target Recovery Point Objective (RPO) of no more than 15 minutes.
- A target Recovery Time Objective (RTO) of no more than 2 hours.
- Periodic restoration tests.
- Restricted access to backup storage.

The RPO target may require point-in-time recovery and continuous write-ahead log archiving in addition to periodic full backups. The exact backup configuration must be validated in the selected hosting environment.

## Alternatives Considered

### Alternative A: Relational Database Only

Use PostgreSQL for all persistent and temporary application data, without a separate in-memory store.

**Advantages:**
- Simpler infrastructure.
- One authoritative storage system.
- Strong transaction and integrity capabilities.
- Fewer cache consistency concerns.

**Disadvantages:**
- Repeated read-heavy requests may increase database load.
- Rate limiting and frequently accessed derived data may require additional database operations.
- Some temporary workloads may be handled less efficiently.

**Reason for rejection:** PostgreSQL remains the authoritative store, but a supporting cache may improve performance when measurements justify it.

### Alternative B: PostgreSQL with Redis

Use PostgreSQL for authoritative transactional data and Redis for cache and temporary supporting workloads.

**Advantages:**
- Strong relational consistency for bookings and payments.
- Faster access to frequently requested derived data.
- Efficient support for rate limiting and short-lived data.
- Clear separation between authoritative and disposable data.

**Disadvantages:**
- Additional infrastructure and operational monitoring.
- Cache invalidation must be handled carefully.
- Temporary state in Redis must not replace transactional correctness.
- Additional failure modes must be considered.

**Reason for selection:** This strategy balances transactional correctness and performance while keeping the system's source of truth explicit.

### Alternative C: NoSQL-First Storage

Use a document or key-value database as the primary data store.

**Advantages:**
- Flexible data structures.
- Potentially useful for certain high-volume access patterns.
- Horizontal scaling options depending on the selected technology.

**Disadvantages:**
- Relational constraints and transactional relationships may require additional application-level logic.
- Seat allocation and payment consistency become more difficult to reason about.
- The EventFlow domain contains strongly related entities and transactional invariants.

**Reason for rejection:** A relational database is a more suitable primary store for the initial booking and payment domain.

## Consequences

### Positive Consequences

- PostgreSQL provides a single authoritative source for booking and payment state.
- Database transactions and constraints protect seat allocation integrity.
- Redis can improve read performance and support temporary workloads.
- The transactional outbox pattern improves reliability of event publication.
- Backup and recovery objectives are explicitly defined.

### Negative Consequences and Trade-offs

- Redis introduces additional infrastructure and cache consistency concerns.
- Database schema changes require migration planning.
- High write contention during popular ticket releases may require careful indexing, transaction design, and load testing.
- Reliable event publication introduces outbox processing and idempotent consumer requirements.
- RPO and RTO targets require operational investment and regular recovery testing.

### Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Duplicate seat allocation | Database constraints and transactional reservation logic |
| Stale cached seat availability | Short cache lifetimes and invalidation after relevant state changes |
| Redis outage | Fall back to authoritative database behaviour where feasible |
| Lost integration event | Transactional outbox and retryable publisher |
| Database failure | Automated backups, WAL archiving, monitoring, restoration tests |
| Excessive database contention | Load testing, query optimization, indexing, and capacity planning |

## Quality Attribute Impact

| Quality attribute | Expected impact |
|---|---|
| Functional Suitability | Supports correct booking and payment state transitions |
| Performance Efficiency | Redis can reduce repeated reads and support rate limiting |
| Compatibility | PostgreSQL and Redis provide established integration interfaces |
| Interaction Capability | Fast retrieval of event and seat map data supports responsive UI |
| Reliability | Transactions, backups, and outbox processing support recovery |
| Security | Database access control and restricted backup access protect business data |
| Maintainability | Explicit data ownership simplifies reasoning about persistence |
| Flexibility | Supporting storage can be added or scaled as measured needs evolve |

## Traceability

- NFR-01, NFR-03, NFR-04, NFR-07, NFR-08, NFR-11, NFR-12.
- QAS-01, QAS-02, QAS-03, QAS-05.
- Architecture viewpoints: VP-03 Information and Data, VP-04 Interaction and Integration, VP-06 Deployment and Operational.

## Review Conditions

This decision should be reconsidered if:
- Measured database contention cannot be resolved through query and transaction optimization.
- The data model or workload changes substantially.
- Cache usage creates unacceptable consistency or operational risks.
- Recovery objectives cannot be met with the selected database hosting and backup configuration.
