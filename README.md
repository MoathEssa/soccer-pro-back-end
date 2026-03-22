# SoccerPro — Tournament Management System API

A production-grade RESTful API for managing university-level soccer operations: tournaments, teams, players, match scheduling, live match recording, and role-based administration. Built with .NET 9, Clean Architecture, and CQRS — deployed to Azure Container Apps with CI/CD via GitHub Actions and infrastructure-as-code via Bicep.

> **Live API:** https://soccerpro-api.mangoground-9a8d5650.eastus.azurecontainerapps.io/ | **Swagger Docs:** https://soccerpro-api.mangoground-9a8d5650.eastus.azurecontainerapps.io/swagger

---

## Project Overview

SoccerPro is a backend system designed to digitize and centralize the full lifecycle of competitive soccer tournament management. It replaces manual coordination of teams, players, referees, matches, and results with a structured, auditable, and role-secured platform.

**The problem:** Managing multi-team tournaments involves fragmented workflows — player registrations, team rosters, match scheduling, real-time result recording (goals, cards, substitutions), and transfer requests all handled through disconnected processes.

**The solution:** A single API that models the entire tournament domain — from player enrollment and team formation through match scheduling, live result capture (shots, cards, substitutions), and aggregated statistics like top scorers and violation history.

**Built for:** University athletics departments, tournament organizers, and coaching staff who need structured, role-based access to tournament operations.

---

## Key Features

- **Full Tournament Lifecycle** — Create tournaments, assign teams and referees, schedule matches across phases (group stage, knockout, final), and record detailed match outcomes.
- **Granular Match Recording** — Capture shots on goal, card violations, substitutions with reasons, best player selections, and possession rates — all persisted atomically via SQL Server stored procedures with table-valued parameters.
- **Role-Based Access Control** — Six distinct roles (Admin, Coach, Player, Staff, Guest, Manager) enforced through ASP.NET Identity with JWT authentication and HttpOnly secure cookie–based refresh tokens.
- **Player Transfer System** — Request-based workflow for team joins, transfers, and departures with admin approval/rejection processing and status tracking.
- **Bulk Operations** — Batch insertion of players, coaches, and managers with automatic user account provisioning and role assignment per entity.
- **Server-Side Pagination & Search** — All list endpoints support parameterized search with stored procedure–driven pagination returning total counts via output parameters.
- **Automated Input Validation** — FluentValidation integrated into the MediatR pipeline via a custom `ValidationBehavior<TRequest, TResponse>`, rejecting invalid requests before they reach business logic.
- **Database View Projections** — Denormalized SQL views (`PlayerView`, `MatchView`, `TeamView`, `TopScorerPlayerView`, `PlayerViolationView`) for optimized read queries without runtime joins.

---

## Tech Stack

| Layer                 | Technology                                      | Rationale                                                                                                         |
| --------------------- | ----------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| **Runtime**           | .NET 9                                          | Latest LTS-adjacent framework; top-tier performance for API workloads                                             |
| **API**               | ASP.NET Core Web API                            | Convention-based controllers, built-in DI, middleware pipeline                                                    |
| **Auth**              | ASP.NET Identity + JWT Bearer                   | Industry-standard identity management with stateless token auth                                                   |
| **CQRS / Mediator**   | MediatR 12                                      | Decouples controllers from business logic; enables cross-cutting pipeline behaviors                               |
| **Validation**        | FluentValidation 11                             | Declarative, testable input validation with automatic MediatR integration                                         |
| **ORM / Data Access** | EF Core 9 (Identity) + Raw ADO.NET / Dapper     | EF Core for Identity schema; raw SQL + stored procedures for performance-critical tournament queries              |
| **Database**          | SQL Server                                      | Stored procedures, table-valued parameters, SQL views, and output parameters for complex transactional operations |
| **Mapping**           | AutoMapper                                      | Convention-based DTO ↔ Entity mapping reduces boilerplate                                                         |
| **Secrets**           | GitHub Actions Secrets → Container App env vars | Zero secrets in source code; injected at deploy time via Bicep parameters                                         |
| **Infrastructure**    | Azure Bicep                                     | Declarative infrastructure-as-code for Container Apps Environment                                                 |
| **CI/CD**             | GitHub Actions                                  | Automated Docker build, push to ghcr.io, Bicep deploy on push to `main`                                           |
| **Hosting**           | Azure Container Apps (free consumption)         | Serverless container hosting with $0 cost for demo workloads                                                      |
| **Docs**              | Swagger / Swashbuckle                           | Auto-generated OpenAPI spec with JWT security scheme and XML doc comments                                         |

---

## Architecture & Design

### Clean Architecture (4-Layer)

```
┌──────────────────────────────────────────────────┐
│                  SoccerPro.API                    │
│  Controllers → MediatR → Command/Query Handlers  │
├──────────────────────────────────────────────────┤
│              SoccerPro.Application                │
│  Features (CQRS) │ Services │ DTOs │ Validation  │
├──────────────────────────────────────────────────┤
│               SoccerPro.Domain                    │
│  Entities │ Enums │ Repository Interfaces (Ports) │
├──────────────────────────────────────────────────┤
│            SoccerPro.Infrastructure               │
│  Repository Impls │ EF DbContext │ Migrations     │
└──────────────────────────────────────────────────┘
```

