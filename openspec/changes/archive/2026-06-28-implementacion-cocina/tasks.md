# Tasks: Implementacion de la Pantalla de Cocina

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | ~150 lines |
| 400-line budget risk | Low |
| Chained PRs recommended | No |
| Suggested split | Single PR (Not needed) |
| Delivery strategy | ask-on-risk |
| Chain strategy | size-exception |

Decision needed before apply: No
Chained PRs recommended: No
Chain strategy: size-exception
400-line budget risk: Low

### Suggested Work Units

| Unit | Goal | Likely PR | Notes |
|------|------|-----------|-------|
| 1 | Implementar la vista reactiva y el backend de Cocina.aspx | PR 1 | Base branch, incluye controles, lógica de estado y verificación manual. |

## Phase 1: Foundation (Security & Infrastructure)

- [x] 1.1 Validar seguridad en `Page_Load` de [Cocina.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx.cs) para permitir acceso exclusivo a roles `Cocina` o `Gerente`.
- [x] 1.2 Agregar `<asp:ScriptManager ID="ScriptManager1" runat="server" />` al inicio de [Cocina.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx).

## Phase 2: Core Implementation (UI & Code-behind)

- [x] 2.1 Agregar `<asp:UpdatePanel ID="upCocina" runat="server">` y `<asp:Timer ID="timerRefresh" runat="server" Interval="30000" OnTick="timerRefresh_Tick" />` en [Cocina.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx).
- [x] 2.2 Agregar el `<asp:Repeater ID="repComandas" runat="server" OnItemCommand="repComandas_ItemCommand" OnItemDataBound="repComandas_ItemDataBound">` con tarjetas responsivas Bootstrap en [Cocina.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx).
- [x] 2.3 Implementar `CargarComandas()` y `timerRefresh_Tick()` en [Cocina.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx.cs) invocando `ObtenerComandasActivas()` de `PedidoNegocio`.
- [x] 2.4 Implementar `repComandas_ItemCommand()` en [Cocina.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx.cs) para procesar los botones "Empezar" (cambiar estado a `EnPreparacion`) y "Completar" (cambiar a `Listo`).
- [x] 2.5 Implementar `repComandas_ItemDataBound()` en [Cocina.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx.cs) para alternar visibilidad de botones según el estado actual de la comanda.

## Phase 3: Verification (Manual Testing)

- [x] 3.1 Probar restricción de seguridad intentando ingresar con un rol de `Mesero` o sin sesión iniciada.
- [x] 3.2 Crear una comanda de prueba en [Pedidos.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Pedidos.aspx) y verificar que aparezca de forma asíncrona tras el tick de 30 segundos.
- [x] 3.3 Validar que el botón "Empezar preparación" cambie el estado a `EnPreparacion` y que el botón "Completar" cambie el estado a `Listo`, removiendo la tarjeta de la vista.
