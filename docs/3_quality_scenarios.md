# 3. Quality Attribute Scenarios

## 3.1. Purpose

This document defines measurable Quality Attribute Scenarios (QAS) for EventFlow. The scenarios translate the non-functional requirements into concrete situations that can be used to evaluate architectural decisions and verify the system quality.

Each scenario follows the six-part structure:
1. Source — entity that generates the stimulus.
2. Stimulus — event or condition affecting the system.
3. Environment — conditions under which the stimulus occurs.
4. Artifact — system element affected by the stimulus.
5. Response — expected system behaviour.
6. Response Measure — measurable criterion used to evaluate the response.

The scenarios are derived from the requirements in `docs/2_nfr_specification.md`.

## 3.2. Quality Scenario QAS-01: Concurrent Seat Booking

- **Quality Attribute:** Reliability
- **Related Requirements:** NFR-01, NFR-02, NFR-08
- **Priority:** Critical

| Scenario Element | Description |
|---|---|
| Source | Two or more authenticated customers attempting to reserve the same seat. |
| Stimulus | Multiple reservation requests for the same event and seat arrive concurrently. |
| Environment | A popular event is on sale. The booking service is operating under normal or elevated load, and all requests reach the system within a short time interval. |
| Artifact | Seat reservation logic, booking service, and transactional database. |
| Response | The system processes competing requests atomically. It grants the reservation to at most one customer, rejects the other requests with a clear seat-unavailable response, and preserves consistent booking state. |
| Response Measure | Exactly one active reservation may exist for the seat at a time. No duplicate confirmed bookings are created. All rejected requests receive a deterministic business response. The result is verified through at least 100 concurrent attempts against the same seat. |

### Architectural Significance

This scenario requires reliable concurrency control and transaction consistency. The architecture must prevent race conditions when multiple users attempt to reserve the same seat. The database and booking workflow must enforce seat uniqueness independently of client-side validation.

## 3.3. Quality Scenario QAS-02: Peak Ticket Release Load

- **Quality Attribute:** Performance Efficiency
- **Related Requirements:** NFR-03, NFR-04, NFR-07
- **Priority:** High

| Scenario Element | Description |
|---|---|
| Source | Customers accessing EventFlow when ticket sales for a highly anticipated event begin. |
| Stimulus | A sudden increase in concurrent browsing, seat map retrieval, and booking requests occurs. |
| Environment | A ticket release period with 5,000 concurrent active users and a target workload of 200 API requests per second, sustained for at least 15 minutes after warm-up. |
| Artifact | API gateway or application entry point, event catalogue, seat map service, booking service, database, and cache. |
| Response | The system continues serving requests without uncontrolled degradation. It applies admission control or rate limiting where necessary and preserves the correctness of booking operations. |
| Response Measure | Event catalogue and event details requests achieve p95 response time of no more than 500 ms. Seat map retrieval achieves p95 response time of no more than 1,000 ms. Booking confirmation requests achieve p95 response time of no more than 2,000 ms, excluding external payment processing. Non-business-error rate remains below 1%. |

### Architectural Significance

This scenario validates the system's ability to handle demand spikes. It influences decisions about horizontal scaling, caching, database capacity, request throttling, and separation of resource-intensive operations.

## 3.4. Quality Scenario QAS-03: Payment Provider Failure

- **Quality Attribute:** Reliability
- **Related Requirements:** NFR-01, NFR-05, NFR-08
- **Priority:** Critical

| Scenario Element | Description |
|---|---|
| Source | External payment provider. |
| Stimulus | The payment provider becomes unavailable, returns an error, or delays its response while a customer is completing a booking. |
| Environment | A customer has an active temporary seat reservation and submits a payment request during a partial or complete payment provider outage. |
| Artifact | Payment integration component, booking state machine, reservation storage, and payment notification handler. |
| Response | The system records the payment attempt and its state, prevents an unverified payment from producing a confirmed booking, handles retries safely, and processes delayed provider notifications idempotently. The customer receives a clear status and can retry or recover according to the reservation policy. |
| Response Measure | No booking is marked as paid or confirmed without verified payment authorization. Repeated delivery of the same payment notification creates no duplicate payment record or ticket. All affected booking attempts have an auditable final or recoverable state. |

### Architectural Significance

This scenario requires clear separation between booking and payment states, idempotent integration operations, controlled retry policies, and reliable processing of asynchronous payment notifications. It also motivates the use of timeouts and failure isolation.

