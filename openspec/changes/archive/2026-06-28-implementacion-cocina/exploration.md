## Exploration: Implementacion de la Pantalla de Cocina

### Current State
El backend para el flujo de Cocina está completamente implementado:
- `negocio/PedidoNegocio.cs` posee los métodos `ObtenerComandasActivas()` y `ActualizarEstadoComanda()`.
- La base de datos tiene las tablas `Comandas` y `DetallesPedidos` creadas y pobladas con datos de prueba.
Sin embargo, en el frontend:
- `tpi-progra3-G16A/Cocina.aspx` y `tpi-progra3-G16A/Cocina.aspx.cs` son solo placeholders estáticos que validan la sesión y el rol (`Cocina` o `Gerente`), pero no muestran información dinámica ni permiten interactuar con las comandas.

### Affected Areas
- `tpi-progra3-G16A/Cocina.aspx` — Para agregar la interfaz gráfica (tarjetas de comandas, lista de insumos y botones de acción).
- `tpi-progra3-G16A/Cocina.aspx.cs` — Para implementar la lógica del code-behind que interactúa con `PedidoNegocio`, carga las comandas y maneja las transiciones de estado.

### Approaches
1. **Vista con Controles Estándar y UpdatePanel (Recomendado):**
   - Usar un control `Repeater` o `GridView` embebido dentro de un `UpdatePanel` de ASP.NET AJAX.
   - Pros: Implementación directa usando el ciclo de vida clásico de Web Forms; recargas parciales de página automáticas sin perder el estado del scroll; seguro y rápido de implementar.
   - Cons: Cierto overhead por el tamaño del ViewState transmitido en cada actualización parcial.
   - Effort: Low

2. **Carga y Actualización vía WebMethods (Client-side Rendering con JS):**
   - Implementar un endpoint `WebMethod` estático en el code-behind y consumir los datos usando JavaScript (`fetch` / JSON) para renderizar en el cliente.
   - Pros: Reducción del tráfico de red (sin ViewState); interfaz más fluida y desacoplada.
   - Cons: Requiere escribir código JavaScript ad-hoc para renderizado, aumentando la complejidad y alejándose de los patrones actuales de la solución.
   - Effort: Medium

### Recommendation
Se recomienda el **Enfoque 1 (UpdatePanel y Repeater)**. Al ser una aplicación construida con ASP.NET Web Forms y Bootstrap, un `UpdatePanel` acoplado a un `Timer` para refresco automático provee la reactividad necesaria en la cocina sin agregar complejidad de scripts del lado del cliente ni alterar el estilo arquitectónico del proyecto.

### Risks
- **Falta de refresco en tiempo real:** Si el cocinero no refresca manualmente, no verá las comandas nuevas.
  *Mitigación:* Implementar un control `asp:Timer` dentro del `UpdatePanel` configurado para actualizar el listado cada 30 segundos.
- **Concurrencia al tomar comandas:** Dos cocineros podrían intentar tomar la misma comanda en simultáneo.
  *Mitigación:* Validar el estado actual en la base de datos antes de proceder con el cambio en `ActualizarEstadoComanda`.

### Ready for Proposal
Yes
