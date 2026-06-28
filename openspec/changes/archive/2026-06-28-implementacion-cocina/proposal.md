# Proposal: Implementacion de la Pantalla de Cocina

## Intent
Implementar la interfaz gráfica y la lógica del lado del servidor para el flujo de Cocina en `Cocina.aspx`, permitiendo al personal visualizar las comandas activas (`Pendiente`, `EnPreparacion`) y gestionar su ciclo de vida en tiempo real de forma reactiva.

## Scope

### In Scope
- Creación de tarjetas visuales en `Cocina.aspx` para listar comandas activas con sus insumos, cantidades y observaciones.
- Botones de acción para cambiar de estado (`Pendiente` -> `EnPreparacion` -> `Listo`).
- Control de tiempo de espera visible por comanda.
- Temporizador (`asp:Timer` y `UpdatePanel`) para refresco automático cada 30 segundos.
- Control de acceso por rol (`Cocina` y `Gerente`).

### Out of Scope
- Gestión del estado `Entregado` (lo maneja el mesero desde `Pedidos.aspx`).
- Pantallas para reasignar platos a cocineros específicos.

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- `abm-pedidos`: Agregar requerimientos de interfaz reactiva para el personal de cocina (actualización periódica y controles de transición de estado en UI).

## Approach
Se utilizará la arquitectura en capas existente. En [Cocina.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx) se integrará un `UpdatePanel` que contendrá un `asp:Repeater` para renderizar las comandas. Un `asp:Timer` disparará actualizaciones asíncronas periódicas. El code-behind invocará a `PedidoNegocio` para recuperar los datos y registrar los cambios de estado.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| [Cocina.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx) | Modified | Reemplazar maquetado estático con `UpdatePanel`, `Repeater` y `Timer`. |
| [Cocina.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Cocina.aspx.cs) | Modified | Enlazar datos de `ObtenerComandasActivas` y manejar comandos `Empezar` y `Completar`. |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Concurrencia (dos usuarios editando el mismo estado) | Low | Validar el estado en la DB antes de aplicar `ActualizarEstadoComanda`. |
| Pérdida de foco o scroll al refrescar | Medium | El uso de `UpdatePanel` mantiene el estado del cliente y reduce la recarga visual. |

## Rollback Plan
Revertir cambios en `Cocina.aspx` y `Cocina.aspx.cs` usando `git checkout -- tpi-progra3-G16A/Cocina.aspx*`.

## Dependencies
- Base de datos dockerizada levantada y accesible.

## Success Criteria
- [ ] Listado de comandas cargado automáticamente al abrir la página.
- [ ] Transición correcta de estados al presionar botones.
- [ ] Refresco automático asíncrono sin recarga completa de página.