## 3.5. Quality Scenario QAS-04: Automated Reservation Abuse

- **Quality Attribute:** Security
- **Related Requirements:** NFR-01, NFR-09, NFR-10
- **Priority:** Critical

| Scenario Element | Description |
|---|---|
| Source | An automated client or malicious actor attempting to acquire a large number of tickets unfairly. |
| Stimulus | The actor sends repeated authentication, seat reservation, or checkout requests at a rate exceeding normal human interaction patterns. |
| Environment | A high-demand event is on sale. The attacker uses automated scripts and may distribute requests across multiple sessions or network addresses. |
| Artifact | Authentication and authorization components, API entry point, reservation service, rate limiter, and security audit subsystem. |
| Response | The system detects suspicious activity, applies configurable rate limits and abuse controls, rejects or challenges excessive requests, records relevant security events, and preserves legitimate customer access where possible. |
| Response Measure | Requests exceeding configured limits are throttled or rejected. Unauthorized operations cannot create or modify another user's bookings. Automated abuse tests demonstrate that repeated requests cannot bypass seat uniqueness or reservation expiration rules. Security events include timestamps, relevant account or session identifiers, and correlation identifiers where available. |

### Architectural Significance

This scenario influences identity management, rate limiting, abuse detection, audit logging, and reservation policy design. Protection must be layered because IP-based blocking alone is insufficient against distributed automation.

## 3.6. Quality Scenario QAS-05: Recovery After Infrastructure Failure

- **Quality Attribute:** Reliability
- **Related Requirements:** NFR-07, NFR-08, NFR-11
- **Priority:** Critical

| Scenario Element | Description |
|---|---|
| Source | Infrastructure failure, such as an application instance crash or database service interruption. |
| Stimulus | An active EventFlow component becomes unavailable during normal operation or while booking requests are being processed. |
| Environment | The production environment is handling active customer sessions and booking transactions. The failure occurs without prior warning. |
| Artifact | Application instances, database, booking persistence layer, monitoring, backup, and recovery mechanisms. |
| Response | The system detects the failure, isolates the unavailable component, restores service using healthy instances or recovery procedures, and reconciles interrupted operations without creating duplicate or inconsistent bookings. |
| Response Measure | The service is restored within the target Recovery Time Objective (RTO) of 2 hours. Data loss does not exceed the target Recovery Point Objective (RPO) of 15 minutes. After recovery, booking and payment records remain consistent and no duplicate confirmed tickets are present. |

### Architectural Significance

This scenario requires monitoring, health checks, backup and restoration procedures, fault isolation, and documented recovery processes. It also highlights the need to design transaction handling so that interrupted operations can be safely reconciled.

## 3.7. Scenario-to-Requirement Traceability

| Scenario | Primary quality attribute | Related NFRs | Main architectural concern |
|---|---|---|---|
| QAS-01 | Reliability | NFR-01, NFR-02, NFR-08 | Concurrent transaction consistency |
| QAS-02 | Performance Efficiency | NFR-03, NFR-04, NFR-07 | Peak load and horizontal scalability |
| QAS-03 | Reliability | NFR-01, NFR-05, NFR-08 | Payment failure handling and idempotency |
| QAS-04 | Security | NFR-01, NFR-09, NFR-10 | Anti-automation and access protection |
| QAS-05 | Reliability | NFR-07, NFR-08, NFR-11 | Fault recovery and data integrity |

## 3.8. Validation and Testing Strategy

| Scenario | Recommended validation method | Evidence |
|---|---|---|
| QAS-01 | Concurrent booking integration test | Transaction results and duplicate-seat assertions |
| QAS-02 | Load and stress test | Throughput, p95 latency, and error-rate report |
| QAS-03 | Payment provider failure simulation | Payment and booking state transition logs |
| QAS-04 | Security and abuse simulation | Rate-limit results, authorization tests, and audit events |
| QAS-05 | Failure injection and recovery exercise | Recovery duration, data consistency checks, and backup restoration report |

All scenarios shall be validated in a controlled test environment before production acceptance. Thresholds are project targets and may be refined after representative measurements and stakeholder review.

## 3.9. Conclusion

The five quality attribute scenarios describe the most significant architectural risks for EventFlow: concurrent seat allocation, peak demand, payment provider failures, automated reservation abuse, and infrastructure recovery.

Each scenario provides a measurable basis for evaluating architectural alternatives and verifying that the system meets its critical quality objectives. The scenarios directly support the architectural decisions documented in the ADRs.
