## Exploration: abm-insumos

### Current State
- **Domain Entity**: `Insumo.cs` contains properties for `Id`, `Nombre`, `Precio`, `Stock`, `Tipo` (TipoInsumo enum), and `Activo` (bool).
- **Database Table**: `Insumos` table in `RestoBarDb` has `Id`, `Nombre`, `Precio` (decimal), `Stock` (int), `Tipo` (nvarchar), and `Activo` (bit).
- **Business Logic**: `InsumoNegocio.cs` contains only an empty class placeholder (`Class1`). It needs to be renamed and implemented with standard database CRUD logic.

### Affected Areas
- `negocio/InsumoNegocio.cs` — Rename from `Class1` to `InsumoNegocio` and implement database-driven CRUD operations.

### Approaches
1. **Hard Delete (`DELETE FROM Insumos WHERE Id = @Id`)**
   - Pros: Cleans database records.
   - Cons: Breaks integrity if referenced in future `DetallePedido` tables.
   - Effort: Low

2. **Soft Delete (`UPDATE Insumos SET Activo = 0 WHERE Id = @Id`)**
   - Pros: Safely keeps references for existing order records.
   - Cons: Keeps inactive records in table (filtered at list time).
   - Effort: Low

### Recommendation
We recommend **Approach 2 (Soft Delete)** using the `Activo` column to maintain referential integrity with future order detail structures.

### Risks
- **Price/Stock constraints**: `Precio` and `Stock` must be non-negative. We should implement logic controls to prevent invalid insertions (e.g. Price <= 0).

### Ready for Proposal
Yes.
