# Proposal: ABM Pedidos (Backend & Database Only)

## Intent
Implementar el ciclo de vida completo de un Pedido en la base de datos y en la capa de lógica de negocio, resolviendo la lógica dummy del backend en `PedidoNegocio.cs` para permitir que otros desarrolladores integren la interfaz gráfica posteriormente.

## Scope

### In Scope
- Creación de tablas SQL: `Pedidos`, `Comandas` y `DetallesPedidos` en `RestoBarDb.sql`.
- Implementación de métodos CRUD parametrizados en `PedidoNegocio.cs` (AbrirPedido, ObtenerPedidoAbiertoPorMesa, RegistrarComanda, CerrarYCobrarPedido).
- Control automático de stock de insumos al confirmar comandas en la capa de negocio.
- Modificación del estado de la mesa (`Libre` / `Ocupada`) en base de datos al abrir o cerrar pedidos.

### Out of Scope
- Diseño visual o maquetación en la página de pedidos (`Pedidos.aspx`).
- Integración de eventos UI en el code-behind (`Pedidos.aspx.cs`).
- Gestión visual o lógica del panel de cocina (`Cocina.aspx`).

## Capabilities

### New Capabilities
- `abm-pedidos-backend`: Provee los métodos de negocio necesarios para registrar la apertura de un pedido en una mesa, cargar comandas con múltiples detalles descontando stock, y realizar la facturación liberando la mesa.

### Modified Capabilities
- Ninguna.

## Approach
Se adoptará un diseño directo con ADO.NET N-Tier usando `AccesoDatos.cs`. Agregaremos tablas SQL con claves foráneas adecuadas. Implementaremos `PedidoNegocio.cs` para manejar las transacciones secuenciales del pedido (cabecera, comanda, detalle). Toda validación de stock y estado de mesas se resolverá a nivel de negocio en C#.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `scripts/RestoBarDb.sql` | Modified | Agregar tablas de Pedidos, Comandas, DetallesPedidos y seed data inicial. |
| `negocio/PedidoNegocio.cs` | Modified | Implementación real de persistencia para órdenes, comandas e insumos. |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Inconsistencia al guardar detalles sin comanda o sin pedido | Med | Inserción jerárquica estricta con excepciones controladas. |
| Venta de insumos sin stock | Med | Validar cantidad solicitada contra stock en el backend antes de guardar el detalle. |

## Rollback Plan
Revertir los cambios locales con `git checkout -- negocio/PedidoNegocio.cs scripts/RestoBarDb.sql`. En base de datos, correr `DROP TABLE DetallesPedidos, Comandas, Pedidos`.

## Dependencies
- Base de datos SQL Server ejecutándose (vía Docker en puerto 1434 o local).

## Success Criteria
- [ ] Se pueden abrir pedidos asociándolos a una mesa libre y a un mesero logueado mediante `AbrirPedido`.
- [ ] Se pueden registrar comandas con múltiples insumos (descontando stock automáticamente).
- [ ] Se puede cobrar y cerrar el pedido, actualizando el estado de la mesa a `Libre` y del pedido a `Cerrado`.
- [ ] La compilación con `dotnet build` finaliza con 0 errores.
