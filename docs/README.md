# Documentación — MangaT API

Índice de manuales y guías del proyecto.

## Manuales

| Documento | Descripción |
|-----------|-------------|
| [**MANUAL-DOCKER.md**](MANUAL-DOCKER.md) | Guía completa — Docker: instalación, levantamiento, pruebas y configuración |
| [DOCKER.md](DOCKER.md) | Referencia rápida de comandos Docker |
| [**DOTNET10.md**](DOTNET10.md) | **Novedades .NET 10 adoptadas** — OpenAPI 3.1, Swagger, Scalar, IExceptionHandler, Docker |

## Arquitectura y aprendizaje

| Documento | Descripción |
|-----------|-------------|
| [ARCHITECTURE.md](ARCHITECTURE.md) | Capas, flujo de peticiones, seguridad y errores |
| [**SOLID.md**](SOLID.md) | **Principios SOLID** con ejemplos en código, specifications y auth DIP |
| [**LOGGING.md**](LOGGING.md) | **Serilog** — logging estructurado, niveles, sinks y correlación con TraceId |
| [LEARNING_PATH.md](LEARNING_PATH.md) | Ruta de aprendizaje por fases con ejercicios |

## Stack actual (.NET 10)

| Componente | Tecnología |
|------------|------------|
| Runtime | .NET 10 LTS (`net10.0`) |
| Documentación API | OpenAPI 3.1 nativo + **Swagger UI** + **Scalar UI** |
| Errores | RFC 9457 Problem Details |
| Auth | JWT Bearer |
| BD | EF Core 10 + SQL Server |
| Logging | **Serilog** (consola + archivo rotativo) |
| Contenedores | Docker Compose |

## Inicio rápido

```powershell
# Docker (recomendado)
Copy-Item .env.example .env
docker compose up --build

# Swagger UI: http://localhost:8080/swagger
# Scalar UI:  http://localhost:8080/scalar/v1
# OpenAPI:    http://localhost:8080/openapi/v1.json
```

Ver el [Manual Docker completo](MANUAL-DOCKER.md) para instrucciones detalladas.
