# Tasks: ABM Insumos (Negocio)

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
| 1 | Complete InsumoNegocio CRUD | Single PR | Standalone business logic change |

## Phase 1: CRUD & Query Implementation

- [x] 1.1 Rename class to `InsumoNegocio` and namespace it to `negocio` in `InsumoNegocio.cs`.
- [x] 1.2 Implement `ObtenerInsumos()` to query database and map the list of active insumos, parsing `TipoInsumo` enum.
- [x] 1.3 Implement `ObtenerInsumoPorId(int id)`.
- [x] 1.4 Implement `AgregarInsumo(Insumo nuevo)`.
- [x] 1.5 Implement `ModificarInsumo(Insumo insumo)`.
- [x] 1.6 Implement `EliminarInsumo(int id)` (soft delete: `Activo = 0`).

## Phase 2: Compilation Verification

- [x] 2.1 Run `dotnet build` to ensure the project compiles with no warnings/errors in the domain and business logic layers.
