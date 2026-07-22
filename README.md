# 📚 Smart Library - From Monolith to Microservices 🚀

> **This isn't just another Library Management System.**
>
> It's my playground for breaking a Monolith into real-world Microservices while learning the architecture, patterns, and challenges that come with distributed systems.

---

## 🎯 Mission

The goal isn't to build the biggest library system.

The goal is to understand **why** companies move from Monoliths to Microservices and implement those concepts from scratch using **.NET 8**.

Every commit pushes the project one step closer to a production-like architecture.

---

# 🧱 Current Architecture

The project started as a **Clean Architecture Monolith**.

Now it's being gradually transformed into independent services.

```text
SmartLibrary

├── 📦 Book Service
├── 📦 Borrow Service
├── 📦 Auth Service
└── 📦 Library.BuildingBlocks
```

Each service owns:

* its own API
* its own Application layer
* its own Domain
* its own Infrastructure
* its own DbContext
* its own Database (coming soon)

No more giant shared DbContext.
No more Generic Repository.

Each service owns its business.

---

# ✅ What's Done

### ✔ Split the Monolith

* Extracted **Book Service**
* Extracted **Borrow Service**
* Extracted **Auth Service**

---

### ✔ Removed Generic Repository

Goodbye...

```csharp
IRepository<T>
```

Hello...

```csharp
DbContext + Specifications
```

Following modern EF Core practices.

---

### ✔ Shared Building Blocks

Created a shared project for common components.

Current shared components include:

* BaseEntity
* OperatingResult
* Common utilities

---

### ✔ Clean Architecture

Every service follows:

```text
API
   ↓
Application
   ↓
Domain
   ↑
Infrastructure
```

Dependencies always point inward.

---

## 🔥 Currently Working On

* Removing direct service dependencies
* Replacing shared entities with HTTP communication
* Implementing service-to-service communication
* Isolating each bounded context

---

# 🛣️ Roadmap

This project is far from finished.

Actually...

The fun part is just getting started.

## Phase 1

* ✅ Split Monolith
* ✅ Independent Services
* ✅ Shared Building Blocks
* ✅ Remove Generic Repository

---

## Phase 2

* ⏳ API Gateway
* ⏳ Service Discovery
* ⏳ Service-to-Service Communication (HttpClient)

---

## Phase 3

* ⏳ JWT Authentication
* ⏳ Refresh Tokens
* ⏳ Authorization Policies

---

## Phase 4

* ⏳ RabbitMQ
* ⏳ Event Driven Architecture
* ⏳ Publish / Subscribe
* ⏳ Integration Events

No more...

```text
Service A
        ↓
calls
        ↓
Service B
```

Instead...

```text
Book Borrowed
        ↓
RabbitMQ
        ↓
Interested Services
```

---

## Phase 5

* ⏳ Docker
* ⏳ Docker Compose
* ⏳ Independent SQL Server for each service
* ⏳ Environment Configuration

---

## Phase 6

* ⏳ Redis Caching
* ⏳ Health Checks
* ⏳ Logging
* ⏳ Centralized Exception Handling
* ⏳ Rate Limiting

---

## Phase 7

* ⏳ Observability

* Serilog

* OpenTelemetry

* Distributed Tracing

* Grafana

* Prometheus

---

# 💡 Why this repository?

Because watching a finished project teaches you **what** to build.

Building it step by step teaches you **why** it was built that way.

This repository documents that journey.

Every refactor, every mistake, every improvement... stays here.

---

# ⚙️ Tech Stack

* ASP.NET Core 8
* Entity Framework Core
* Clean Architecture
* MediatR
* Ardalis Specification
* SQL Server
* REST APIs

---

# 🔮 Coming Next

* API Gateway
* RabbitMQ
* Docker
* Redis
* gRPC
* Distributed Transactions
* Saga Pattern
* Outbox Pattern
* CQRS Improvements
* Event-Driven Communication
* Kubernetes (maybe 😉)

---

> **A monolith is where the journey started.**
>
> **Microservices are where the real adventure begins.** 🚀
