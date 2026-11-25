# EventFlow
## Online Ticket Booking & Event Platform

**Project type:** High-load distributed ticket booking platform  
**Academic project:** Laboratory Work No. 1  
**Discipline:** Software Architecture Design, Specification and Engineering Standards  
**Variant:** 4

---

## 1. Project Overview

EventFlow is an online ticket booking and event management platform designed for selling tickets to concerts, sports competitions, and other large-scale events.

The platform provides users with the ability to discover events, browse interactive venue seating plans, select seats, temporarily reserve them, complete payments, and receive electronic tickets.

The system is designed to handle high traffic during ticket releases, prevent double booking, ensure payment consistency, and mitigate automated bot activity and ticket scalping.

## 2. Business Objectives

The primary business objectives are:

- Provide a reliable and accessible online ticket purchasing experience.
- Support high-demand ticket releases and sudden traffic spikes.
- Prevent double booking and unauthorized seat acquisition.
- Ensure secure payment processing and protection of customer information.
- Provide event organizers with tools to manage events, venues, seating configurations, and ticket inventory.
- Maintain operational efficiency and support future platform growth.

## 3. Scope

### 3.1. In Scope

- User registration and authentication.
- Event discovery, search, and filtering.
- Event details and venue information.
- Interactive venue seating maps.
- Real-time seat availability updates.
- Temporary seat reservation with expiration.
- Order creation and management.
- Integration with external payment providers.
- Electronic ticket generation and validation.
- Event organizer management interface.
- Administrative event and venue management.
- Anti-bot and anti-scalping mechanisms.
- Notifications about booking and payment status.
- Operational monitoring and audit logging.

### 3.2. Out of Scope

- Physical event organization and venue operations.
- Direct management of banking infrastructure.
- Development of proprietary payment processing networks.
- Physical ticket printing and delivery.
- Transportation and accommodation booking.
- Resale marketplace between individual users.

## 4. Primary Stakeholders

| Stakeholder | Primary Responsibilities |
|---|---|
| Ticket Buyer | Searches for events and purchases tickets |
| Event Organizer | Creates events and manages ticket inventory |
| System Administrator | Manages platform configuration and users |
| Business Owner | Defines business objectives and investment constraints |
| DevOps Engineer | Maintains infrastructure, deployment, and monitoring |
| Security Officer | Ensures information security and regulatory compliance |
| Payment Provider | Processes external payment transactions |

## 5. Architectural Quality Goals

The architecture is expected to address the following quality characteristics defined by ISO/IEC 25010:2023:

- **Performance Efficiency:** Low response latency and high transaction throughput.
- **Reliability:** Consistent booking operations and service availability.
- **Security:** Protection of personal information, authentication credentials, and payment-related data.
- **Functional Suitability:** Correct event discovery, reservation, ordering, and ticket issuance.
- **Compatibility:** Reliable interaction with external payment and notification providers.
- **Interaction Capability:** Clear and accessible user interfaces.
- **Maintainability:** Modular implementation and automated testing.
- **Flexibility:** Ability to scale and evolve as business requirements change.

## 6. Key Architectural Challenges

The most important architectural challenges include:

1. **Concurrent Seat Booking:** Multiple users may attempt to reserve the same seat simultaneously.
2. **Traffic Spikes:** Popular event releases can generate exceptionally high request volumes.
3. **Reservation Expiration:** Temporarily reserved seats must become available again when the reservation expires.
4. **Payment Consistency:** Payment confirmation and ticket issuance must remain consistent despite network failures.
5. **Bot Protection:** Automated purchasing activity must be detected and mitigated.
6. **Real-Time Availability:** Users should receive sufficiently fresh information about seat availability.
7. **Fault Recovery:** The system must recover from service and infrastructure failures without losing confirmed transactions.

## 7. Architectural Approach

The initial architectural direction is to evaluate a modular architecture with clearly defined domain boundaries.

The architecture will be assessed against the following concerns:

- Transaction consistency.
- Horizontal scalability.
- Concurrency control.
- Service availability.
- Security and access control.
- Operational complexity.
- Development and maintenance costs.

Final architectural decisions will be documented in Architecture Decision Records (ADRs).

## 8. Technology Considerations

The following technologies are candidates for architectural evaluation, not yet finalized implementation decisions:

| Area | Candidate Technologies |
|---|---|
| Backend | Java / Spring Boot |
| Frontend | React / TypeScript |
| Primary Database | PostgreSQL |
| Caching | Redis |
| Message Broker | RabbitMQ / Apache Kafka |
| Authentication | Keycloak / OAuth 2.0 / OpenID Connect |
| Containerization | Docker |
| Orchestration | Kubernetes |
| Monitoring | Prometheus / Grafana |
| API Communication | REST / WebSocket |

Technology selection will be justified through ADR documents.

## 9. Standards and References

The project documentation follows the principles and terminology of:

- ISO/IEC 25010:2023 — Product quality model.
- ISO/IEC/IEEE 42010:2022 — Architecture description.
- SWEBOK Guide v4.0 — Software Engineering Body of Knowledge.

## 10. Documentation Structure

| File | Description |
|---|---|
| `docs/1_system_description.md` | System description, scope, stakeholders, and concerns |
| `docs/2_nfr_specification.md` | Non-functional requirements |
| `docs/3_quality_scenarios.md` | Quality Attribute Scenarios |
| `docs/4_viewpoints_map.md` | Architecture viewpoints and views |
| `docs/adrs/0001-architectural-style-selection.md` | Architectural style decision |
| `docs/adrs/0002-database-and-storage-strategy.md` | Database and storage decision |
| `docs/adrs/0003-authentication-and-security-framework.md` | Authentication and security decision |

## 11. Project Status

**Current phase:** Architecture requirements specification.

The initial deliverables focus on identifying stakeholders and concerns, defining measurable quality requirements, developing quality attribute scenarios, and documenting architectural decisions.

---

**Project:** EventFlow  
**Variant:** 4 — Online Ticket Booking & Event Platform

## Laboratory Work 6 — Final Architecture

- `Architectural_Design_Document_ADD.md` — consolidated arc42 Architecture Design Document.
- `compose.yaml` and `Dockerfile` — single-container .NET 8 prototype deployment.
- `docs/diagrams/deployment-eventflow.puml` — UML Deployment Diagram source.
- `docs/adrs/0004-deployment-topology.md` — deployment topology ADR.
- `Architecture_Presentation.pdf` — architecture defense presentation (generated separately).

The current prototype uses an in-process event bus and in-memory adapters. PostgreSQL, Redis, OIDC, payment, and email integrations are target architecture elements and are not deployed by the current Compose file.
