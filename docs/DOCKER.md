# Docker — Referencia rápida

> Guía completa paso a paso: **[MANUAL-DOCKER.md](MANUAL-DOCKER.md)**

## Inicio en 3 pasos

```powershell
Copy-Item .env.example .env    # opcional, primera vez
docker compose up --build
# Abrir documentación interactiva (.NET 10, solo Development)
# Swagger UI: http://localhost:8080/swagger
# Scalar UI:   http://localhost:8080/scalar/v1
```

## URLs

| Servicio | URL |
|----------|-----|
| Swagger UI | http://localhost:8080/swagger |
| Scalar UI | http://localhost:8080/scalar/v1 |
| OpenAPI 3.1 (nativo) | http://localhost:8080/openapi/v1.json |
| Swagger JSON | http://localhost:8080/swagger/v1/swagger.json |
| Health | http://localhost:8080/health |
| SQL Server | localhost:1433 |

## Comandos frecuentes

```powershell
docker compose up --build -d   # background
docker compose logs -f api     # logs
docker compose down            # parar
docker compose down -v         # parar + borrar BD
docker compose up --build -d api  # rebuild tras cambios
```

## Configuración

Edita `.env` (copia de `.env.example`). Variables principales:

- `API_PORT` — puerto de la API (default 8080)
- `MSSQL_SA_PASSWORD` — contraseña SQL Server
- `JWT_KEY` — clave JWT
- `ASPNETCORE_ENVIRONMENT` — `Development` para Swagger
