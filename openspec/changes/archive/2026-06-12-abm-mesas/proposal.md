# Proposal: ABM Mesas (Negocio)

## Intent

Implement CRUD logic for the `Mesas` entity in the Business Logic layer (`negocio`) and resolve current compilation errors related to domain model mapping.

## Scope

### In Scope
- Implement `AgregarMesa`, `ModificarMesa`, `EliminarMesa`, and `ObtenerMesaPorId` in `MesaNegocio.cs`.
- Fix the compiler errors in `MesaNegocio.cs` by correcting the domain-to-database mapping for `Mesa.cs`.
- Update the SQL queries to retrieve mesero information via `LEFT JOIN` and populate the `Mesero` property.

### Out of Scope
- Modifying UI components (`Mesas.aspx`, `Mesas.aspx.cs`) or any other presentation-layer logic.
- Implementing ABM logic for other entities (e.g., `Usuarios`, `Insumos`).

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- None

## Approach

- Update `MesaNegocio.ObtenerMesas()` to perform a `LEFT JOIN Mesas m LEFT JOIN Usuarios u ON m.MeseroId = u.Id` to load assigned Mesero data.
- Populate the `Mesero` object property on the `Mesa` entity (assign `null` if no mesero is assigned).
- Implement standard CRUD methods using the `AccesoDatos` utility class.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `negocio/MesaNegocio.cs` | Modified | Add CRUD operations and fix compilation issues. |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Unique Table Number constraint violation | Low | Handle SqlException (2627) when inserting/updating duplicate table numbers. |
| Invalid Mesero assignment | Low | Ensure the selected mesero exists in the database and has the correct role. |

## Rollback Plan

- Revert changes to `MesaNegocio.cs` using git checkout:
  `git checkout negocio/MesaNegocio.cs`

## Dependencies

- None (database tables and test data have already been initialized).

## Success Criteria

- [ ] Project compiles successfully (`dotnet build`).
- [ ] Business logic exposes clean CRUD methods for `Mesa`.
