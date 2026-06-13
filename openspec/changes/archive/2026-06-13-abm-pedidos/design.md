# Design: ABM Pedidos (Backend & Database Only)

## Technical Approach
Implementaremos las tablas necesarias en la base de datos y proveeremos métodos en la clase `PedidoNegocio.cs` para el ciclo completo (Apertura -> Carga de Comandas -> Cierre y Facturación). Esta capa proveerá una API limpia para que sea consumida por la capa de presentación.

## Architecture Decisions

| Decision | Option | Tradeoff | Choice | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| **Manejo de Transacciones** | Inserciones individuales en backend | Riesgo de comanda huérfana en fallos de red | Transacciones a nivel de negocio usando bloques Try/Catch de ADO.NET | Garantiza atomicidad al registrar comandas con múltiples detalles e insumos. |
| **Consistencia de Stock** | Trigger en base de datos | Dificulta la visibilidad de errores en C# | Descuento explícito en C# en el método de negocio | Permite validar stock antes de realizar la inserción y tirar excepciones claras. |

## Data Flow

```
Llamada a PedidoNegocio.cs (Capa de Presentación)
      │
      ▼ (Abrir Pedido / Cargar Comanda)
Clase PedidoNegocio.cs (Capa de Negocio)
      │
      ├─► Valida stock en InsumoNegocio
      ▼
AccesoDatos.cs ──► SQL Server (Transacción parametrizada)
```

## File Changes

| File | Action | Description |
|------|--------|-------------|
| `scripts/RestoBarDb.sql` | Modify | Agregar esquemas para `Pedidos`, `Comandas` y `DetallesPedidos` con datos de prueba iniciales. |
| `negocio/PedidoNegocio.cs` | Modify | Implementar métodos para abrir, consultar, cargar consumos y cerrar pedidos. |

## Interfaces / Contracts

```csharp
namespace negocio
{
    public class PedidoNegocio
    {
        // Obtiene el pedido abierto de una mesa o retorna null si está libre
        public Pedido ObtenerPedidoAbiertoPorMesa(int idMesa);
        
        // Abre un nuevo pedido, seteando la mesa como Ocupada
        public void AbrirPedido(int idMesa, int idMesero);
        
        // Registra una comanda con sus detalles y descuenta el stock de los insumos
        public void RegistrarComanda(int idPedido, List<DetallePedido> detalles, string observaciones);
        
        // Calcula el total sumando los detalles de sus comandas, cierra el pedido y libera la mesa
        public decimal CerrarYCobrarPedido(int idPedido);
        
        // Actualiza el estado de una comanda específica
        public void ActualizarEstadoComanda(int idComanda, EstadoDetalle nuevoEstado);
    }
}
```

## Testing Strategy

| Layer | What to Test | Approach |
|-------|-------------|----------|
| Integration | Inserción consistente de Pedido y Comanda | Ejecutar script de pruebas o consultas manuales y verificar integridad en SQL Server. |
| Manual | Métodos de PedidoNegocio | Crear un pequeño script de prueba en un endpoint o consola para validar las llamadas a los métodos. |

## Migration / Rollout
Se aplicarán las nuevas tablas ejecutando la sección correspondiente en `RestoBarDb.sql` sobre la base de datos actual. No se requiere migración de datos existentes.

## Open Questions
Ninguna.
