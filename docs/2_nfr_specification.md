# 2. Non-Functional Requirements Specification

## 2.1. Purpose and Scope

This document defines the non-functional requirements (NFRs) for EventFlow, an online event discovery and ticket booking platform with interactive seat selection and anti-scalping protections.

The requirements describe measurable quality expectations that influence the system architecture, implementation, deployment, operation, and verification.

The specification follows the eight product quality characteristics listed in the laboratory methodological guidelines based on ISO/IEC 25010:2023.

The requirements are identified using the prefix `NFR` and a unique numeric identifier. Each requirement includes:
- Quality characteristic and subcharacteristic.
- Requirement statement.
- Measurable acceptance criteria.
- Priority.
- Verification method.

All numerical thresholds in this document are proposed project targets. They must be validated through performance testing, security assessment, and stakeholder approval before production release.

## 2.2. Requirement Prioritization

The following priority scale is used:

| Priority | Meaning |
|---|---|
| Critical | Mandatory for the system to provide its core service safely and correctly |
| High | Essential for acceptable quality and operational reliability |
| Medium | Important, but can be improved after the initial release |

## 2.3. Quality Model Coverage

The specification covers the following eight quality characteristics:

| ISO/IEC 25010:2023 characteristic | Relevant subcharacteristics | Main EventFlow concern |
|---|---|---|
| Functional Suitability | Functional completeness, correctness, appropriateness | Correct ticket booking and payment workflows |
| Performance Efficiency | Time behaviour, resource utilization, capacity | High demand during ticket releases |
| Compatibility | Co-existence, interoperability | Integration with payment, identity, and notification providers |
| Interaction Capability | Appropriateness recognizability, learnability, user error protection | Accessible and understandable booking experience |
| Reliability | Maturity, availability, fault tolerance, recoverability | Continuous operation and prevention of lost bookings |
| Security | Confidentiality, integrity, non-repudiation, authenticity, accountability | Account protection, payment integrity, anti-scalping |
| Maintainability | Modularity, reusability, analysability, modifiability, testability | Safe evolution and efficient defect resolution |
| Flexibility | Adaptability, scalability, installability | Growth in traffic and deployment environments |

## 2.4. Detailed Non-Functional Requirements

### 2.4.1. Functional Suitability

#### NFR-01 — Booking Transaction Correctness

- **Characteristic:** Functional Suitability
- **Subcharacteristic:** Functional correctness
- **Priority:** Critical
- **Requirement:** The system shall guarantee that a seat cannot be assigned to more than one confirmed booking for the same event.
- **Acceptance criteria:**
  - Under concurrent booking attempts for the same seat, no more than one booking may reach the confirmed state.
  - Every confirmed booking must reference a valid event, seat, customer, and successful payment or an explicitly authorized payment state.
  - The system must preserve booking consistency when concurrent requests are processed.
- **Verification:** Automated integration tests and concurrent transaction tests against the booking service and database.

#### NFR-02 — Booking Workflow Completeness

- **Characteristic:** Functional Suitability
- **Subcharacteristic:** Functional completeness
- **Priority:** Critical
- **Requirement:** The system shall support the complete ticket purchase lifecycle, including event selection, seat selection, temporary reservation, payment processing, ticket issuance, and booking status retrieval.
- **Acceptance criteria:**
  - Every successful purchase produces exactly one valid ticket for each purchased seat.
  - An expired or cancelled reservation cannot be converted into a valid ticket.
  - Users can retrieve the current status of their bookings.
- **Verification:** End-to-end functional tests covering successful, failed, cancelled, and expired booking workflows.

### 2.4.2. Performance Efficiency

#### NFR-03 — Response Time

- **Characteristic:** Performance Efficiency
- **Subcharacteristic:** Time behaviour
- **Priority:** High
- **Requirement:** The system shall provide responsive interactions during normal and elevated traffic.
- **Acceptance criteria:**
  - Event catalogue and event details API requests shall achieve a response time of no more than 500 ms at the 95th percentile under the defined normal load.
  - Seat map retrieval shall achieve a response time of no more than 1,000 ms at the 95th percentile under the defined normal load.
  - Booking confirmation API requests shall achieve a response time of no more than 2,000 ms at the 95th percentile, excluding external payment provider processing time.
- **Verification:** Load testing with response time percentile measurements and distributed tracing.

#### NFR-04 — Concurrent User Capacity

- **Characteristic:** Performance Efficiency
- **Subcharacteristic:** Capacity
- **Priority:** High
- **Requirement:** The system shall support increased traffic during popular event ticket releases.
- **Acceptance criteria:**
  - The initial production configuration shall support at least 5,000 concurrent active users.
  - The system shall process at least 200 API requests per second during the defined load test.
  - Under the target load, the error rate for non-business-error requests shall remain below 1%.
  - Performance measurements shall be collected during a test lasting at least 15 minutes after the initial warm-up period.
