# Ruta de aprendizaje

Este proyecto está diseñado para estudiar backend de forma progresiva.

## Fase 1 — Fundamentos (ya implementado)

- [x] Solución multi-proyecto
- [x] Controller + Service + Repository
- [x] EF Core con migraciones y seed
- [x] DTOs y mapeo
- [x] Swagger UI
- [x] README y documentación

**Ejercicio**: Agrega un campo `Publisher` al manga y propágalo por todas las capas.

## Fase 2 — Dominio (ya implementado)

- [x] Proyecto `MangaT.Domain` separado
- [x] Value Object `Rating`
- [x] Entidad rica `Manga` con métodos `Create` y `Update`
- [x] Excepciones de dominio

**Ejercicio**: Crea un value object `MangaTitle` con validación de longitud máxima.

## Fase 3 — API completa (ya implementado)

- [x] CRUD completo
- [x] Paginación (`page`, `pageSize`)
- [x] Validación con FluentValidation
- [x] Result pattern
- [x] Problem Details

**Ejercicio**: Agrega filtro por `category` en `GET /api/v1/manga?category=Action`.

## Fase 4 — Seguridad (ya implementado)

- [x] JWT Bearer
- [x] Roles Admin / Reader
- [x] Endpoint de login
- [x] Protección de endpoints de escritura

**Ejercicio**: Haz que `Reader` solo pueda ver manga con `Point >= 7`.

## Fase 5 — Testing (ya implementado)

- [x] Tests unitarios de dominio
- [x] Tests unitarios de servicio con mocks
- [x] Tests de integración con `WebApplicationFactory`

**Ejercicio**: Agrega un test que verifique que `reader` recibe 403 al intentar DELETE.

## Fase 6 — Próximos pasos (por implementar)

- [ ] CQRS con MediatR (`GetPopularMangaQuery`, `CreateMangaCommand`)
- [ ] Docker Compose
- [ ] GitHub Actions (build + test)
- [ ] User Secrets para JWT y connection string
- [ ] Specification pattern para consultas reutilizables

## Preguntas guía para estudiar

1. ¿Por qué `MangaController` no usa `MangaDbContext` directamente?
2. ¿Cuál es la diferencia entre `Manga` (dominio) y `MangaDto` (API)?
3. ¿Por qué `Rating` es un value object y no un `double` simple?
4. ¿Qué pasa si quitas `[Authorize]` del POST?
5. ¿Por qué los tests unitarios no necesitan base de datos?

## Recursos recomendados

- [Clean Architecture (Robert C. Martin)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Value Objects](https://martinfowler.com/bliki/ValueObject.html)
- [JWT.io](https://jwt.io/) — depurar tokens
- [Documentación EF Core](https://learn.microsoft.com/ef/core/)
