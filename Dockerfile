# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY InteractHub.API/*.csproj ./InteractHub.API/
COPY InteractHub.Core/*.csproj ./InteractHub.Core/
COPY InteractHub.Infrastructure/*.csproj ./InteractHub.Infrastructure/
COPY InteractHub.Tests/*.csproj ./InteractHub.Tests/

RUN dotnet restore

COPY . .

WORKDIR /app/InteractHub.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "InteractHub.API.dll"]