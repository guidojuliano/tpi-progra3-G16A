# Design: ABM Insumos (Negocio)

## Technical Approach

We will implement complete CRUD database logic in `negocio/InsumoNegocio.cs` using the parameterized helper methods in `AccesoDatos.cs`.

## Architecture Decisions

| Decision | Choice | Alternatives considered | Rationale |
|----------|--------|-------------------------|-----------|
| Deletion Mode | Soft delete (`Activo = 0`) | Hard delete | Retains history of items when referenced by order details in database. |
| Enum parsing | `Enum.Parse` to map string values (`Plato`, `Bebida`) to `TipoInsumo` | Store enum as integer | Improves database readability for reporting and audits. |

## Data Flow

Data flows from the presentation logic down to SQL Server:

    UI (Caller) ──(Insumo object)──→ InsumoNegocio ──→ AccesoDatos ──→ SQL Server
        ▲                                                                │
        └──────────────(Insumo / List<Insumo>)───────────────────────────┘

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `negocio/InsumoNegocio.cs` | Modify | Implement target CRUD operations. |

## Interfaces / Contracts

```csharp
namespace negocio
{
    public class InsumoNegocio
    {
        public List<Insumo> ObtenerInsumos();
        public Insumo ObtenerInsumoPorId(int id);
        public void AgregarInsumo(Insumo nuevo);
        public void ModificarInsumo(Insumo insumo);
        public void EliminarInsumo(int id); // Updates Activo = 0
    }
}
```

### SQL Queries mapping:

- **SELECT (All Active)**:
  `SELECT Id, Nombre, Precio, Stock, Tipo, Activo FROM Insumos WHERE Activo = 1`
- **SELECT (By Id)**:
  `SELECT Id, Nombre, Precio, Stock, Tipo, Activo FROM Insumos WHERE Id = @Id`
- **INSERT**:
  `INSERT INTO Insumos (Nombre, Precio, Stock, Tipo, Activo) VALUES (@Nombre, @Precio, @Stock, @Tipo, 1)`
- **UPDATE**:
  `UPDATE Insumos SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock, Tipo = @Tipo, Activo = @Activo WHERE Id = @Id`
- **DELETE (Soft)**:
  `UPDATE Insumos SET Activo = 0 WHERE Id = @Id`

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Unit / Logic | Insumo CRUD operations | Manual verification and compilation. |

## Migration / Rollout

No migration required. The database schema has already been updated in `RestoBarDb.sql`.

## Open Questions

- None.
