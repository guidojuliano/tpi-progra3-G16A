## Exploration: abm-usuarios

### Current State
- **Domain Entity**: `Usuario.cs` contains properties for `Id`, `Nombre`, `Apellido`, `Email`, `Password`, `Rol` (enum), and `Activo` (bool).
- **Database Table**: `Usuarios` table in `RestoBarDb` has `Id`, `Nombre`, `Apellido`, `Email`, `Password`, `Rol` (stored as string), and `Activo` (bit, default 1).
- **Business Logic**: `UsuarioNegocio.cs` contains dummy methods: `ObtenerUsuarios()` returns an empty list, and `ValidarUsuario()` always returns `true`. There are no write methods (Insert, Update, Delete).

### Affected Areas
- `negocio/UsuarioNegocio.cs` — Replace dummy methods and implement CRUD operations (`AgregarUsuario`, `ModificarUsuario`, `EliminarUsuario` / soft delete).

### Approaches
1. **Hard Delete (`DELETE FROM Usuarios WHERE Id = @Id`)**
   - Pros: Cleans up the database records.
   - Cons: Will fail with a foreign key constraint violation if the user is referenced as a mesero in the `Mesas` table (or in future `Pedidos`).
   - Effort: Low

2. **Soft Delete (`UPDATE Usuarios SET Activo = 0 WHERE Id = @Id`)**
   - Pros: Preserves referential integrity and keeps historic relation data intact for mesas and orders, which is crucial for auditing.
   - Cons: User remains in the database (though filtered out from active dropdowns).
   - Effort: Low

### Recommendation
We recommend **Approach 2 (Soft Delete)**. Since the `Usuarios` table is referenced as a foreign key `MeseroId` in `Mesas`, a hard delete would crash or block if a waiter is assigned to a table. Soft delete using the `Activo` bit column is the professional standard for user management.

### Risks
- **Duplicate Emails**: The `Email` column has a `UNIQUE` constraint. We must handle exceptions gracefully when inserting/updating if the email already exists.
- **Login Validation**: `ValidarUsuario` currently takes `nombreUsuario` (Username), but the `Usuario` class and the database table use `Email`. We should update `ValidarUsuario` to use `email` and `password`.

### Ready for Proposal
Yes. The scope is well defined and risks are identified.
