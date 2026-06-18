# .NET 10 en MangaT API — Novedades adoptadas

Este documento muestra **qué novedades de .NET 10 usa el proyecto hoy** y dónde están en el código.

---

## Resumen ejecutivo

| Área | Tecnología .NET 10 | Estado |
|------|-------------------|--------|
| Runtime | `net10.0` LTS | ✅ Activo |
| OpenAPI | 3.1 nativo (`Microsoft.AspNetCore.OpenApi`) | ✅ Activo |
| UI moderna | Scalar (`Scalar.AspNetCore`) | ✅ Activo |
| UI clásica | Swagger UI (`Swashbuckle.AspNetCore` 10) | ✅ Activo |
| Errores | RFC 9457 Problem Details + `IExceptionHandler` | ✅ Activo |
| Auth | JWT Bearer | ✅ Activo |
| Health | EF Core health check | ✅ Activo |
| Docker | Imágenes `dotnet/sdk:10.0` y `aspnet:10.0` | ✅ Activo |

---

## 1. OpenAPI 3.1 nativo

**En .NET 10** Microsoft incluye generación nativa con soporte **OpenAPI 3.1**. Es el documento que consume Scalar.

```csharp
// Program.cs
builder.Services.AddOpenApi("v1", options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<AuthorizeOperationTransformer>();
});

app.MapOpenApi(); // expone /openapi/v1.json
```

**Documento OpenAPI nativo:** `http://localhost:8080/openapi/v1.json`

**Transformers personalizados:** `MangaT.API/OpenApi/OpenApiTransformers.cs`
- `BearerSecuritySchemeTransformer` — declara esquema JWT en el contrato OpenAPI
- `AuthorizeOperationTransformer` — marca endpoints con `[Authorize]` como protegidos

---

## 2. Swagger UI (estándar educativo e industria)

**Swashbuckle** sigue siendo el estándar más conocido para documentar APIs en .NET. Lo mantenemos **junto a** OpenAPI nativo y Scalar para fines educativos.

```csharp
// MangaT.API/Swagger/SwaggerConfiguration.cs
builder.Services.AddMangaTSwagger();
app.UseMangaTSwagger();
```

| Recurso | URL (Development) |
|---------|-------------------|
| Swagger UI | `http://localhost:8080/swagger` |
| JSON de Swagger | `http://localhost:8080/swagger/v1/swagger.json` |

Incluye botón **Authorize** para pegar el JWT obtenido en `POST /api/v1/auth/login`.

---

## 3. Scalar UI (UI moderna .NET 10)

**Scalar** es la UI moderna recomendada para .NET 10. Lee el OpenAPI nativo y ofrece:
- Interfaz interactiva para probar endpoints
- Soporte Bearer JWT (botón Authorize)
- Generación de código en múltiples lenguajes

```csharp
app.MapScalarApiReference(options =>
{
    options.WithTitle("MangaT API")
           .WithOpenApiRoutePattern("/openapi/{documentName}.json");
});
```

**URL en Development:** `http://localhost:8080/scalar/v1`

---

## 4. IExceptionHandler + Problem Details (RFC 9457)

Patrón recomendado desde .NET 8, refinado en .NET 10:

```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
app.UseExceptionHandler();
```

**Archivo:** `MangaT.API/ExceptionHandling/GlobalExceptionHandler.cs`

Convierte excepciones a respuestas estándar con `traceId` y `errorCode`.

---

## 5. Primary constructors y C# moderno

Usado en todo el proyecto:

```csharp
public class MangaController(IMangaService mangaService) : ControllerBase
public class MangaService(IMangaRepository repo, ...) : IMangaService
public class GlobalExceptionHandler(ILogger<...> logger, IHostEnvironment env) : IExceptionHandler
```

---

## 6. Health checks con EF Core

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MangaDbContext>("database");

app.MapHealthChecks("/health");
```

Verifica que la API puede conectar a la base de datos.

---

## 7. Docker con imágenes .NET 10

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
```

- Linux base: **Ubuntu 24.04** (default en .NET 10)
- Ver [MANUAL-DOCKER.md](MANUAL-DOCKER.md)

---

## 8. Paquetes alineados a 10.0.8

| Paquete | Versión |
|---------|---------|
| `Microsoft.AspNetCore.OpenApi` | 10.0.8 |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.8 |
| `Microsoft.EntityFrameworkCore.*` | 10.0.8 |
| `Swashbuckle.AspNetCore` | 10.0.1 |
| `Microsoft.OpenApi` | 2.3.0 |
| `Scalar.AspNetCore` | 2.0.36 |

---

## Mapa de archivos clave

```
MangaT.API/
├── Program.cs                          → OpenAPI 3.1, Swagger, Scalar, JWT, Problem Details
├── OpenApi/OpenApiTransformers.cs      → Transformers .NET 10 para JWT en OpenAPI nativo
├── Swagger/SwaggerConfiguration.cs     → Swagger UI clásico (Swashbuckle 10)
├── ExceptionHandling/
│   └── GlobalExceptionHandler.cs       → IExceptionHandler
└── Controllers/                        → [Authorize] detectado por OpenAPI transformer

Dockerfile                              → dotnet/aspnet:10.0
docker-compose.yml                      → API + SQL Server 2022
```

---

## Cómo verificar que usas .NET 10

```powershell
# SDK instalado
dotnet --version
# Esperado: 10.0.x

# Target framework del proyecto
dotnet list MangaT.API package
# Microsoft.AspNetCore.OpenApi 10.0.8

# OpenAPI 3.1 nativo
curl http://localhost:8080/openapi/v1.json
# Busca "openapi": "3.1.0"

# Swagger UI (clásico)
# Abrir http://localhost:8080/swagger

# Scalar UI (moderno)
# Abrir http://localhost:8080/scalar/v1
```

---

## Próximas novedades .NET 10 por adoptar

| Novedad | Descripción |
|---------|-------------|
| Imagen Chiseled | `aspnet:10.0-noble-chiseled` para producción distroless |
| OpenTelemetry | Métricas y trazas distribuidas |
| Native AOT | Compilación sin runtime (avanzado) |
| Passkeys / WebAuthn | Auth sin contraseña con Identity |
| Minimal APIs + validación | `AddValidation()` si migramos endpoints |

---

## Referencias oficiales

- [What's new in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
- [What's new in ASP.NET Core 10](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0)
- [OpenAPI in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview)
- [Scalar ASP.NET Core integration](https://scalar.com/products/api-references/integrations/aspnetcore/integration)