**Dependency rule strictly enforced:** Domain has zero dependencies on outer layers. Infrastructure implements interfaces defined in Domain. Application orchestrates through services and MediatR handlers.

### CQRS via MediatR

Every API action flows through a dedicated Command or Query:

```
Controller → _mediator.Send(Command/Query)
  → ValidationBehavior (FluentValidation pipeline)
    → CommandHandler / QueryHandler
      → Service Layer (business rules)
        → Repository (data access)
          → ApiResponse<T> (standardized response)
```

Commands mutate state (e.g., `ScheduleMatchCommand`, `AddPlayerCommand`). Queries are read-only (e.g., `FetchPlayersQuery`, `TopScorerPlayerQuery`). This separation makes it straightforward to optimize reads and writes independently.

### Result Pattern

A custom `Result<T>` type flows through the service layer, carrying either a success value or a typed `Error` with HTTP status code. This eliminates exception-driven control flow for expected failures:

```csharp
Result<T>.Success(value)
Result<T>.Failure(Error.RecoredNotFound("..."), HttpStatusCode.NotFound)
```

Handlers convert `Result<T>` → `ApiResponse<T>` at the boundary, producing consistent JSON responses with `Succeeded`, `StatusCode`, `Message`, `Data`, `Errors`, and `Meta` fields.

### Dual Data Access Strategy

- **EF Core** manages ASP.NET Identity tables (users, roles, refresh tokens) and schema migrations.
- **ADO.NET with stored procedures** handles all tournament domain operations — player inserts with TVPs (table-valued parameters), match result recording with nested shots/cards/substitutions, and paginated search queries with output parameters for total counts.

This hybrid approach leverages EF Core's strength in identity/schema management while using raw SQL for performance-critical, multi-table transactional writes that would be cumbersome through an ORM.

### Global Exception Handling

A custom middleware catches and normalizes all unhandled exceptions into the `ApiResponse<T>` envelope:

- `ValidationException` → 422 Unprocessable Entity
- `UnauthorizedAccessException` → 401
- `KeyNotFoundException` → 404
- `SqlException` 2627/2601 (unique constraint) → 409 Conflict
- `SqlException` 547 (FK violation) → 400 Bad Request
- `SqlException` 1205 (deadlock) → 409 with retry hint

No raw stack traces leak to clients.

### Stored Procedures & Database Design

Stored procedures are the backbone of all domain data operations. The API delegates complex, multi-table transactional logic to SQL Server rather than orchestrating it in application code.

| Stored Procedure                      | Purpose                                                                     | Key Technique                                                                                                       |
| ------------------------------------- | --------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `SP_InsertPlayerWithMultipleContacts` | Creates a Person + Player + N contact records in one call                   | TVP (`ContactInfoType`) for contacts; `OUTPUT @PersonId` returned to caller                                         |
| `SP_InsertFullMatchRecord`            | Records a complete match result — match record, shots, cards, substitutions | 3 TVPs (`ShotsOnGoalType`, `CardsViloationsType_V2`, `MatchSubstitutionsType`) in a single transactional round-trip |
| `SP_InsertMatchRecord`                | Records a match record with output ID                                       | `OUTPUT @InsertedMatchRecoredId` for downstream inserts                                                             |
| `SP_SearchPlayers`                    | Paginated, filtered player search                                           | `OFFSET/FETCH` pagination; `OUTPUT @TotalCount` avoids a separate count query                                       |
| `SP_SearchMatches`                    | Paginated match search by tournament, phase, teams, field, date             | Multi-parameter filtering with server-side pagination                                                               |
| `SP_ScheduleMatch`                    | Creates a match schedule entry with validation                              | Enforces tournament/team/field constraints at the DB level                                                          |
| `SP_AssignPlayerToTeam`               | Assigns a player to a team with role and position                           | Business rule enforcement in SQL                                                                                    |
| `SP_ValidatePlayerTeamsInTeam`        | Validates that a set of player-team IDs belong to a given tournament team   | TVP (`TVP_PlayerTeamIdList`) with return value (1 = valid, 0 = invalid)                                             |

**Table-Valued Parameters (TVPs)** are used extensively to batch related child records into a single procedure call. For example, `SP_InsertFullMatchRecord` accepts the match scalar data alongside three structured tables — shots on goal, card violations, and substitutions — ensuring the entire match result is committed atomically.

**SQL Server Views** provide precomputed read models for high-frequency queries:

- `PlayerView` — Denormalized player + person + department data
- `MatchView` — Match schedule joined with tournament, phase, teams, and field names
- `TeamView` — Team + manager identity
- `TopScorerPlayerView` — Aggregated goal counts per player
- `PlayerViolationView` — Card history per player
- `CoachView`, `RefereeView`, `ManagerSearchView` — Role-specific denormalized views

