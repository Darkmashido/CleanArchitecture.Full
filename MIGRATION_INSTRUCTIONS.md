# Instrucciones para Migración EF Core - Relación Client-Account

## Cambios Realizados

Se ha implementado una relación **One-to-Many** entre `Client` y `Account`:
- Un `Client` (Cliente) puede tener múltiples `Account` (Cuentas)
- Una `Account` siempre pertenece a un `Client`

### Nuevas Entidades y Cambios:

1. **Nueva Entidad: Client**
   - Tabla: `clients`
   - Propiedades: Id, Name, Email, PhoneNumber, DocumentNumber, DocumentType, CreatedAt, LastModifiedAt
   - Índices únicos: Email, DocumentNumber

2. **Actualización: Account**
   - Se agregó FK: `ClientId` (referencia a Client)
   - Relación configurada con Cascade Delete

3. **DTOs Actualizados:**
   - `AccountDto` incluye ahora `ClientId` y `ClientDto?`
   - `ClientDto` creado para representar clientes

4. **Repositorios:**
   - `IClientRepository` creado en Domain
   - `ClientRepository` implementado en Infrastructure

5. **DbSeeder:**
   - Ahora crea 3 clientes primero
   - Luego crea 4 cuentas asociadas (Ada: 2 cuentas, Alan: 1, Grace: 1)

## Crear la Migración

```powershell
# Navega a la carpeta del proyecto Infrastructure
cd src\CleanArchitecture.Full.Infrastructure

# Crear una nueva migración
dotnet ef migrations add AddClientTableAndAccountClientRelationship --project . --startup-project ..\CleanArchitecture.Full.Api

# O si usas Visual Studio Package Manager Console
Add-Migration AddClientTableAndAccountClientRelationship -Project CleanArchitecture.Full.Infrastructure -StartupProject CleanArchitecture.Full.Api
```

## Actualizar la Base de Datos

```powershell
# Aplicar la migración
dotnet ef database update --project src\CleanArchitecture.Full.Infrastructure --startup-project src\CleanArchitecture.Full.Api

# O en Package Manager Console
Update-Database -Project CleanArchitecture.Full.Infrastructure -StartupProject CleanArchitecture.Full.Api
```

## Cambios en la Base de Datos

### Nueva Tabla: clients
```sql
CREATE TABLE clients (
	Id UUID PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL,
	Email NVARCHAR(100) NOT NULL UNIQUE,
	PhoneNumber NVARCHAR(20) NOT NULL,
	DocumentNumber NVARCHAR(20) NOT NULL UNIQUE,
	DocumentType NVARCHAR(10) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	LastModifiedAt DATETIME NULL
);
```

### Tabla Modificada: accounts
```sql
ALTER TABLE accounts
ADD ClientId UUID NOT NULL;

ALTER TABLE accounts
ADD FOREIGN KEY (ClientId) REFERENCES clients(Id) ON DELETE CASCADE;
```

## Registrar el Repositorio en DI Container

Necesitas registrar `ClientRepository` en la configuración de inyección de dependencias en `Infrastructure.cs`:

```csharp
public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql(connectionString));

		services.AddScoped<IAccountRepository, AccountRepository>();
		services.AddScoped<IClientRepository, ClientRepository>();  // ← Agregar esta línea

		return services;
	}
}
```

## Verificación

Una vez aplicada la migración:
1. Las tablas `clients` y `accounts` estarán relacionadas
2. DbSeeder cargará automáticamente 3 clientes y 4 cuentas
3. Los endpoints existentes funcionarán con los DTOs actualizados

## Notas Importantes

- La cascada de eliminación está habilitada: eliminar un cliente eliminará todas sus cuentas
- Email y DocumentNumber son únicos en la tabla clients
- La propiedad `HolderName` en Account se mantiene por compatibilidad con la BD existente
- El mapeo de DTOs incluye la información del cliente cuando está disponible
