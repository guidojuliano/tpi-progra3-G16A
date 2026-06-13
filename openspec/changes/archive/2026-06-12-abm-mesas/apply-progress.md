# Apply Progress: ABM Mesas (Negocio)

**Change**: abm-mesas
**Mode**: Standard

## Completed Tasks

- [x] 1.1 Update `ObtenerMesas()` in `MesaNegocio.cs` to use `LEFT JOIN` and populate nested `Mesa.Mesero` object.
- [x] 1.2 Verify that database NULLs are handled gracefully when loading Mesas.
- [x] 2.1 Implement `ObtenerMesaPorId(int id)`.
- [x] 2.2 Implement `AgregarMesa(Mesa nueva)`.
- [x] 2.3 Implement `ModificarMesa(Mesa mesa)`.
- [x] 2.4 Implement `EliminarMesa(int id)`.
- [x] 3.1 Verify that the project compiles with zero errors (`dotnet build`).

## Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `negocio/MesaNegocio.cs` | Modified | Fixed the incorrect mapping and implemented full CRUD logic using parameterized SQL queries. |

## Deviations from Design

None — implementation matches design.

## Issues Found

None.

## Remaining Tasks

None.

## Workload / PR Boundary

- Mode: single PR
- Current work unit: Complete business logic
- Boundary: Starts with broken compilation and missing methods; ends with clean compilation and full CRUD implementations.
- Estimated review budget impact: Low (~100-150 lines changed).

## Status

6/6 tasks complete. Ready for verify.
