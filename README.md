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

---

## Hướng Dẫn Chạy Local

### Flow nhanh cho team

1. Clone repo về máy.
2. Khởi động PostgreSQL:

```bash
docker compose up --build -d postgres
```

3. Tạo file `InteractHub.API/appsettings.Development.json` nếu máy bạn chưa có. File này phải trỏ tới:

```json
"Host=localhost;Port=5433;Database=interacthubdb;Username=postgres;Password=interacthub@123"
```

4. Restore packages và apply migration:

```bash
dotnet restore
dotnet ef database update --project InteractHub.Infrastructure --startup-project InteractHub.API
```

5. Seed dữ liệu mẫu nếu cần demo/test local:

```bash
docker exec -i interacthub-postgres psql -U postgres -d interacthubdb < InteractHub.Infrastructure/Data/Seeders/reset-and-seed.sql
```

6. Chạy API:

```bash
dotnet run --project InteractHub.API
```

7. Mở Swagger:

```bash
https://localhost:5250/docs
```

Deploy (Render):

```bash
https://csharp-backend.onrender.com/docs
```

**Ghi chú:** `RoleSeeder` sẽ tự seed role/admin khi app khởi động. SQL seed script chỉ dùng khi cần bộ dữ liệu mẫu đầy đủ.

### Cấu hình local mẫu

Tạo file `InteractHub.API/appsettings.Development.json` (**file này đã bị gitignore**):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=interacthubdb;Username=postgres;Password=interacthub@123"
  },
  "JwtSettings": {
    "SecretKey": "interacthub-super-secret-key-minimum-32-chars!",
    "Issuer": "InteractHub",
    "Audience": "InteractHub",
    "ExpirationInMinutes": 60
  },
  "AzureBlobStorage": {
    "ConnectionString": "",
    "ContainerName": "interacthub-media"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "AllowedHosts": "*",
  "AllowedOrigins": "http://localhost:5173"
}
```

**Note:** PostgreSQL dùng `postgres / interacthub@123`, khớp với `docker-compose.yml` và `.env`

---

## Các Lệnh Thường Dùng

### Docker & Database

```bash
# Bật PostgreSQL
docker compose up -d postgres

# Tắt PostgreSQL
docker compose down

# Xem logs PostgreSQL
docker logs interacthub-postgres

# Xem container đang chạy
docker ps
```

### Build & Run

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build InteractHub.sln

# Chạy API
dotnet run --project InteractHub.API

# Chạy với watch mode (auto reload khi code thay đổi)
dotnet watch --project InteractHub.API
```

### Database Migration

```bash
# Tạo migration mới
dotnet ef migrations add <TênMigration> \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API --output-dir Data/Migrations

# Apply migration
dotnet ef database update \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API

# Xóa migration cuối cùng (chưa apply)
dotnet ef migrations remove \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API
```

### Testing

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với code coverage
dotnet test --collect:"XPlat Code Coverage"

# Chạy tests của 1 project cụ thể
dotnet test InteractHub.Tests/InteractHub.Tests.csproj
```

---

## Thông Tin Cấu Hình

### PostgreSQL (Docker)

- **Host:** `localhost`
- **Port:** `5433`
- **Username:** `postgres`
- **Password:** `interacthub@123`
- **Database:** `interacthubdb`

### JWT Settings

- **Issuer:** `InteractHub`
- **Audience:** `InteractHub`
- **Token Expiration:** 60 phút

### CORS

- **Allowed Origins:** `http://localhost:5173` (Frontend dev server)

---

## Troubleshooting

### Lỗi kết nối PostgreSQL

```bash
# Kiểm tra container có chạy không
docker ps

# Xem logs
docker logs interacthub-postgres

# Restart container
docker-compose restart
```

### Lỗi Migration

```bash
# Xóa database và chạy lại
dotnet ef database drop --project InteractHub.Infrastructure --startup-project InteractHub.API
dotnet ef database update --project InteractHub.Infrastructure --startup-project InteractHub.API
```

### Port đã được sử dụng

Thay đổi port trong `InteractHub.API/Properties/launchSettings.json`

---

## Branch Strategy

| Branch      | Mục đích                          |
| ----------- | --------------------------------- |
| `main`      | Production — chỉ merge từ develop |
| `dev`       | Staging — tích hợp các feature    |
| `feature/*` | Tính năng mới                     |
| `fix/*`     | Bug fix                           |
| `test/*`    | Unit test                         |

