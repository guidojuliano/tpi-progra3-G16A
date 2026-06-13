# Implementation Progress: ABM Pedidos (Backend & Database Only)

**Change**: abm-pedidos
**Mode**: Standard

### Completed Tasks
- [x] 1.1 Modificar `scripts/RestoBarDb.sql` agregando tablas `Pedidos`, `Comandas` y `DetallesPedidos`.
- [x] 1.2 Agregar datos de prueba (seed data) para mesas y comandas inactivas en `scripts/RestoBarDb.sql`.
- [x] 2.1 Implementar `ObtenerPedidoAbiertoPorMesa` en `negocio/PedidoNegocio.cs`.
- [x] 2.2 Implementar `AbrirPedido` (inserta en `Pedidos`, actualiza `Mesas` a Ocupada).
- [x] 2.3 Implementar `RegistrarComanda` (inserta comanda y detalles, actualiza stock de insumo).
- [x] 2.4 Implementar `CerrarYCobrarPedido` (calcula total, cierra pedido, libera mesa y mesero).
- [x] 2.5 Implementar `ActualizarEstadoComanda` (actualiza el estado de una comanda).
- [x] 3.1 Correr `dotnet build` para verificar compilación sin errores.
- [x] 3.2 Verificar mediante consultas directas en la base de datos que la creación, consulta, actualización y eliminación lógica funcionan correctamente.

### Files Changed
| File | Action | What Was Done |
|------|--------|---------------|
| `scripts/RestoBarDb.sql` | Modified | Added schema definitions for Pedidos, Comandas, and DetallesPedidos, and query verifications. |
| `negocio/PedidoNegocio.cs` | Modified | Implemented C# data access logic for managing order life cycles, comanda detail insertion, stock validations, and billing. |
| `openspec/changes/abm-pedidos/tasks.md` | Modified | Updated tasks checklist status to complete. |

### Deviations from Design
None — implementation matches design.

### Issues Found
- Docker container was restarting due to CRLF line endings in `init-db.sh`. Fixed the line endings to LF and restarted the container successfully.
- Initial NuGet restore threw an OutOfMemoryException. Resolved by building with `--no-restore` flag, completing the compilation successfully with 0 errors.

### Remaining Tasks
None.

### Workload / PR Boundary
- Mode: single PR
- Current work unit: DB & Backend CRUD
- Boundary: Starts with SQL table creation and ends with the full logic compile validation
- Estimated review budget impact: ~200 lines (Low)

### Status
9/9 tasks complete. Ready for verify.
