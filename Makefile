DB_URL=mssql://sa:secret@localhost:1433/exemple_dotnet_db
OUTPUT_FOLDER=Models
PROJECT_PATH=Exemple_Dotnet_API/Exemple_Dotnet_API.csproj

POSTGRES_SCAFFOLD_CMD = dotnet ef dbcontext scaffold "Host=localhost;Database=exemple_dotnet_db;Username=root;Password=P@ssword123" Npgsql.EntityFrameworkCore.PostgreSQL -o $(OUTPUT_FOLDER)

postgres:
	docker run --name exemple-pg-container -p 5432:5432 -e POSTGRES_USER=root -e POSTGRES_PASSWORD=P@ssword123 -d postgres

createdb:
	docker exec -it exemple-pg-container createdb --username=root --owner=root exemple_dotnet_db

scaffold:
	cd Entities && \
	$(POSTGRES_SCAFFOLD_CMD)

new_migrations:
	cd Exemple_Dotnet_API && \
	dotnet ef --project ../Entities migrations add $(migrationsName) --context ExempleApiDbContext

migrations_update:
	cd Exemple_Dotnet_API && \
	dotnet ef database update --context ExempleApiDbContext

remove_last_migration:
	cd Exemple_Dotnet_API && \
	dotnet ef --project ../Entities migrations remove --context ExempleApiDbContext

server:
	dotnet watch run --project $(PROJECT_PATH)

.PHONY: network postgres mssql createdb scaffold new_migration migrations_update server