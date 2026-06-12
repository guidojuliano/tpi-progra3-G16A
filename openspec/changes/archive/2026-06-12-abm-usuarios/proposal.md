# Proposal: ABM Usuarios (Negocio)

## Intent

Implement CRUD logic for the `Usuarios` entity in the Business Logic layer (`negocio/UsuarioNegocio.cs`) and implement login validation matching credentials against the database.

## Scope

### In Scope
- Implement `AgregarUsuario`, `ModificarUsuario`, and `EliminarUsuario` (using soft delete) in `UsuarioNegocio.cs`.
- Implement `ObtenerUsuarios` (retrieving all active users) and `ObtenerUsuarioPorId`.
- Fix and implement `ValidarUsuario(string email, string password)` to check credentials in the database.

### Out of Scope
- Modifying UI components or presentation files.
- Advanced security features (e.g., password hashing/salting), keeping plain-text storage as established by the database seeds.

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- None

## Approach

- Use soft delete (`Activo = 0`) to prevent referential integrity errors (e.g., when a user is assigned to a mesa).
- Update the parameters of `ValidarUsuario` from `nombreUsuario` to `email` since the schema does not have a username column.
- Execute SQL commands with parameters via `AccesoDatos`.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `negocio/UsuarioNegocio.cs` | Modified | Add database logic for CRUD and authentication. |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Duplicate Email constraint violation | Low | Catch SqlException (2627) and handle duplicate email registration in caller. |

## Rollback Plan

- Revert changes to `UsuarioNegocio.cs` using git checkout:
  `git checkout negocio/UsuarioNegocio.cs`

## Dependencies

- None.

## Success Criteria

- [ ] Project compiles successfully (`dotnet build`).
- [ ] Authentication and CRUD operations are fully implemented in `UsuarioNegocio.cs`.