**Quy trình:** `feature/xxx` → PR → `dev`  
⚠️ **CHỈ ĐƯỢC PUSH LÊN BRANCH `dev`** — Không push trực tiếp lên `main`

---

## API Endpoints

Xem đầy đủ tại Swagger UI sau khi chạy project: `/docs`

| Controller              | Base Route           |
| ----------------------- | -------------------- |
| AuthController          | `/api/auth`          |
| PostsController         | `/api/posts`         |
| UsersController         | `/api/users`         |
| FriendsController       | `/api/friends`       |
| StoriesController       | `/api/stories`       |
| NotificationsController | `/api/notifications` |

---

## Phân Công Backend Team

### Hoàng Anh

| # | Việc cần làm | Branch | Status |
|---|-------------|--------|--------|
| 0 | JWT Auth (Register, Login, Seed Roles) | `feature/auth-jwt` | Done |
| 1 | Seed Data (Roles, Admin, MusicTracks, Hashtags) | `feature/seed-data` | Done |
| 2 | PostsService + PostsController | `feature/posts-service` | Done |
| 3 | NotificationService + NotificationsController | `feature/notifications-service` | Inprogress |
| 4 | FileUploadService (Azure Blob) | `feature/file-upload-service` | Inprogress |
| 5 | CI/CD + Azure Deployment | `feature/cicd` | Inprogress |


### Thanh Hải

| # | Việc cần làm | Branch | Status |  
|---|-------------|--------|--------|
| 2 | UserService + UsersController | `feature/users-service` | Done |
| 3 | StoriesService + StoriesController | `feature/stories-service` | Done |
| 4 | Unit Test — UserService, StoriesService | `test/users-stories` | Inprogress |

### Gia Hân

| # | Việc cần làm | Branch | Status |
|---|-------------|--------|--------|
| 1 | FriendsService + FriendsController | `feature/friends-service` | Done |
| 2 | PostReportService + PostReportsController | `feature/postreports-service` | Done |
| 3 | SignalR Hub (Notifications real-time) | `feature/signalr-hub` | Inprogress |
| 4 | Unit Test — FriendsService, PostReportService | `test/friends-reports` | Inprogress |
 

---


## Hướng Dẫn Implement — Flow Chuẩn

> `Request → Controller → Service → DbContext → Database`
 
Mỗi tính năng mới đều phải tạo đủ 5 thành phần theo thứ tự sau:

### Bước 1 — Tạo DTOs
 
**Vị trí:** `InteractHub.Core/DTOs/<Feature>/`
 
```
InteractHub.Core/
└── DTOs/
    └── Posts/
        ├── CreatePostRequestDto.cs   ← dữ liệu nhận từ client
        ├── UpdatePostRequestDto.cs   ← dữ liệu nhận từ client
        └── PostResponseDto.cs        ← dữ liệu trả về client
```
 
Quy tắc đặt tên:
- `*RequestDto` → nhận vào từ client
- `*ResponseDto` → trả ra cho client
- **Không bao giờ trả Entity trực tiếp ra ngoài**

---

### Bước 2 — Tạo Interface
 
**Vị trí:** `InteractHub.Core/Interfaces/Services/`
 
```
InteractHub.Core/
└── Interfaces/
    └── Services/
        └── IPostsService.cs
```

---

### Bước 3 — Implement Service
 
**Vị trí:** `InteractHub.Infrastructure/Services/`
 
```
InteractHub.Infrastructure/
└── Services/
    └── PostsService.cs
```

---

### Bước 4 — Đăng ký Dependency Injection
 
**Vị trí:** `InteractHub.API/Program.cs`
 
Tìm phần `// ── Dependency Injection` và thêm vào:
 
```csharp
// InteractHub.API/Program.cs
builder.Services.AddScoped<IPostsService, PostsService>();
// mỗi service mới thêm 1 dòng tương tự
```
 
---

### Bước 5 — Viết Controller
 
**Vị trí:** `InteractHub.API/Controllers/`
 
```
InteractHub.API/
└── Controllers/
    └── PostsController.cs
```

---

### Bước 6 — Viết Unit Test => Để sau viết
 
**Vị trí:** `InteractHub.Tests/Services/`
 
```
InteractHub.Tests/
└── Services/
    └── PostsServiceTests.cs
```

## Tài Liệu Liên Quan

- [`/docs/erd.png`](docs/erd.png) — Sơ đồ ERD database
- [`/docs/api.md`](docs/api.md) — Danh sách API endpoint chi tiết
- [`/docs/deployment.md`](docs/deployment.md) — Hướng dẫn deploy Azure