- **Verification:** Load and stress testing using a representative event browsing and booking workload.

### 2.4.3. Compatibility

#### NFR-05 — External Service Interoperability

- **Characteristic:** Compatibility
- **Subcharacteristic:** Interoperability
- **Priority:** High
- **Requirement:** The system shall exchange data with external payment, identity, and notification services through documented and version-controlled interfaces.
- **Acceptance criteria:**
  - External integrations shall use documented API contracts.
  - Payment callbacks shall be authenticated and validated before changing booking status.
  - Repeated delivery of the same payment notification shall not create duplicate transactions or tickets.
  - Integration failures shall be logged with a correlation identifier.
- **Verification:** Contract tests, integration tests, and simulated duplicate or invalid callback tests.

### 2.4.4. Interaction Capability

#### NFR-06 — Booking Usability and Error Prevention

- **Characteristic:** Interaction Capability
- **Subcharacteristics:** Learnability and user error protection
- **Priority:** High
- **Requirement:** The booking interface shall help users understand seat availability, reservation status, and the next required action while preventing avoidable booking errors.
- **Acceptance criteria:**
  - Available, temporarily reserved, and unavailable seats shall have visually distinguishable states that do not rely on color alone.
  - The interface shall display the reservation expiration time while a temporary reservation is active.
  - Users shall receive clear feedback when a selected seat becomes unavailable.
  - Critical booking actions shall provide a clear confirmation or error message.
- **Verification:** Usability testing with representative users, accessibility review, and UI acceptance tests.

### 2.4.5. Reliability

#### NFR-07 — Service Availability

- **Characteristic:** Reliability
- **Subcharacteristic:** Availability
- **Priority:** Critical
- **Requirement:** The production system shall remain accessible and operational during normal service conditions.
- **Acceptance criteria:**
  - The monthly availability target for core browsing and booking APIs shall be at least 99.9%.
  - Planned maintenance shall be excluded from the availability calculation only when announced and approved according to the operational policy.
  - Service health shall be monitored through automated health checks and alerting.
- **Verification:** Production monitoring, uptime measurements, and deployment recovery exercises.

#### NFR-08 — Fault Tolerance and Recovery

- **Characteristic:** Reliability
- **Subcharacteristic:** Fault tolerance and recoverability
- **Priority:** Critical
- **Requirement:** The system shall handle temporary failures of external services and recover from application or infrastructure failures without creating inconsistent booking or payment records.
- **Acceptance criteria:**
  - Temporary payment provider failures shall not result in a confirmed booking without verified payment authorization.
  - Repeated processing of the same payment event shall be idempotent.
  - Database backups shall be created at least once every 24 hours.
  - The target Recovery Point Objective (RPO) shall not exceed 15 minutes, and the target Recovery Time Objective (RTO) shall not exceed 2 hours.
- **Verification:** Failure injection tests, backup restoration tests, and disaster recovery exercises.

### 2.4.6. Security

#### NFR-09 — Authentication and Access Control

- **Characteristic:** Security
- **Subcharacteristics:** Authenticity and confidentiality
- **Priority:** Critical
- **Requirement:** The system shall authenticate users securely and enforce authorization rules for all protected operations.
- **Acceptance criteria:**
  - Protected API endpoints shall reject requests without valid authentication credentials.
  - Users shall only access or modify bookings they are authorized to manage.
  - Administrative operations shall require explicitly assigned administrative permissions.
  - Passwords, where locally managed, shall never be stored in plaintext and shall use an industry-accepted password hashing algorithm.
  - All external communication involving authentication or personal data shall use HTTPS with TLS 1.2 or later.
- **Verification:** Authentication and authorization tests, configuration review, and security testing.

#### NFR-10 — Anti-Scalping and Abuse Protection

- **Characteristic:** Security
- **Subcharacteristics:** Integrity and accountability
- **Priority:** Critical
- **Requirement:** The system shall reduce automated abuse, mass reservation attempts, and unfair ticket acquisition.
- **Acceptance criteria:**
  - The system shall apply configurable rate limits to authentication, reservation, and checkout endpoints.
  - Suspicious request patterns shall be detected and recorded for investigation.
  - Reservation attempts shall be associated with an authenticated user or an explicitly defined anonymous session.
  - Repeated requests shall not bypass seat uniqueness or reservation expiration rules.
  - Anti-abuse controls shall not rely exclusively on IP address blocking.
- **Verification:** Security testing, rate-limit tests, automated abuse simulations, and audit log review.

