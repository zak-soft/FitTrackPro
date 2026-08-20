# =========================
# 1. Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copier les fichiers projet
COPY backend/FitTrackPro.Api/FitTrackPro.Api.csproj backend/FitTrackPro.Api/
COPY backend/FitTrackPro.Application/FitTrackPro.Application.csproj backend/FitTrackPro.Application/
COPY backend/FitTrackPro.Domain/FitTrackPro.Domain.csproj backend/FitTrackPro.Domain/
COPY backend/FitTrackPro.Infrastructure/FitTrackPro.Infrastructure.csproj backend/FitTrackPro.Infrastructure/

# Restaurer les dépendances
RUN dotnet restore backend/FitTrackPro.Api/FitTrackPro.Api.csproj

# Copier le reste du code
COPY backend/ backend/

# Compiler et publier
RUN dotnet publish backend/FitTrackPro.Api/FitTrackPro.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# 2. Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Copier uniquement l'application publiée
COPY --from=build /app/publish .

# Port utilisé par l'application
EXPOSE 8080

# Lancer l'API
ENTRYPOINT ["dotnet", "FitTrackPro.Api.dll"]