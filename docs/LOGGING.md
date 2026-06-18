# Logging con Serilog — MangaT API

Guía educativa sobre cómo está implementado el logging profesional en este proyecto.

## ¿Por qué Serilog y no solo `ILogger`?

ASP.NET Core incluye abstracciones de logging (`ILogger<T>`), pero **Serilog** es el estándar de facto en .NET para APIs en producción porque aporta:

| Característica | Beneficio educativo / profesional |
|----------------|-----------------------------------|
| **Logging estructurado** | Propiedades `{MangaId}`, `{TraceId}` buscables en herramientas (Datadog, Seq, ELK) |
| **Sinks configurables** | Consola + archivo sin cambiar código, solo `appsettings.json` |
| **Enriquecimiento** | Máquina, entorno, contexto HTTP automáticos |
| **Integración ASP.NET Core** | Una línea por petición con `UseSerilogRequestLogging` |

> **Alternativas válidas:** `ILogger` nativo + OpenTelemetry es la tendencia cloud-native. Para aprender arquitectura limpia, Serilog + `ILogger<T>` es la mejor combinación: el código usa la abstracción y Serilog es el proveedor.

---

## Arquitectura en este proyecto

```
┌─────────────────────────────────────────────────────────┐
│  Program.cs                                             │
│  • Bootstrap logger (errores de arranque)               │
│  • AddMangaTSerilog() ← appsettings.json                │
│  • UseMangaTRequestLogging() ← 1 línea por HTTP         │
└─────────────────────────────────────────────────────────┘
                          │
          ┌───────────────┼───────────────┐
          ▼               ▼               ▼
   GlobalExceptionHandler  MangaService   AuthService
   (4xx Warning, 5xx Error) (auditoría CRUD) (login ok/fallo)
                          │
                          ▼
              Consola + ../logs/mangat-YYYYMMDD.log (raíz del repo, fuera de MangaT.API)
```

### Archivos clave

| Archivo | Responsabilidad |
|---------|-----------------|
| `MangaT.API/Logging/SerilogExtensions.cs` | Configuración centralizada |
| `MangaT.API/appsettings.json` | Sinks, niveles y enrichers |
| `MangaT.API/appsettings.Development.json` | Más detalle (`MangaT` en Debug) |
| `MangaT.API/appsettings.Testing.json` | Silencia logs en tests |

---

## Configuración (`appsettings.json`)

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "WriteTo": [
    { "Name": "Console", "Args": { ... } },
    { "Name": "File", "Args": { "path": "../logs/mangat-.log", "rollingInterval": "Day" } }
  ]
}
```

### Niveles (de menor a mayor severidad)

| Nivel | Cuándo usarlo en MangaT |
|-------|-------------------------|
| **Debug** | Detalle de desarrollo (consultas EF en Dev) |
| **Information** | Flujo normal: manga creado, login OK, migración OK |
| **Warning** | Errores 4xx, login fallido, reintentos de migración |
| **Error** | Errores 5xx no controlados |
| **Fatal** | La aplicación no puede continuar |

---

## Logging estructurado (patrón correcto)

```csharp
// ✅ BIEN: propiedades con nombre (buscables)
logger.LogInformation(
    "Manga creado {MangaId} — {Title} por {Author}",
    manga.Id, manga.Title, manga.Author);

// ❌ MAL: interpolación de strings (pierde estructura)
logger.LogInformation($"Manga creado {manga.Id}");
```

**Reglas de seguridad:**
- Nunca registrar contraseñas, tokens JWT completos ni connection strings.
- En login fallido solo registrar `{Username}`, no `{Password}`.

---

## Correlación con errores HTTP

Cada respuesta ProblemDetails incluye `traceId`. El middleware de request logging escribe el mismo valor:

```
HTTP GET /api/v1/manga/99 → 404 en 12.3456 ms
  TraceId: 0HN7...
```

Si un usuario reporta un error, busca ese `TraceId` en los logs.

---

## Docker

Los logs de archivo se guardan en **`../logs/`** (raíz del repositorio) al ejecutar en local con `dotnet run`. Así la carpeta no aparece dentro del proyecto en Visual Studio. `.gitignore` ya excluye `logs/`.

En Docker el working directory es `/app`; `docker-compose.yml` sobreescribe la ruta a `logs/mangat-.log` y monta el volumen:

```yaml
volumes:
  - mangat-logs:/app/logs
```

Ver logs en tiempo real:

```powershell
docker compose logs -f api
```

---

## Variables de entorno (sin recompilar)

Serilog lee la sección `Serilog` desde configuración. Puedes sobreescribir niveles:

```env
Serilog__MinimumLevel__Default=Debug
Serilog__MinimumLevel__Override__MangaT=Debug
```

---

## Ejercicios de aprendizaje

1. **Observa el arranque:** ejecuta la API y localiza `"Iniciando MangaT API"` y `"Migraciones de base de datos aplicadas"`.
2. **Prueba un 404:** `GET /api/v1/manga/9999` → busca Warning con `NOT_FOUND` y el `TraceId`.
3. **Prueba login:** POST `/api/v1/auth/login` con credenciales incorrectas → Warning sin contraseña.
4. **Crea un manga** (con token admin) → Information con `{MangaId}`.
5. **Abre `../logs/mangat-*.log`** (raíz del repo) y compara con la consola.
6. **Cambia el nivel** en `appsettings.Development.json` a `"MangaT": "Debug"` y reinicia.

---

## Próximos pasos (opcional)

- [ ] Sink **Seq** (`Serilog.Sinks.Seq`) para UI de logs en desarrollo
- [ ] **OpenTelemetry** para trazas distribuidas en producción cloud
- [ ] Filtro de datos sensibles con `Destructure.ByTransforming<T>()`
