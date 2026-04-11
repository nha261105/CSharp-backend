## Railway Deploy Checklist (Backend + PostgreSQL)

### 1) Create services

- Create one PostgreSQL service on Railway.
- Create one backend service from this repository.

### 2) Required backend environment variables

Set these on the backend service:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__DefaultConnection=Host=<RAILWAY_DB_HOST>;Port=<RAILWAY_DB_PORT>;Database=<RAILWAY_DB_NAME>;Username=<RAILWAY_DB_USER>;Password=<RAILWAY_DB_PASSWORD>;SSL Mode=Require;Trust Server Certificate=true`
- `JwtSettings__SecretKey=<minimum-32-chars-secret>`
- `JwtSettings__Issuer=InteractHub`
- `JwtSettings__Audience=InteractHub`
- `JwtSettings__ExpirationInMinutes=60`
- `AllowedOrigins=https://<your-frontend-domain>`

`AllowedOrigins` supports comma-separated values if multiple frontend domains are needed.

### 3) Build and start

- Railway can use the root `Dockerfile` directly.
- App listens on port `8080` in container and is compatible with Railway routing.

### 4) Database migration behavior

- App runs `Database.Migrate()` on startup, then seeds role/admin data.
- Ensure PostgreSQL variables are correct before first deployment.

### 5) Post-deploy smoke checks

- Open `GET /swagger` in non-production local env only.
- Test at least:
	- `POST /api/auth/register`
	- `POST /api/auth/login`
	- One authorized endpoint with returned JWT.

