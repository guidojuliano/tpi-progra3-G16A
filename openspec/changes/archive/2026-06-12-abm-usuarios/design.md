# Design: ABM Usuarios (Negocio)

## Technical Approach

We will implement full database operations in `negocio/UsuarioNegocio.cs` using the custom ADO.NET utility `AccesoDatos.cs`. All SQL commands will be parameterized to avoid SQL Injection. Since `Usuarios` are linked to `Mesas` (and future `Pedidos`), deletion will be implemented as a soft delete (`Activo = 0`).

## Architecture Decisions

| Decision | Choice | Alternatives considered | Rationale |
|----------|--------|-------------------------|-----------|
| Deletion Mode | Soft delete: Update `Activo = 0` | Hard delete: `DELETE FROM Usuarios` | Avoids database foreign key constraint conflicts when a waiter is assigned to a table or has placed orders. |
| Enum parsing | `Enum.Parse` to map database string to domain `Rol` enum | Store enum as integer | Storing enum names as strings in DB (`Gerente`, `Mesero`, `Chef`) makes the database tables highly readable and less error-prone during manual inspection. |

## Data Flow

Data flows from the presentation layer (e.g. login form, user management form) to the Business layer, which executes SQL parameterized queries:

    UI (Login/CRUD) ──(Credentials/Usuario)──→ UsuarioNegocio ──→ AccesoDatos ──→ SQL Server
          ▲                                                                         │
          └─────────────────────(Boolean/Usuario List)──────────────────────────────┘

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `negocio/UsuarioNegocio.cs` | Modify | Implement authentication logic and CRUD operations. |

## Interfaces / Contracts

```csharp
namespace negocio
{
    public class UsuarioNegocio
    {
        public List<Usuario> ObtenerUsuarios();
        public Usuario ObtenerUsuarioPorId(int id);
        public void AgregarUsuario(Usuario nuevo);
        public void ModificarUsuario(Usuario usuario);
        public void EliminarUsuario(int id); // Performs soft delete (Activo = 0)
        public bool ValidarUsuario(string email, string password);
    }
}
```

### SQL Queries mapping:

- **SELECT (All Active)**:
  `SELECT Id, Nombre, Apellido, Email, Password, Rol, Activo FROM Usuarios WHERE Activo = 1`
- **SELECT (By Id)**:
  `SELECT Id, Nombre, Apellido, Email, Password, Rol, Activo FROM Usuarios WHERE Id = @Id`
- **INSERT**:
  `INSERT INTO Usuarios (Nombre, Apellido, Email, Password, Rol, Activo) VALUES (@Nombre, @Apellido, @Email, @Password, @Rol, 1)`
- **UPDATE**:
  `UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Password = @Password, Rol = @Rol, Activo = @Activo WHERE Id = @Id`
- **DELETE (Soft)**:
  `UPDATE Usuarios SET Activo = 0 WHERE Id = @Id`
- **VALIDATION (Login)**:
  `SELECT Id FROM Usuarios WHERE Email = @Email AND Password = @Password AND Activo = 1`

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Unit / Logic | Authentication checks & CRUD operations | Manual code verification via compilation; testing methods call flow. |

## Migration / Rollout

No migration required. The database schema has already been updated in `RestoBarDb.sql`.

## Open Questions

- None.
