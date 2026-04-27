# Flow Run

## Mục tiêu

Tài liệu này hướng dẫn thành viên trong team chạy backend ở local theo các bước ngắn nhất có thể.

## 1. Chuẩn bị

- Cài `.NET 8 SDK`.
- Cài `Docker Desktop` nếu muốn chạy SQL Server local.
- Mở workspace của project.

## 2. Tạo file cấu hình local

Tạo file `InteractHub.API/appsettings.Development.json` nếu máy bạn chưa có.

Mẫu tối thiểu:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:interacthub-mssql-123.database.windows.net,1433;Initial Catalog=InteractHubDb;Persist Security Info=False;User ID=malhh;Password=interacthub123@;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "JwtSettings": {
    "SecretKey": "interacthub-super-secret-key-minimum-32-chars!",
    "Issuer": "InteractHub",
    "Audience": "InteractHub",
    "ExpirationInMinutes": 60
  },
  "AllowedOrigins": "http://localhost:5173"
}
```

Ghi chú:
- Project hiện đã deploy backend lên Azure App Service.
- Database production đang chạy trên Azure SQL Database.
- Nếu bạn muốn dùng database local thay vì Azure SQL, hãy bật block `sqlserver` trong `docker-compose.yml` và thay connection string tương ứng.

## 3. Chạy project local

1. Khôi phục package.

```bash
dotnet restore
```

2. Cập nhật database.

```bash
dotnet ef database update --project InteractHub.Infrastructure --startup-project InteractHub.API
```

3. Chạy API.

```bash
dotnet run --project InteractHub.API
```

4. Mở Swagger local.

```bash
https://localhost:5250/docs
```

## 4. Nếu muốn chạy SQL Server local bằng Docker

1. Mở `docker-compose.yml`.
2. Bỏ comment block `sqlserver`.
3. Thiết lập `MSSQL_SA_PASSWORD` trong file `.env`.
4. Chạy:

```bash
docker compose up -d sqlserver
```

## 5. URL backend deploy

Môi trường production hiện truy cập ở:

```text
https://interacthub-api-123.azurewebsites.net/docs
```

Nếu domain App Service thay đổi, dùng domain mới nhưng giữ path `/docs`.
