# Tasks: ABM Usuarios (Negocio)

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | 120-170 |
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
| 1 | Complete UsuarioNegocio CRUD & Authentication | Single PR | Standalone business logic change |

## Phase 1: Authentication & Mapping Implementation

- [x] 1.1 Update `ObtenerUsuarios()` in `UsuarioNegocio.cs` to fetch active users from `Usuarios` table and properly parse the `Rol` enum.
- [x] 1.2 Update `ValidarUsuario(string email, string password)` in `UsuarioNegocio.cs` to query the database and verify credentials, returning `true` on match and `false` on mismatch.

## Phase 2: CRUD Core Implementation

- [x] 2.1 Implement `ObtenerUsuarioPorId(int id)` in `UsuarioNegocio.cs` using parameterized query.
- [x] 2.2 Implement `AgregarUsuario(Usuario nuevo)` in `UsuarioNegocio.cs` using `INSERT INTO Usuarios` and mapping domain properties to parameters.
- [x] 2.3 Implement `ModificarUsuario(Usuario usuario)` in `UsuarioNegocio.cs` using `UPDATE Usuarios`.
- [x] 2.4 Implement `EliminarUsuario(int id)` in `UsuarioNegocio.cs` using `UPDATE Usuarios SET Activo = 0 WHERE Id = @Id` (soft delete).

## Phase 3: Compilation Verification

- [x] 3.1 Run `dotnet build` to ensure the project compiles with no warnings/errors in the domain and business logic layers.
