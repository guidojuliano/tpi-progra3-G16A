# Tasks: ABM Pedidos (Backend & Database Only)

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | 150 - 200 lines |
| 400-line budget risk | Low |
| Chained PRs recommended | No |
| Suggested split | Not needed |
| Delivery strategy | ask-on-risk |
| Chain strategy | size-exception |

Decision needed before apply: No
Chained PRs recommended: No
Chain strategy: size-exception
400-line budget risk: Low

### Suggested Work Units

| Unit | Goal | Likely PR | Notes |
|------|------|-----------|-------|
| 1 | DB tables & `PedidoNegocio.cs` CRUD | PR 1 | Base layer and business logic |

## Phase 1: Infrastructure / Database

- [x] 1.1 Modificar `scripts/RestoBarDb.sql` agregando tablas `Pedidos`, `Comandas` y `DetallesPedidos`.
- [x] 1.2 Agregar datos de prueba (seed data) para mesas y comandas inactivas en `scripts/RestoBarDb.sql`.

## Phase 2: Capa de Negocio (Backend)

- [x] 2.1 Implementar `ObtenerPedidoAbiertoPorMesa` en `negocio/PedidoNegocio.cs`.
- [x] 2.2 Implementar `AbrirPedido` (inserta en `Pedidos`, actualiza `Mesas` a Ocupada).
- [x] 2.3 Implementar `RegistrarComanda` (inserta comanda y detalles, actualiza stock de insumo).
- [x] 2.4 Implementar `CerrarYCobrarPedido` (calcula total, cierra pedido, libera mesa y mesero).
- [x] 2.5 Implementar `ActualizarEstadoComanda` (actualiza el estado de una comanda).

## Phase 3: Verification

- [x] 3.1 Correr `dotnet build` para verificar compilación sin errores.
- [x] 3.2 Verificar mediante consultas directas en la base de datos que la creación, consulta, actualización y eliminación lógica funcionan correctamente.