### 2.4.7. Maintainability

#### NFR-11 — Modular Design and Testability

- **Characteristic:** Maintainability
- **Subcharacteristics:** Modularity and testability
- **Priority:** High
- **Requirement:** The system shall maintain clear boundaries between its main business capabilities and provide automated tests for critical business logic.
- **Acceptance criteria:**
  - Event management, booking, payment integration, identity, and ticket issuance shall have clearly defined responsibilities and interfaces.
  - Critical booking and payment business rules shall be covered by automated tests.
  - Changes to an external integration shall not require unrelated business modules to be rewritten.
  - Build and test execution shall be reproducible in the documented development environment.
- **Verification:** Architecture review, automated test reports, and build pipeline inspection.

### 2.4.8. Flexibility

#### NFR-12 — Scalability and Deployment Adaptability

- **Characteristic:** Flexibility
- **Subcharacteristics:** Scalability and adaptability
- **Priority:** High
- **Requirement:** The system shall support scaling of resource-intensive components and deployment in different environments without changing core business logic.
- **Acceptance criteria:**
  - Application instances shall be deployable independently of persistent database storage.
  - Configuration values and secrets shall be externalized from application source code.
  - The deployment process shall support separate development, testing, and production environments.
  - The architecture shall allow application capacity to be increased by adding instances, subject to shared resource constraints.
- **Verification:** Deployment tests, configuration review, and horizontal scaling experiments.

## 2.5. Summary of Requirements and Priorities

| ID | Quality characteristic | Requirement | Priority |
|---|---|---|---|
| NFR-01 | Functional Suitability | Booking transaction correctness | Critical |
| NFR-02 | Functional Suitability | Booking workflow completeness | Critical |
| NFR-03 | Performance Efficiency | Response time | High |
| NFR-04 | Performance Efficiency | Concurrent user capacity | High |
| NFR-05 | Compatibility | External service interoperability | High |
| NFR-06 | Interaction Capability | Booking usability and error prevention | High |
| NFR-07 | Reliability | Service availability | Critical |
| NFR-08 | Reliability | Fault tolerance and recovery | Critical |
| NFR-09 | Security | Authentication and access control | Critical |
| NFR-10 | Security | Anti-scalping and abuse protection | Critical |
| NFR-11 | Maintainability | Modular design and testability | High |
| NFR-12 | Flexibility | Scalability and deployment adaptability | High |

## 2.6. Verification Strategy

NFR verification shall be integrated into the software development and release process.

| Verification activity | Requirements covered | Expected evidence |
|---|---|---|
| Unit and integration testing | NFR-01, NFR-02, NFR-05, NFR-08, NFR-09 | Automated test reports |
| End-to-end testing | NFR-01, NFR-02, NFR-06 | Booking workflow test results |
| Load and stress testing | NFR-03, NFR-04 | Response time, throughput, and error rate reports |
| Availability monitoring | NFR-07 | Uptime and incident reports |
| Security testing | NFR-05, NFR-09, NFR-10 | Security assessment and test results |
| Architecture and code review | NFR-11, NFR-12 | Review records and architecture documentation |
| Recovery testing | NFR-08 | Backup restoration and recovery exercise reports |

## 2.7. Assumptions and Constraints

1. The initial performance targets are architectural objectives and shall be validated against the expected production workload.
2. External payment provider availability and processing time are outside the direct control of EventFlow.
3. Availability calculations shall use a documented monitoring period and exclude only explicitly approved maintenance windows.
4. Security controls shall be reviewed against applicable data protection and payment processing obligations before production deployment.
5. The architecture may evolve as measured load, operational experience, and stakeholder priorities become clearer.

## 2.8. Traceability to Architectural Decisions

The requirements in this document provide input for the Architecture Decision Records (ADRs):

| Requirement IDs | Architectural concern | Related ADR |
|---|---|---|
| NFR-01, NFR-02, NFR-11, NFR-12 | Architectural style and module boundaries | ADR-0001 |
| NFR-01, NFR-03, NFR-04, NFR-07, NFR-08 | Transaction consistency, persistence, caching, and capacity | ADR-0002 |
| NFR-05, NFR-09, NFR-10 | Identity integration, authorization, and security controls | ADR-0003 |

## 2.9. Conclusion

The EventFlow NFR specification defines twelve measurable requirements across the eight quality characteristics used in the laboratory methodological guidelines.

The most critical architectural drivers are booking correctness, reliability, security, and protection against automated abuse. Performance, interoperability, usability, maintainability, and scalability provide additional constraints for architecture design and future system evolution.

These requirements will be used to derive quality attribute scenarios and evaluate architectural decisions in the following documents.
