# Enterprise Order Management System

A production-ready **Order Management System** built with **.NET 8** and **Angular**. This project serves as a comprehensive showcase of modern enterprise C# architecture, advanced Domain-Driven Design (DDD), and key Gang of Four (GoF) design patterns.

---

## 🛠️ Core Engineering Skills & Competencies Showcase

### 1. Architecture & System Design
* **Clean & Hexagonal Architecture (Ports & Adapters):** Complete decoupling of core domain logic from external dependencies (frameworks, ORMs, UI).
* **CQRS (Command Query Responsibility Segregation):** Strict separation of write operations (state mutations) and read operations for optimized throughput.

### 2. Design Patterns & Advanced C# Techniques
* **Behavioral Patterns:**
  * **State Pattern:** Encapsulates complex order lifecycle transitions (`Pending` $\rightarrow$ `Paid` $\rightarrow$ `Shipped` $\rightarrow$ `Cancelled`), ensuring invalid state transitions are caught at the domain level.
  * **Strategy Pattern:** Flexible pricing and discount calculations (`PercentageDiscount`, `FlatDiscount`).
  * **Observer Pattern & Domain Events:** Decoupled side-effect handling (`OrderPlacedDomainEvent` triggering notifications and inventory updates).
  * **Chain of Responsibility:** Extensible validation pipelines (`StockValidator` $\rightarrow$ `CreditLimitValidator`).
* **Creational Patterns:**
  * **Factory Method Pattern:** Dynamic payment gateway instantiation (`PaymentGatewayFactory` creating `StripeAdapter` / `PayPalAdapter`).
  * **Builder Pattern:** Fluent domain object construction (`OrderBuilder`) for testing and domain scenarios.
* **Structural Patterns:**
  * **Decorator Pattern:** Non-intrusive caching (`CachingOrderQueryDecorator`) over query handlers using `IMemoryCache`.
  * **Adapter Pattern:** Wrapping third-party external SDKs into domain port interfaces.

### 3. Data Persistence & SQL Optimization
* **Entity Framework Core 8:** Configured using Shadow Properties (`_stateName`) to map state objects cleanly without polluting the domain model.
* **Auto-Reconstruction Mechanism:** Custom state reconstruction handlers preventing materialization nullability issues during ORM mapping.
* **SQL Server Integration:** Production-grade relational schema, automated migrations, and connection handling.

---

## 🚀 Tech Stack

* **Backend:** .NET 8 Web API, C#, Entity Framework Core 8
* **Frontend:** Angular, RxJS, TypeScript
* **Database:** SQL Server
* **Architecture:** Clean Architecture, Hexagonal Architecture, CQRS, DDD

---

## ⚙️ Quick Start

### 1. Configure Database Connection
Update `Api.Presentation/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OrderManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

### 2. Run Database Migrations
```bash
dotnet ef database update --project Infrastructure --startup-project Api.Presentation
```

### 3. Launch Applications
* **Backend:**
  ```bash
  dotnet run --project Api.Presentation
  ```
* **Frontend:**
  ```bash
  cd frontend
  npm install
  ng serve
  ```