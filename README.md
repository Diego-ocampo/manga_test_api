# MangaT API

Proyecto base educativo en **.NET 10** para aprender arquitectura limpia, separación de capas, DDD, seguridad y testing.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express (o LocalDB) para desarrollo local

## Estructura del proyecto

```
MangaT.API              → Capa de presentación (controllers, auth, middleware)
MangaT.ApplicationCore  → Casos de uso, DTOs, validación, servicios
MangaT.Domain           → Entidades de dominio, value objects, reglas de negocio
MangaT.Infrastructure   → EF Core, repositorios, persistencia
MangaT.Tests.Unit       → Tests del dominio y aplicación
MangaT.Tests.Integration→ Tests del API con WebApplicationFactory
```

## Inicio rápido

```bash
# Restaurar y compilar
dotnet build

# Aplicar migraciones (también se aplican al iniciar la API)
dotnet ef database update --project MangaT.Infrastructure --startup-project MangaT.API

# Ejecutar API
dotnet run --project MangaT.API
```

La API inicia en `http://localhost:5292` y Swagger UI queda disponible en la raíz (`/`) en modo Development.

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
# 1. Obtener token
curl -X POST http://localhost:5292/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'

# 2. Usar token en POST
curl -X POST http://localhost:5292/api/v1/manga \
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
- **Result pattern** para errores esperados
- **JWT + roles** (autenticación vs autorización)
- **Problem Details** para respuestas de error
- **Tests unitarios** e **integración**

## Documentación adicional

- [Arquitectura](docs/ARCHITECTURE.md)
- [Ruta de aprendizaje](docs/LEARNING_PATH.md)

## Próximos pasos sugeridos

- CQRS con MediatR
- Docker Compose (API + SQL Server)
- CI con GitHub Actions
- User Secrets para credenciales locales