---

## How to Run the Project

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local or Azure SQL)

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/MoathEssa/soccer-pro-back-end.git
   cd soccer-pro-back-end
   ```

2. **Restore the database**
   Import the included `Soccerpro-2025-5-10-5-26.bacpac` into your SQL Server instance using SSMS or `SqlPackage`:

   ```bash
   sqlpackage /Action:Import /TargetServerName:localhost /TargetDatabaseName:SoccerPro /SourceFile:Soccerpro-2025-5-10-5-26.bacpac
   ```

3. **Configure secrets**

   Use .NET User Secrets:

   ```bash
   cd SoccerPro.API
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=SoccerPro;Trusted_Connection=True;TrustServerCertificate=True;"
   dotnet user-secrets set "JwtSettings:SecretKey" "your-256-bit-secret-key-here"
   ```

4. **Run the API**

   ```bash
   cd SoccerPro.API
   dotnet run
   ```

5. **Access Swagger UI**
   Navigate to `https://localhost:5001/swagger` to explore and test all endpoints.

---

## Live Demo

|                        |                                                                                 |
| ---------------------- | ------------------------------------------------------------------------------- |
| **Backend API**        | https://soccerpro-api.mangoground-9a8d5650.eastus.azurecontainerapps.io/        |
| **API Docs (Swagger)** | https://soccerpro-api.mangoground-9a8d5650.eastus.azurecontainerapps.io/swagger |

---

## Deployment

The backend is deployed to **Azure Container Apps (free consumption tier)** with infrastructure defined as code and fully automated CI/CD.

| Component      | Technology                                      | Files                          |
| -------------- | ----------------------------------------------- | ------------------------------ |
| Infrastructure | Azure Bicep                                     | `infra/main.bicep`             |
| CI/CD          | GitHub Actions                                  | `.github/workflows/deploy.yml` |
| Container      | Docker → GitHub Container Registry (ghcr.io)    | `Dockerfile`                   |
| Hosting        | Azure Container Apps (free consumption, .NET 9) | Resource created by Bicep      |
| Secrets        | GitHub Actions Secrets → Container App env vars | No secrets in source control   |

**How it works:**

1. Push to `main` triggers the GitHub Actions workflow.
2. The workflow builds a Docker image and pushes it to GitHub Container Registry (free).
3. Bicep deploys (or updates) the Container Apps Environment and Container App, injecting all runtime secrets as environment variables.
4. The container starts serving traffic at the Container Apps FQDN.

### Production Configuration

Production secrets are never committed to source control.

- **Local development** → `dotnet user-secrets` or environment variables.
- **CI/CD & production** → GitHub Actions secrets are passed as Bicep parameters at deploy time and land as Container App environment variables.

ASP.NET Core automatically reads environment variables. Double underscores (`__`) map to nested configuration keys:

| Environment Variable                   | Maps To                               |
| -------------------------------------- | ------------------------------------- |
| `ConnectionStrings__DefaultConnection` | `ConnectionStrings:DefaultConnection` |
| `JwtSettings__SecretKey`               | `JwtSettings:SecretKey`               |
| `AppSettings__FrontendBaseUrl`         | `AppSettings:FrontendBaseUrl`         |

### GitHub Actions Secrets

| Secret                      | Purpose                                        |
| --------------------------- | ---------------------------------------------- |
| `AZURE_CREDENTIALS`         | Azure service principal JSON for `azure/login` |
| `CONNECTION_STRING_DEFAULT` | SQL Server connection string                   |
| `JWT_SECRET_KEY`            | JWT signing key (≥ 32 characters)              |
| `FRONTEND_BASE_URL`         | Deployed frontend URL (for CORS)               |

---

## Future Improvements

- **Real-time match updates** — Integrate SignalR for live score broadcasting to connected clients during matches.
- **Caching layer** — Add Redis caching for frequently read data (tournament standings, top scorers) to reduce database pressure during peak tournament activity.
- **Event sourcing for match events** — Model shots, cards, and substitutions as an event stream for complete audit trails and replay capability.
- **Rate limiting and API versioning** — Enforce per-client rate limits and introduce versioned endpoints for backward-compatible API evolution.
- **Automated integration tests** — Build a test suite against an in-memory or containerized SQL Server to validate stored procedure contracts and business rule enforcement.
- **Notification system** — Push notifications for request approvals, match scheduling, and card suspensions via email or in-app alerts.
- **Multi-tenancy** — Extend the platform to support multiple organizations or leagues operating independent tournament structures on a shared infrastructure.

---

## Project Structure

```
SoccerPro.sln
├── SoccerPro.API/              → Controllers, Middleware, Swagger config
├── SoccerPro.Application/      → CQRS Features, Services, DTOs, Validation, Mapping
├── SoccerPro.Domain/           → Entities, Enums, Repository interfaces
└── SoccerPro.Infrastructure/   → Repository implementations, EF DbContext, Migrations
```

---

## License

See [LICENSE.txt](LICENSE.txt) for details.
