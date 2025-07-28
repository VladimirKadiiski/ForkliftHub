# ForkliftHub

ForkliftHub is a web application built with ASP.NET Core MVC (.NET 8) and Entity Framework Core 8. It provides functionality for managing forklift and warehouse equipment listings, including support for categories, brands, engines, mast types, machine models, and more.

---

##  Features

- Admin & User Areas with role-based access
- Product listing with filtering, pagination, and search
- Full CRUD support for all main entities
- Responsive design using Razor Views
- Unit testing with mocking for services
- Error handling with custom 404 and 500 views
- Swagger/OpenAPI support for Web API project

---

##  Technologies

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8
- Microsoft SQL Server (LocalDB)
- Bootstrap 5
- xUnit + Moq
- Swagger / Swashbuckle

---

##  Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB)
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository
2. Update `appsettings.json` with your connection string
3. Apply migrations:
dotnet ef database update
4. Run the application:
dotnet run --project ForkliftHub
5. Test API with Swagger:
https://localhost:{port}/swagger
6. Test the services:
dotnet test
