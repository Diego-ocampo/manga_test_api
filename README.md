# MangaT API

Proyecto base educativo en **.NET 10** para aprender arquitectura limpia, separación de capas, DDD, seguridad y testing.

## Requisitos

**Opción A — Local**
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express (o LocalDB) para desarrollo local

**Opción B — Docker (recomendado)**
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Estructura del proyecto

```
MangaT.API              → Capa de presentación (controllers, auth, middleware)
MangaT.ApplicationCore  → Casos de uso, DTOs, validación, servicios
MangaT.Domain           → Entidades de dominio, value objects, reglas de negocio
MangaT.Infrastructure  → EF Core, repositorios, persistencia
MangaT.Tests.Unit       → Tests del dominio y aplicación
MangaT.Tests.Integration→ Tests del API con WebApplicationFactory
```

## Inicio rápido con Docker

```bash
docker compose up --build
```

| Servicio | URL |
|----------|-----|
| API + Scalar UI | http://localhost:8080/scalar/v1 |
| OpenAPI 3.1 JSON | http://localhost:8080/openapi/v1.json |
| Health check | http://localhost:8080/health |
| SQL Server | localhost:1433 |

Ver [docs/MANUAL-DOCKER.md](docs/MANUAL-DOCKER.md) para la guía completa.

## Inicio rápido local

```bash
dotnet build
dotnet ef database update --project MangaT.Infrastructure --startup-project MangaT.API
dotnet run --project MangaT.API
```

La API inicia en `http://localhost:5292`. En Development, Scalar UI está en `/scalar/v1` y el documento OpenAPI en `/openapi/v1.json`.

## Endpoints principales

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| POST | `/api/v1/auth/login` | No | Obtener token JWT |
| GET | `/api/v1/manga` | No | Listado paginado |
| GET | `/api/v1/manga/popular` | No | Manga populares por calificación |
| GET | `/api/v1/manga/{id}` | No | Detalle por id |
| POST | `/api/v1/manga` | Admin | Crear manga |
| PUT | `/api/v1/manga/{id}` | Admin | Actualizar manga |
| DELETE | `/api/v1/manga/{id}` | Admin | Eliminar manga |
| GET | `/health` | No | Health check con estado de BD |

## Usuarios de demostración

| Usuario | Contraseña | Rol |
|---------|------------|-----|
| `admin` | `Admin123!` | Admin (CRUD) |
| `reader` | `Reader123!` | Reader (solo lectura autenticada) |

Los endpoints GET son públicos. Las operaciones de escritura requieren rol **Admin**.

### Ejemplo: login y crear manga

```bash
# 1. Obtener token (Docker usa puerto 8080)
curl -X POST http://localhost:8080/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'

# 2. Usar token en POST
curl -X POST http://localhost:8080/api/v1/manga \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"title":"Chainsaw Man","author":"Tatsuki Fujimoto","point":9.1,"volumeCount":15}'
```

## Tests

```bash
dotnet test
```

## Conceptos que enseña este proyecto

- **Separación de capas** e inversión de dependencias
- **DDD táctico**: entidad rica (`Manga`), value object (`Rating`), excepciones de dominio
- **Patrón Repository** y servicios de aplicación
- **Validación** con FluentValidation
- **Excepciones controladas** + Problem Details (RFC 9457)
- **JWT + roles** (autenticación vs autorización)
- **OpenAPI 3.1 nativo** + Scalar UI (stack .NET 10)
- **Tests unitarios** e **integración**

## Documentación adicional

- [Índice de documentación](docs/README.md)
- [**Manual Docker (guía completa)**](docs/MANUAL-DOCKER.md)
- [Docker — referencia rápida](docs/DOCKER.md)
- [Arquitectura](docs/ARCHITECTURE.md)
- [Ruta de aprendizaje](docs/LEARNING_PATH.md)
- [Novedades .NET 10](docs/DOTNET10.md)

## Próximos pasos sugeridos

- CQRS con MediatR
- CI con GitHub Actions
- User Secrets para credenciales locales
- OpenTelemetry para métricas
