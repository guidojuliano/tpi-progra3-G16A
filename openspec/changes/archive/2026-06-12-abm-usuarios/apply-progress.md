# Apply Progress: ABM Usuarios (Negocio)

**Change**: abm-usuarios
**Mode**: Standard

## Completed Tasks

- [x] 1.1 Update `ObtenerUsuarios()` in `UsuarioNegocio.cs` to fetch active users from database and properly parse the `Rol` enum.
- [x] 1.2 Update `ValidarUsuario(string email, string password)` in `UsuarioNegocio.cs` to check credentials against the database.
- [x] 2.1 Implement `ObtenerUsuarioPorId(int id)`.
- [x] 2.2 Implement `AgregarUsuario(Usuario nuevo)`.
- [x] 2.3 Implement `ModificarUsuario(Usuario usuario)`.
- [x] 2.4 Implement `EliminarUsuario(int id)` using soft delete.
- [x] 3.1 Verify that the project compiles with zero errors (`dotnet build`).

## Files Changed

| File | Action | What Was Done |
|------|--------|---------------|
| `negocio/UsuarioNegocio.cs` | Modified | Cleansed the file and implemented real SQL parameterized queries for CRUD and credentials check. |

## Deviations from Design

None — implementation matches design.

## Issues Found

None.

## Remaining Tasks

None.

## Workload / PR Boundary

- Mode: single PR
- Current work unit: Complete business logic
- Boundary: Starts with dummy implementations; ends with clean compilation and full CRUD + login validation implementations.
- Estimated review budget impact: Low (~150 lines changed).

## Status

7/7 tasks complete. Ready for verify.
