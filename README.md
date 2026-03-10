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
InteractHub.API → AutoMapper
InteractHub.API → AutoMapper.Extensions.Microsoft.DependencyInjection
InteractHub.API → FluentValidation.AspNetCore
InteractHub.API → Microsoft.AspNetCore.Authentication.JwtBearer
InteractHub.API → Microsoft.AspNetCore.OpenApi
InteractHub.API → Microsoft.EntityFrameworkCore.Design
InteractHub.API → Serilog.AspNetCore
InteractHub.API → Serilog.Sinks.Console
InteractHub.API → Serilog.Sinks.File
InteractHub.API → Swashbuckle.AspNetCore

InteractHub.Application → AutoMapper
InteractHub.Application → FluentValidation
InteractHub.Application → FluentValidation.DependencyInjectionExtensions
InteractHub.Application → MediatR

InteractHub.Infrastructure → Microsoft.AspNetCore.Identity.EntityFrameworkCore
InteractHub.Infrastructure → Microsoft.EntityFrameworkCore
InteractHub.Infrastructure → Microsoft.EntityFrameworkCore.SqlServer
InteractHub.Infrastructure → Microsoft.EntityFrameworkCore.Tools
InteractHub.Infrastructure → System.IdentityModel.Tokens.Jwt

```

**Dependency rule:**
```
API → Core ← Infrastructure
```
Core không phụ thuộc bất kỳ layer nào khác.

---

## Tech Stack

| Thành phần | Công nghệ |
|-----------|-----------|
| Framework | ASP.NET Core 8.0 |
| ORM | Entity Framework Core 8.0 |
| Database | SQL Server |
| Authentication | JWT + ASP.NET Core Identity |
| Real-time | SignalR |
| Storage | Azure Blob Storage |
| API Docs | Swagger / OpenAPI |
| Testing | xUnit + Moq + FluentAssertions |
| CI/CD | GitHub Actions |
| Cloud | Microsoft Azure |

---

## Yêu Cầu Hệ Thống

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local hoặc Azure SQL)
- Visual Studio 2022 / VS Code / Rider

---

## Hướng Dẫn Chạy Local

### 1. Clone repo

```bash
git clone https://github.com/<your-org>/interacthub-backend.git
cd interacthub-backend
```

### 2. Tạo file cấu hình local

Tạo file `InteractHub.API/appsettings.Development.json` (file này đã bị gitignore):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InteractHubDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-minimum-32-characters",
    "Issuer": "InteractHub",
    "Audience": "InteractHub",
    "ExpirationInMinutes": 60
  },
  "AzureBlobStorage": {
    "ConnectionString": "your-azure-blob-connection-string",
    "ContainerName": "interacthub-media"
  }
}
```

### 3. Chạy Migration

```bash
dotnet ef database update \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API
```

### 4. Chạy ứng dụng

```bash
dotnet run --project InteractHub.API
```

### 5. Mở Swagger

```
https://localhost:5001/swagger
```

---

## Chạy Tests

```bash
dotnet test
```

Xem code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

##  Branch Strategy

| Branch | Mục đích |
|--------|----------|
| `main` | Production — chỉ merge từ develop |
| `dev` | Staging — tích hợp các feature |
| `feature/*` | Tính năng mới |
| `fix/*` | Bug fix |
| `test/*` | Unit test |

**Quy trình:** `feature/xxx` → PR → `dev`
**Note**: CHỈ ĐƯỢC PUSH LÊN DEV

---

## API Endpoints

Xem đầy đủ tại Swagger UI sau khi chạy project: `/swagger`

| Controller | Base Route |
|-----------|------------|
| AuthController | `/api/auth` |
| PostsController | `/api/posts` |
| UsersController | `/api/users` |
| FriendsController | `/api/friends` |
| StoriesController | `/api/stories` |
| NotificationsController | `/api/notifications` |

---

##  Phân Công Backend Team

<!-- | Thành viên | Phụ trách |
|-----------|-----------|
| Leader | Architecture, Auth (B3), CI/CD |
| Dev 1 | Database & Entities (B1) |
| Dev 2 | API Controllers & DTOs (B2) |
| Dev 3 | Service Layer & Business Logic (B4) | -->

---

## Tài Liệu Liên Quan

- [`/docs/erd.png`](docs/erd.png) — Sơ đồ ERD database
- [`/docs/api.md`](docs/api.md) — Danh sách API endpoint chi tiết
- [`/docs/deployment.md`](docs/deployment.md) — Hướng dẫn deploy Azure