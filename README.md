# InteractHub Backend

> ASP.NET Core 8.0 Web API cho ứng dụng mạng xã hội **InteractHub**  
> Môn học: C# và .NET Development

---

## Kiến Trúc

```
interacthub-backend/
├── InteractHub.API            # Presentation Layer (Controllers, Middlewares)
├── InteractHub.Core           # Domain Layer (Entities, DTOs, Interfaces)
├── InteractHub.Infrastructure # Infrastructure Layer (EF Core, Repositories, Services)
└── InteractHub.Tests          # Unit & Integration Tests
```

## Nuget Packages

```
╰─ grep -r "<PackageReference" --include="*.csproj" -rn . | sed 's|./\([^/]*\)/.*Include="\([^"]*\)".*|\1 → \2|' | sort
InteractHub.API → FluentValidation.AspNetCore
InteractHub.API → Microsoft.AspNetCore.Authentication.JwtBearer
InteractHub.API → Microsoft.AspNetCore.OpenApi
InteractHub.API → Microsoft.AspNetCore.SignalR
InteractHub.API → Serilog.AspNetCore
InteractHub.API → Serilog.Sinks.Console
InteractHub.API → Serilog.Sinks.File
InteractHub.API → Swashbuckle.AspNetCore

InteractHub.Core → Microsoft.AspNetCore.Identity.EntityFrameworkCore

InteractHub.Infrastructure → Azure.Storage.Blobs
InteractHub.Infrastructure → Microsoft.AspNetCore.Identity.EntityFrameworkCore
InteractHub.Infrastructure → Microsoft.EntityFrameworkCore.Design
InteractHub.Infrastructure → Npgsql.EntityFrameworkCore.PostgreSQL
InteractHub.Infrastructure → Microsoft.EntityFrameworkCore.Tools

InteractHub.Tests → coverlet.collector
InteractHub.Tests → FluentAssertions
InteractHub.Tests → Microsoft.AspNetCore.Mvc.Testing
InteractHub.Tests → Microsoft.EntityFrameworkCore.InMemory
InteractHub.Tests → Microsoft.NET.Test.Sdk
InteractHub.Tests → Moq
InteractHub.Tests → xunit
InteractHub.Tests → xunit.runner.visualstudio

```
---

## Tech Stack

| Thành phần     | Công nghệ                      |
| -------------- | ------------------------------ |
| Framework      | ASP.NET Core 8.0               |
| ORM            | Entity Framework Core 8.0      |
| Database       | PostgreSQL                     |
| Authentication | JWT + ASP.NET Core Identity    |
| Real-time      | SignalR                        |
| Storage        | Azure Blob Storage             |
| API Docs       | Swagger / OpenAPI              |
| Testing        | xUnit + Moq + FluentAssertions |
| CI/CD          | GitHub Actions                 |
| Cloud          | Microsoft Azure                |

---

## Yêu Cầu Hệ Thống

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (để chạy PostgreSQL)
- Visual Studio 2022 / VS Code / Rider

## Tài Liệu Liên Quan

- [`/docs/erd.png`](docs/erd.png) — Sơ đồ ERD database
- [`/docs/api.md`](docs/api.md) — Danh sách API endpoint chi tiết
- [`/docs/deployment.md`](docs/deployment.md) — Hướng dẫn deploy Azure