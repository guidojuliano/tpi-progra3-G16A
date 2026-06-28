# Design: Implementacion de la Pantalla de Cocina

## Technical Approach
Implementar la interfaz reactiva para el flujo de Cocina en `Cocina.aspx` y `Cocina.aspx.cs` consumiendo el backend existente de `PedidoNegocio.cs` para el ciclo de vida de las comandas activas (`Pendiente` y `EnPreparacion`).

## Architecture Decisions

| Componente | Opción | Tradeoff | Decisión |
|------------|--------|----------|----------|
| **Reactividad** | `UpdatePanel` + `asp:Timer` | + Fácil integración con ciclo de vida Web Forms, refresco automático de 30s.<br>- Transfiere ViewState completo en recargas parciales. | **Elegido:** Mantiene la coherencia arquitectónica y simplifica el desarrollo. |
| **Control Renderizado** | `asp:Repeater` | + HTML personalizado fácil de estructurar en Bootstrap para tarjetas.<br>- Requiere control manual del evento `OnItemDataBound`. | **Elegido:** Ofrece mayor control sobre el diseño visual comparado con `GridView`. |
| **Control Asincrónico** | `ScriptManager` local | + Obligatorio para el funcionamiento de `UpdatePanel` ya que no está en la master page. | **Elegido:** Se colocará al inicio de `Cocina.aspx`. |

## Data Flow

```
Cocina.aspx (UI) ──[Timer Tick / Acción Click]──→ Cocina.aspx.cs (Code-behind)
     ↑                                                   │
     │ (Renderiza HTML parcial)                          ▼
AccesoDatos ←── ObtenerComandasActivas/Actualizar ── PedidoNegocio
```

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `tpi-progra3-G16A/Cocina.aspx` | Modify | Reemplazar maquetado estático con un `ScriptManager`, `UpdatePanel`, un control `Timer` y un `Repeater` para las tarjetas de comandas. |
| `tpi-progra3-G16A/Cocina.aspx.cs` | Modify | Implementar `CargarComandas()`, `timerRefresh_Tick()`, `repComandas_ItemCommand()` y validación de seguridad de roles. |

## Interfaces / Contracts

No se agregan nuevas interfaces. Se consumen los siguientes métodos de `PedidoNegocio`:

```csharp
public List<Comanda> ObtenerComandasActivas();
public void ActualizarEstadoComanda(int idComanda, EstadoDetalle nuevoEstado);
```

Y el code-behind de `Cocina.aspx.cs` resolverá los comandos de UI:

```csharp
protected void repComandas_ItemCommand(object source, RepeaterCommandEventArgs e)
{
    int idComanda = Convert.ToInt32(e.CommandArgument);
    if (e.CommandName == "Empezar") {
        pedidoNegocio.ActualizarEstadoComanda(idComanda, EstadoDetalle.EnPreparacion);
    } else if (e.CommandName == "Completar") {
        pedidoNegocio.ActualizarEstadoComanda(idComanda, EstadoDetalle.Listo);
    }
    CargarComandas();
}
```

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Manual | Control de seguridad | Intentar ingresar con usuario `Mesero` y verificar redirección a `Error.aspx`. |
| Manual | Flujo de Cocina | Loguearse como `Cocina`. Iniciar preparación de comanda `Pendiente`. Completar preparación. Verificar que cambie el estado en base de datos a `Listo` y desaparezca de la pantalla. |
| Manual | Refresco automático | Generar una comanda desde `Pedidos.aspx` y verificar que a los 30 segundos aparezca automáticamente en la pantalla de cocina sin recargar el navegador. |

## Migration / Rollout
No migration required.

## Open Questions
None
