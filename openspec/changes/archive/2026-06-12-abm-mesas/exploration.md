## Exploration: abm-mesas

### Current State
- **Compilation State**: The project is currently broken. `MesaNegocio.cs` refers to `MeseroId` and `NombreMesero` on the `Mesa` domain class, which do not exist.
- **Domain Class**: `Mesa.cs` has `Id`, `Numero`, `Estado` (enum), and `Mesero` (of type `Usuario`).
- **Database Table**: `Mesas` table contains `Id`, `Numero`, `Ocupada` (bit), `MeseroId` (int, foreign key referencing `Usuarios`), and `Estado` (nvarchar).
- **Business Logic**: `MesaNegocio.cs` only has a `ObtenerMesas()` method, which performs a simple query. It needs CRUD methods.
- **UI (Mesas.aspx)**: Only has a read-only GridView listing ID, Number, and a non-existent `MeseroId` binding. It has no inputs or forms to Add, Edit, or Delete mesas.

### Affected Areas
- `dominio/Mesa.cs` — Ensure it maps properly to the database columns (e.g., using `Mesero` object).
- `negocio/MesaNegocio.cs` — Fix the compiler errors and implement `Agregar`, `Modificar`, `Eliminar`, and `ObtenerPorId`.
- `tpi-progra3-G16A/Mesas.aspx` — Add the CRUD UI (form/modal, Action buttons in GridView, DropDownList for Mesero selection, Validation).
- `tpi-progra3-G16A/Mesas.aspx.cs` — Implement event handlers for CRUD operations (Save, Edit, Delete, Cancel).

### Approaches
1. **Direct Form in Mesas.aspx (Standard Web Forms approach)**
   - Pros: Simple, keep everything on a single page, easy to implement without Ajax.
   - Cons: The page can become cluttered if there are many inputs.
   - Effort: Low

2. **Separate Edit Page (e.g., FormMesa.aspx)**
   - Pros: cleaner separation of concerns, keeps list page fast.
   - Cons: Needs redirecting, more file overhead.
   - Effort: Medium

### Recommendation
We recommend **Approach 1 (Direct Form in Mesas.aspx)** using a collapsible edit form or modal layout. Since it's an academic project, having the list and form on the same page using standard ASP.NET controls is simple, robust, and fast. We will also fix the compiler error in `MesaNegocio.cs` by joining `Mesas` with `Usuarios` and properly mapping the `Mesero` object.

### Risks
- **Concurrency/Integrity**: Table `Numero` must be unique in the database. UI must handle potential unique constraint violations gracefully.
- **Foreign Key constraint**: `MeseroId` references `Usuarios`. We must populate the mesero dropdown only with users of role `Mesero` from the DB.

### Ready for Proposal
Yes. We have a clear understanding of what is broken, what needs to be added, and how to structure it.
