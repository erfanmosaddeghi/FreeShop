FreeShop — Modular Monolith (DDD + CQRS) on .NET 8

Orders module with clean layering (Domain / Application / Infrastructure / Api), CQRS (write/read separation), MediatR pipeline (Validation/Transaction), EF Core with PostgreSQL (primary) and optional SQL Server. Swagger-enabled host.

1) Prerequisites
	•	.NET SDK 8.x
	•	Check: dotnet --version → 8.*
	•	PostgreSQL 14+ (recommended)
	•	Default local dev: Host=localhost;Port=5432;Username=postgres;Password=postgres
	•	(Optional) SQL Server 2019+ for Windows or Docker
	•	Git
	•	(Optional, Mac)**: psql CLI from Postgres.app or Homebrew

If you previously installed dotnet-ef: update it
dotnet tool update -g dotnet-ef

2) Clone & Solution Layout
    git clone <your-repo-url> FreeShop
    cd FreeShop

	    src/
	    Host/FreeShop.Host
	    Modules/
	        Orders/
	        Api
	        Application
	        Domain
	        Infrastructure
	        Migrations.PostgreSql
	        Migrations.SqlServer
	    SharedKernel/Domain

3) App configuration (Host)
Edit src/Host/FreeShop.Host/appsettings.json:
		{
		"Db": { "Provider": "npgsql" },
		"ConnectionStrings": {
			"OrdersNpgsql": "Host=localhost;Port=5432;Database=ordersdb;Username=postgres;Password=postgres",
			"OrdersSqlServer": "Server=localhost;Database=OrdersDb;User Id=sa;Password=Your_strong_password1!;TrustServerCertificate=True;"
			}
		}
    
•	Set "Provider": "npgsql" for PostgreSQL, "sqlserver" for SQL Server.
•	You can override via environment variable: Db__Provider=npgsql

4) Build & Restore
	    dotnet restore
	    dotnet build

5) Entity Framework — Migrations (per provider)

	This project keeps provider-specific migrations in separate projects:
		•	Modules.Orders.Migrations.PostgreSql (Postgres)
		•	Modules.Orders.Migrations.SqlServer (SQL Server)

5.1 Add migrations projects to solution (if not yet added)
		    dotnet sln add src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj
		    dotnet sln add src/Modules/Orders/Migrations.SqlServer/Modules.Orders.Migrations.SqlServer.csproj

5.2 Create/Update database (PostgreSQL)
The migrations project already contains design-time factories for both write (OrdersDbContext) and read (OrdersReadDbContext) sides.

Create DB schema & tables:
    # Write-side schema (orders)
	    dotnet ef database update \
	    --project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
	    --startup-project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
	    --context OrdersDbContext

    # Read-side schema (orders_read)
	    dotnet ef database update \
	    --project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
	    --startup-project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
	    --context OrdersReadDbContext

If you changed the model and need new migrations:

    dotnet ef migrations add <Name> \
    --project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
    --startup-project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
    --context OrdersDbContext \
    --output-dir Migrations/Write

    dotnet ef migrations add <Name>_Read \
    --project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
    --startup-project src/Modules/Orders/Migrations.PostgreSql/Modules.Orders.Migrations.PostgreSql.csproj \
    --context OrdersReadDbContext \
    --output-dir Migrations/Read

6) Run the API (Swagger)
    	dotnet run --project src/Host/FreeShop.Host/FreeShop.Host.csproj

		•	http://localhost:5000/swagger  (Kestrel default may vary; check console output)
