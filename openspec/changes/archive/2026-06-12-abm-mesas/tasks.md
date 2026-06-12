# Tasks: ABM Mesas (Negocio)

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | 100-150 |
| 400-line budget risk | Low |
| Chained PRs recommended | No |
| Suggested split | Single PR |
| Delivery strategy | ask-on-risk |
| Chain strategy | stacked-to-main |

Decision needed before apply: No
Chained PRs recommended: No
Chain strategy: stacked-to-main
400-line budget risk: Low

### Suggested Work Units

| Unit | Goal | Likely PR | Notes |
|------|------|-----------|-------|
| 1 | Complete MesaNegocio CRUD & mapping fix | Single PR | Standalone business logic change |

## Phase 1: Foundation (Mapping Fix)

- [x] 1.1 Update `ObtenerMesas()` in `MesaNegocio.cs` to use `LEFT JOIN Usuarios u ON m.MeseroId = u.Id` to load Mesero data and populate `Mesa.Mesero` object. Fix the compiler errors caused by referencing `MeseroId` and `NombreMesero` properties on `Mesa` directly.
- [x] 1.2 Verify that `ObtenerMesas()` handles database NULLs gracefully (e.g. if `MeseroId` is NULL).

## Phase 2: CRUD Core Implementation

- [x] 2.1 Implement `ObtenerMesaPorId(int id)` in `MesaNegocio.cs` using parameterized query and `LEFT JOIN` to return a single `Mesa` object (or `null`).
- [x] 2.2 Implement `AgregarMesa(Mesa nueva)` in `MesaNegocio.cs` using `INSERT INTO Mesas (Numero, Ocupada, MeseroId, Estado)`. Map `nueva.Estado` and `nueva.Mesero?.Id` to parameters.
- [x] 2.3 Implement `ModificarMesa(Mesa mesa)` in `MesaNegocio.cs` using `UPDATE Mesas SET Numero = @Numero, Ocupada = @Ocupada, MeseroId = @MeseroId, Estado = @Estado WHERE Id = @Id`.
- [x] 2.4 Implement `EliminarMesa(int id)` in `MesaNegocio.cs` using `DELETE FROM Mesas WHERE Id = @Id`.

## Phase 3: Compilation Verification

- [x] 3.1 Run `dotnet build` to ensure the project compiles with no warnings/errors in the domain and business logic layers.
