# SQL Server Migration Skeleton

This project was switched from PostgreSQL provider to SQL Server provider.

Use these commands to create SQL Server migrations from the current model:

```bash
dotnet restore

dotnet ef migrations add InitialSqlServer \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API \
  --output-dir Data/Migrations

dotnet ef database update \
  --project InteractHub.Infrastructure \
  --startup-project InteractHub.API
```

If old PostgreSQL migrations still exist, remove them first before creating `InitialSqlServer`.

```bash
rm -f InteractHub.Infrastructure/Data/Migrations/*.cs
```

For Azure SQL deployment, use connection string via App Service settings:

- `ConnectionStrings__DefaultConnection`
- `JwtSettings__SecretKey`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`
- `AllowedOrigins`
