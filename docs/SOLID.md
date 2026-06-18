# SOLID y patrones OOP en MangaT API

Guía de estudio: cada principio con **archivo concreto** del repositorio para repasar y explicar en clase o entrevista.

---

## S — Single Responsibility (Responsabilidad única)

> *Cada clase debe tener una sola razón para cambiar.*

| Clase | Una sola responsabilidad | Archivo |
|-------|--------------------------|---------|
| `MangaController` | Traducir HTTP ↔ servicios | `MangaT.API/Controllers/MangaController.cs` |
| `MangaService` | Orquestar casos de uso | `MangaT.ApplicationCore/Services/MangaService.cs` |
| `MangaRepository` | Persistir en SQL vía EF | `MangaT.Infrastructure/Repositories/MangaRepository.cs` |
| `MangaQuerySpecifications` | Decidir QUÉ filtros aplicar | `MangaT.ApplicationCore/Specifications/MangaQuerySpecifications.cs` |
| `AuthService` | Orquestar login (no generar JWT ni leer config) | `MangaT.ApplicationCore/Services/AuthService.cs` |
| `GlobalExceptionHandler` | Traducir excepciones → ProblemDetails | `MangaT.API/ExceptionHandling/GlobalExceptionHandler.cs` |

**Pregunta de repaso:** ¿Por qué `MangaController` no valida el título del manga?

---

## O — Open/Closed (Abierto/Cerrado)

> *Abierto a extensión, cerrado a modificación.*

### Ejemplo: Specification pattern

Añadir filtro por categoría **sin** modificar `MangaRepository`:

```
GET /api/v1/manga?category=Action
         │
         ▼
MangaByCategorySpecification  ← nueva clase
         │
         ▼
MangaRepository.ApplySpecifications()  ← no cambia al añadir filtros
```

| Archivo | Qué enseña |
|---------|------------|
| `MangaT.Domain/Specifications/ISpecification.cs` | Contrato genérico |
| `MangaByCategorySpecification.cs` | Filtro por categoría |
| `MangaMinRatingSpecification.cs` | Filtro Point >= 7 (Reader) |
| `MangaRepository.cs` → `ApplySpecifications` | Bucle que aplica N filtros |

**Ejercicio:** Crea `MangaByAuthorSpecification` sin tocar el repositorio.

---

## L — Liskov Substitution (Sustitución de Liskov)

> *Las implementaciones deben ser intercambiables sin romper el contrato.*

| Abstracción | Implementación real | Implementación test |
|-------------|---------------------|---------------------|
| `IMangaRepository` | `MangaRepository` (EF + SQL) | `Mock<IMangaRepository>` en unit tests |
| `ICurrentUserContext` | `HttpCurrentUserContext` | Mock que simula rol Reader |
| `IUserCredentialValidator` | `ConfigurationUserCredentialValidator` | Podría ser `LdapCredentialValidator` |

**Pregunta de repaso:** ¿Por qué `MangaServiceTests` no necesita base de datos?

---

## I — Interface Segregation (Segregación de interfaces)

> *Interfaces pequeñas y específicas; no obligar a implementar lo que no se usa.*

| Interfaz | Métodos | Notas |
|----------|---------|-------|
| `IAuthService` | 1 (`LoginAsync`) | Mínima para auth |
| `IUserCredentialValidator` | 1 (`Validate`) | Solo validación |
| `IJwtTokenGenerator` | 1 (`GenerateToken`) | Solo emisión JWT |
| `ICurrentUserContext` | 3 propiedades | Sin métodos de persistencia |
| `IMangaRepository` | 6 | CRUD + Query con specifications |

**Antes (AuthService monolítico):** leía config, validaba usuarios y firmaba JWT en una clase.

**Después (segregado):** tres interfaces, tres responsabilidades.

---

## D — Dependency Inversion (Inversión de dependencias)

> *Depender de abstracciones, no de implementaciones concretas.*

```
┌─────────────────────────────────────────┐
│  ApplicationCore (alto nivel)           │
│  IMangaRepository, ICurrentUserContext  │
│  IUserCredentialValidator, IJwtToken... │
└─────────────────┬───────────────────────┘
                  │ implementa
┌─────────────────▼───────────────────────┐
│  Infrastructure / API (bajo nivel)        │
│  MangaRepository, HttpCurrentUserContext│
│  ConfigurationUserCredentialValidator   │
│  JwtTokenGenerator                      │
└─────────────────────────────────────────┘
```

| Abstracción | Implementación | Registro DI |
|-------------|----------------|-------------|
| `ICurrentUserContext` | `HttpCurrentUserContext` | `Program.cs` |
| `IUserCredentialValidator` | `ConfigurationUserCredentialValidator` | `DependencyInjection.cs` |
| `IJwtTokenGenerator` | `JwtTokenGenerator` | `DependencyInjection.cs` |

**Pregunta de repaso:** ¿Por qué `ApplicationCore` no referencia `Microsoft.AspNetCore.Http`?

---

## Flujo completo: GET con rol Reader

```
1. HTTP GET /api/v1/manga  + JWT Reader
2. HttpCurrentUserContext.IsInRole("Reader") → true
3. MangaQuerySpecifications.BuildForList → [MangaMinRatingSpecification(7)]
4. MangaRepository.QueryAsync → SQL WHERE Point >= 7
5. Respuesta JSON paginada
```

---

## Excepciones por capa

| Capa | Tipo | HTTP | Ejemplo |
|------|------|------|---------|
| Dominio | `MangaNotFoundException` | 404 | Manga id inexistente o oculto para Reader |
| Dominio | `DomainException` | 400 | Título vacío en `Manga.Create` |
| Aplicación | `AppException.Validation` | 422 | FluentValidation falla |
| API | `GlobalExceptionHandler` | — | Mapea todo a ProblemDetails |

---

## Tests que demuestran SOLID

| Test | Principio |
|------|-----------|
| `MangaServiceTests` con mocks | L, D |
| `Delete_Should_Return_Forbidden_For_Reader_Role` | Segregación de permisos |
| `GetAll_Should_Filter_By_Category` | O (Specification) |
| `GetAll_As_Reader_Should_Hide_Low_Rated_Manga` | Regla de negocio + Specification |

---

## Mapa rápido de archivos nuevos

```
MangaT.Domain/
  Specifications/ISpecification.cs

MangaT.ApplicationCore/
  Specifications/
    MangaByCategorySpecification.cs
    MangaMinRatingSpecification.cs
    MangaQuerySpecifications.cs
  Interfaces/
    ICurrentUserContext.cs
    IUserCredentialValidator.cs
    IJwtTokenGenerator.cs

MangaT.Infrastructure/
  Auth/
    ConfigurationUserCredentialValidator.cs
    JwtTokenGenerator.cs

MangaT.API/
  Security/HttpCurrentUserContext.cs
```

---

## Recursos

- [SOLID (Uncle Bob)](https://blog.cleancoder.com/uncle-bob/2020/10/18/Solid-Relevance.html)
- [Specification pattern (Fowler)](https://martinfowler.com/apslp/spec.html)
- [LEARNING_PATH.md](LEARNING_PATH.md) — ejercicios por fase
