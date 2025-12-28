# изпълнение

| ресурс | статус | дата | какво е направено | ресурси за четене |
|------|--------|------|-------------------|-------------------|
| 01.ProjectSetup | 🟩 100% | 10-12-2025 | Създаден ASP.NET Core Web API проект |  |
| - GitHubRepo | Y | 10-12-2025 | Repo в GitHub + initial commit |  |
| - CreateProject | Y | 10-12-2025 | Created: ASP.NET Core Web API project + Class Library |  |
| - SolutionStructure | Y | 10-12-2025 | Controllers / Services / Models | Clean Architecture |
| 02.Database | 🟧 80% | 09-12-2025 | Основна DB конфигурация | EF Core |
| - Database Diagram | Y | 09-12-2025 | Db Sets Structure Created | DrawSQL used |
| ../Common/Entities | Y | 24-12-2025 | Entity models |  |
| ../Common/AppDbContext | Y | 27-12-2025 | DbContext written | DbContext |
| ../Migrations | wip | N |  | EF Core Migrations |
| 03.DTOs | 🟥 0%  | 2025-01-14 | Част от DTO моделите | DTO pattern |
| ../CreateDTOs | wip |  | Request / Response DTO | Mapping |
| ../AutoMapper |  |  |  | AutoMapper |
| 04.Services | 🟧 40% | 2025-01-15 | Business logic | Service Layer |
| ../Common/Services/BaseServices | Y | 28-12-2025 | Services for entities | SOLID; change tracking |
| ../Common/Services | Y | 28-12-2025 | Implementation; inherit from BaseServices.cs |  |
| 05.Controllers | 🟥 0% | - | CRUD endpoints | REST APIs |
| ../CrudControllers | wip | - | GET / POST / PUT / DELETE | REST |
| ../ValidationHook |  |  |  | Model validation |
| 06.Validation | 🟧 10% |  |  | FluentValidation |
| ../FluentValidation |  |  |  | FluentValidation Docs |
| ../ValidationMiddleware |  |  |  | Validation pipeline |
| ../Common/ServiceResult | wip | 28-12-2025 | Personalized Service Result |  
| 07.Security | 🟥 0% |  |  | Authentication |
| ../JWTAuthentication |  |  |  | JWT ASP.NET Core |
| ../AuthorizationRoles |  |  |  | Role-based auth |
| 08.Swagger | 🟥 0%  | | Swagger добавен | Swagger |
| ../SwaggerConfig | Y |  | OpenAPI setup | Swagger UI |
| ../SwaggerAuth |  |  |  | JWT in Swagger |
| 09.Logging | 🟥 0% |  |  | Logging |
| ../Serilog |  |  |  | Serilog |
| 10.Testing | 🟥 0% |  |  | Testing |
| ../UnitTests |  |  |  | xUnit |
| 11.Frontend | 🟥 0% |  |  | React |
| ../ReactSetup |  |  |  | Vite / CRA |
| ../ApiIntegration |  |  |  | Axios |
| ../JwtHandling |  |  |  | Auth flow |
| ../UIComponents |  |  |  | React components |
| 12.Finalization | 🟥 0% |  |  | Cleanup |
| ../Refactor |  |  |  | Clean Code |
| ../Documentation |  |  |  | README |
