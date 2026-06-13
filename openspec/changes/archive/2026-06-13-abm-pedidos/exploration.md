## Exploration: ABM Pedidos

### Current State
The system has `MesaNegocio`, `InsumoNegocio`, and `UsuarioNegocio` fully implemented with database access. However, `PedidoNegocio` is a placeholder class, and there are no database tables or schemas for `Pedidos`, `Comandas`, or `DetallesPedidos`. Additionally, the user-facing web page `Pedidos.aspx` is currently an empty shell.

### Affected Areas
- `scripts/RestoBarDb.sql` — Needs schema definitions and seed data for `Pedidos`, `Comandas`, and `DetallesPedidos`.
- `negocio/PedidoNegocio.cs` — Needs SQL database queries (insert, update, delete, list) to handle order life cycle and calculations.
- `tpi-progra3-G16A/Pedidos.aspx` & `Pedidos.aspx.cs` — Needs UI elements to display tables, open orders, add items, and close orders.
- `dominio/Pedido.cs`, `dominio/Comanda.cs`, `dominio/DetallePedido.cs` — Models are already defined, but we must map them correctly from DB rows.

### Approaches
1. **Direct ADO.NET N-Tier integration (Standard approach)** — Implement `PedidoNegocio` and additional logic classes (e.g. `ComandaNegocio`, `DetallePedidoNegocio` or consolidate under `PedidoNegocio`) using the existing `AccesoDatos` class. Integrate Web Forms directly to list and manage orders.
   - Pros: Follows existing project patterns, zero architectural overhead, easy for evaluator review.
   - Cons: Multi-table inserts (Order -> Comanda -> Detail) require careful transaction handling.
   - Effort: Medium

2. **Entity Framework / ORM adoption** — Introduce an ORM to handle complex relationships between Pedido, Comanda, and DetallePedido.
   - Pros: Auto-manages foreign keys and transactions.
   - Cons: Introduces a new dependency not approved in the original project stack. Higher complexity for evaluation.
   - Effort: High

### Recommendation
We recommend **Approach 1 (Direct ADO.NET N-Tier integration)**. It preserves the clean, layered style the professors expect for this course and maintains consistency with the other business logic classes. We will add a database transaction or ensure ordered sequential inserts to guarantee data integrity across Pedidos, Comandas, and DetallesPedidos.

### Risks
- **Inconsistent Database State:** Inserting a detail without a parent comanda, or a comanda without an open pedido, due to lack of SQL transaction or poor constraint definitions.
- **Stock Depletion Race Conditions:** Multiple meseros ordering the same item at the same time could push stock below zero if not validated at the database/business layer.

### Ready for Proposal
Yes. The scope is well-understood. We are ready to define the formal proposal.
