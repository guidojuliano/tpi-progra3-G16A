# Proposal: ABM Insumos (Negocio)

## Intent

Implement CRUD logic for the `Insumos` entity in the Business Logic layer (`negocio/InsumoNegocio.cs`) and support soft deletion.

## Scope

### In Scope
- Rename the class in `InsumoNegocio.cs` and namespace it to `negocio`.
- Implement CRUD operations (`AgregarInsumo`, `ModificarInsumo`, `EliminarInsumo`).
- Implement queries (`ObtenerInsumos` and `ObtenerInsumoPorId`).

### Out of Scope
- Modifying UI components or presentation files.

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- None

## Approach

- Use soft delete (`Activo = 0`) when deleting an insumo.
- Implement parameterized queries via `AccesoDatos` class.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `negocio/InsumoNegocio.cs` | Modified | Rename placeholder and implement full CRUD operations. |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Negative price or stock | Low | Add validation logic to enforce positive prices and stocks. |

## Rollback Plan

- Revert changes to `InsumoNegocio.cs` using git checkout:
  `git checkout negocio/InsumoNegocio.cs`

## Dependencies

- None.

## Success Criteria

- [ ] Project compiles successfully (`dotnet build`).
- [ ] CRUD operations are fully functional in `InsumoNegocio.cs`.
