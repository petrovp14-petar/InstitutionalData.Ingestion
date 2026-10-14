# Secure Institutional Data Ingestion Component

## 📌 Project Overview
This repository contains a production-grade, asynchronous **Data Ingestion and Middleware Component** built with **C# and .NET 10**. It functions as a decoupled backend synchronization engine designed to securely ingest, validate, and persist high-volume identity and record data from external platforms into a core relational system.

I chose this specific project architecture because it directly mirrors the high-security operational challenges, strict data integrity constraints, and middleware processing flows found in complex higher-education enterprise environments.

---

## 🛠️ Key Architectural Highlights

* **Clean Architecture & Separation of Concerns:** Uses a highly decoupled structure utilizing the Service and Repository design patterns to cleanly split data access from business execution rules.
* **Modern C# 13/14 Paradigm:** Leverages file-scoped namespaces and C# Primary Constructors to eliminate unnecessary injection boilerplate and keep code layouts highly maintainable.
* **Input Enforcement & Validation:** Utilizes custom Data Transfer Objects (DTOs) backed by regular expressions and data annotations to validate payloads and shield system tables from malformed entries.
* **SQL Injection Mitigation:** Leverages Dapper micro-ORM with explicit object-parameter mapping to guarantee data execution layer safety without compromising on transaction speed.
* **Robust Error Scoping:** Pairs task-based asynchronous processing (`async/await`) with structural logging boundaries to ensure critical transaction contexts are captured and never swallowed silently.
* **Decoupled Quality Assurance:** Includes an isolated testing project featuring **xUnit** and **Moq** to assert service behavior entirely independent of physical data store configurations.

---

## 📂 Project Structure
```text
📦 src
 ┣ 📜 UserIngestionDto.cs            # Data Transfer Object with input validation rules
 ┣ 📜 IUserRepository.cs            # Abstract interface contract for data persistence
 ┣ 📜 UserRepository.cs              # Secure data access infrastructure via Dapper
 ┣ 📜 DataIngestionService.cs        # Core business workflow and sync logic engine
 ┗ 📜 InstitutionalData.csproj       # .NET 10 compilation configurations & dependencies
📦 tests
 ┗ 📜 DataIngestionServiceTests.cs   # Unit tests with data layer interface mocking
📜 .gitignore                        # Global build-artifact exclusion filters
📜 README.md                        # Project documentation and architectural overview
```

---

## ⚙️ Technology Stack & Dependencies
* **Language Runtime:** .NET 10.0 / C#
* **Micro-ORM:** Dapper (v2.1.35)
* **Unit Testing Stack:** xUnit (v2.4.2) & Moq (v4.18.4)
* **Logging Interface:** Microsoft.Extensions.Logging.Abstractions (v10.0.0)

---

## 🗄️ Target Database Schema (SQL Server)
The repository infrastructure layer interacts with a standard relational storage layer matching the structural DDL script below. A non-clustered index is established on the lookup handle to ensure high concurrency optimization under heavy synchronization loads:

```sql
CREATE TABLE dbo.ApplicationUsers (
    SystemId CHAR(9) NOT NULL PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL
);

CREATE NONCLUSTERED INDEX IX_ApplicationUsers_Email ON dbo.ApplicationUsers(Email);
```

---

## 🚀 Building and Running Tests

### Prerequisites
* .NET 10.0 SDK installed on your local environment.

### Build the Application
Navigate to the root directory containing your solution file and run:
```bash
dotnet build
```

### Run the Unit Test Suite
To execute the validation and synchronization boundary tests without needing a physical database connection, run:
```bash
dotnet test
```
