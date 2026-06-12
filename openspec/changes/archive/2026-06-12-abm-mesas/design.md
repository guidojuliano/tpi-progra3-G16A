# Design: ABM Mesas (Negocio)

## Technical Approach

We will implement the CRUD operations inside `negocio/MesaNegocio.cs` using the helper class `AccesoDatos.cs` for ADO.NET operations. 
We will also fix the compilation errors in `ObtenerMesas()` by properly joining the `Mesas` table with the `Usuarios` table to populate the nested `Usuario Mesero` property instead of relying on non-existent properties like `MeseroId` or `NombreMesero` directly on the `Mesa` class.

## Architecture Decisions

| Decision | Choice | Alternatives considered | Rationale |
|----------|--------|-------------------------|-----------|
| Mapping of Mesero | Fetch full Mesero details via LEFT JOIN and instantiate `dominio.Usuario` | Expose `MeseroId` integer directly on `Mesa` | Keeps the domain model clean. A mesa HAS a mesero (relationship), rather than just storing a foreign key ID. |
| DB Access style | Direct SQL text commands with ADO.NET parameters via `AccesoDatos` | ORM (Entity Framework) or Stored Procedures | The project already uses a custom `AccesoDatos` class wrapper for raw SQL. We follow the established project pattern. |

## Data Flow

Data flows from the presentation layer (when instantiated/called) down to the Database:

    UI (Caller) ──(Mesa object)──→ MesaNegocio ──(SQL + Params)──→ AccesoDatos ──→ SQL Server
        ▲                                                                            │
        └──────────(Mesa / List<Mesa>)───────────────────────────────────────────────┘

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `negocio/MesaNegocio.cs` | Modify | Fix mapping compilation issues and add CRUD methods (`AgregarMesa`, `ModificarMesa`, `EliminarMesa`, `ObtenerMesaPorId`). |

## Interfaces / Contracts

```csharp
namespace negocio
{
    public class MesaNegocio
    {
        public List<Mesa> ObtenerMesas();
        public Mesa ObtenerMesaPorId(int id);
        public void AgregarMesa(Mesa nueva);
        public void ModificarMesa(Mesa mesa);
        public void EliminarMesa(int id);
    }
}
```

### SQL Queries mapping:

- **SELECT (All)**:
  `SELECT m.Id, m.Numero, m.Ocupada, m.MeseroId, m.Estado, u.Nombre, u.Apellido FROM Mesas m LEFT JOIN Usuarios u ON m.MeseroId = u.Id`
- **INSERT**:
  `INSERT INTO Mesas (Numero, Ocupada, MeseroId, Estado) VALUES (@Numero, @Ocupada, @MeseroId, @Estado)`
- **UPDATE**:
  `UPDATE Mesas SET Numero = @Numero, Ocupada = @Ocupada, MeseroId = @MeseroId, Estado = @Estado WHERE Id = @Id`
- **DELETE**:
  `DELETE FROM Mesas WHERE Id = @Id`

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Unit / Logic | CRUD methods execution | Manual verification via debug/adhoc tests or manual execution since there is no test runner configured. |

## Migration / Rollout

No migration required. The database schema has already been updated in `RestoBarDb.sql`.

## Open Questions

- None.
