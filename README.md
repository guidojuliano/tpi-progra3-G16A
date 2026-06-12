# TPI Programación III - Resto Bar

Este proyecto consiste en una aplicación web diseñada para la gestión integral de los pedidos y las mesas de un restaurante o resto bar. La aplicación está pensada para organizar el flujo de trabajo diario entre los distintos integrantes del local (Gerente, Meseros y Chefs) mediante una interfaz accesible con control de perfiles y seguridad.

---

## Descripción General

El sistema permite administrar de manera ágil el estado de las mesas del salón, la toma de pedidos, la comunicación en tiempo real con la cocina y la visualización de reportes diarios. 

La seguridad de la aplicación está resuelta mediante un sistema de inicio de sesión con usuario y contraseña, asignando a cada persona un rol específico que limita las pantallas y acciones a las que puede acceder.

---

## Perfiles de Usuario

El sistema cuenta con tres perfiles bien diferenciados:

*   **Gerente:** Posee acceso total a todas las funcionalidades del sistema. Su rol es puramente administrativo (no opera como mesero ni chef). Se encarga de dar de alta nuevos usuarios, asignarles sus perfiles, administrar el catálogo de insumos (platos y bebidas), gestionar los precios, controlar el stock y visualizar los reportes finales al cierre de la jornada.
*   **Mesero/a:** Es el encargado del servicio en el salón. Puede visualizar las mesas que tiene asignadas para el día, realizar la apertura de nuevos pedidos agregando los platos o bebidas solicitados, registrar el consumo de los clientes, y recibir avisos visuales cuando los platos estén listos en la cocina. Finalmente, realiza el cierre del pedido para generar la cuenta (ticket de cobro) y liberar la mesa.
*   **Chef:** Trabaja en la cocina. El sistema le muestra una pantalla específica únicamente con los platos que tiene asignados para cocinar. Al comenzar y finalizar la preparación de un plato, el chef actualiza su estado para notificar de manera automática al mesero correspondiente.

---

## Dinámica de Funcionamiento del Sistema

### 1. Mesas y Asignación
Al inicio del día, el **Gerente** es el encargado de asignar qué mesa será atendida por qué **Mesero/a**.
*   Esta asignación se puede modificar o reasignar en cualquier momento del día si es necesario.
*   Si una mesa no tiene un mesero asignado, el sistema la marcará como no disponible para operar, garantizando que ninguna mesa libre sea abierta sin un responsable.
*   Todos los pedidos realizados durante la jornada quedan vinculados directamente al mesero asignado a esa mesa en ese momento.

### 2. Gestión de Pedidos
*   Cada mesa puede tener un **único pedido activo** a la vez.
*   A lo largo del día, una misma mesa puede abrir y cerrar pedidos múltiples veces (distintas rondas de clientes que ocupen el lugar).
*   **Apertura del pedido:** El mesero asignado inicia el pedido para su mesa y añade los platos y bebidas que pidan los comensales.
*   **Cierre del pedido:** Al finalizar el servicio, el mesero cierra el pedido. Esto genera el ticket/cuenta para realizar el cobro y libera la mesa de forma automática.

### 3. Insumos (Platos y Bebidas)
*   El catálogo de insumos es administrado únicamente por el **Gerente**, quien puede agregar nuevos platos/bebidas, modificar sus precios y reponer la cantidad disponible.
*   Cada insumo tiene asociado un **Nombre, un Precio y una Cantidad en Stock**.
*   El stock se descuenta de forma automática cuando un mesero agrega un ítem a un pedido. Si el stock de un insumo llega a cero, el sistema bloquea su selección para nuevos pedidos.

### 4. Flujo de Cocina y Asignación Inteligente
*   Al tomarse un pedido, los platos se envían automáticamente a la cocina.
*   El sistema realiza una **asignación automática por plato** al chef que tenga menor carga de trabajo (menor cantidad de platos pendientes de cocinar) al momento del registro. Esto distribuye el trabajo equitativamente entre los chefs y reduce la espera de los clientes.
*   El estado del plato en cocina sigue un ciclo de vida claro:
    *   **Pendiente:** Agregado al pedido pero aún no asignado a un chef o el chef asignado no empezó a cocinarlo.
    *   **En Preparación:** El chef asignado lo tomó y se encuentra cocinándolo. El mesero puede ver que el plato está en proceso.
    *   **Listo:** El chef terminó de cocinarlo y lo marca como finalizado. Esto alerta inmediatamente al mesero para que vaya a retirarlo y entregarlo a la mesa.
    *   **Entregado:** El mesero confirma la entrega del plato en la mesa, lo que da por concluida la trazabilidad de ese ítem.

### 5. Reportes de Gestión
Los reportes son visuales y permiten al **Gerente** evaluar el rendimiento del negocio.
*   **Cierre de día:** El gerente realiza el cierre de la jornada para consolidar la facturación y habilitar las estadísticas completas del día.
*   **Filtro por fecha:** Permite consultar reportes históricos filtrando por días específicos.
*   **Reportes Disponibles:**
    *   Pedidos totales por mesa.
    *   Pedidos atendidos por cada mesero/a.
    *   Cantidad de platos cocinados por cada chef.

---

## Escenario de Ejemplo: Mesa 5 (2 personas)

Para comprender mejor la lógica de negocio y la secuencia de pasos dentro del sistema, a continuación se detalla un escenario práctico de uso:

### Paso 1: El Mesero abre la mesa
Llega la familia a la Mesa 5. El mesero la selecciona en el sistema y hace clic en **"Abrir Mesa"**.
* **Lógica de negocio:**
  * La Mesa 5 cambia su estado a `Ocupada`.
  * Se genera una nueva instancia de **Pedido**:
    * **Id:** `1001` (autogenerado por la base de datos).
    * **Mesa:** Mesa 5.
    * **Mesero:** Juan (el usuario logueado).
    * **FechaHora:** `2026-06-11 21:00` (hora en que se sentaron).
    * **Estado:** `EstadoPedido.Abierto`.
    * **Total:** `0.00` (inicialmente).

### Paso 2: El Mesero toma el pedido (Ronda 1: Comidas y Bebidas)
A las 21:05, el cliente pide *1 Milanesa con papas* (aclara: *"sin sal"*) y *1 Coca-Cola*. El mesero lo registra en su pantalla y presiona **"Enviar a Cocina"**.
* **Lógica de negocio:**
  * Se genera una instancia de **Comanda** asociada al Pedido:
    * **Id:** `5001` (autogenerado).
    * **Pedido:** Pedido 1001.
    * **FechaHora:** `2026-06-11 21:05` (hora en que se mandó a marchar).
    * **Estado:** `EstadoDetalle.Pendiente` (entra en cola de cocina).
    * **Observaciones:** *"Traer la gaseosa rápido por favor"*.
  * Se generan dos filas de **DetallePedido** asociadas a la Comanda 5001:
    * **Detalle 1:** Insumo: *Milanesa*, Cantidad: 1, PrecioUnitario: `4500.00`, Notas: *"sin sal"*.
    * **Detalle 2:** Insumo: *Coca-Cola*, Cantidad: 1, PrecioUnitario: `1200.00`, Notas: `""`.

### Paso 3: El Cocinero ve el ticket en la pantalla de cocina
En la pantalla de cocina (`Cocina.aspx`), al cocinero (e.g. Carlos) le aparece una tarjeta de la Comanda 5001 en estado **Pendiente**.
* **Control del tiempo:** El sistema compara la hora actual (21:10) con la hora de la comanda (21:05) y muestra: *"Tiempo de espera: 5 minutos"*.
* El cocinero lee la nota general (*"Traer la gaseosa rápido por favor"*) y la del plato (*"sin sal"*). Carlos presiona **"Empezar preparación"**.
* **Lógica de negocio:**
  * La Comanda 5001 se actualiza:
    * **Estado:** `EstadoDetalle.EnPreparacion`.
    * **Chef:** Carlos (el usuario logueado en la cocina).
  * La gaseosa (que se sirve en barra) se marca como servida por el mozo, o el chef prepara la comida.

### Paso 4: La comida está lista
A las 21:20, el cocinero Carlos termina la milanesa, la sirve en el plato y presiona **"Completar"** en la pantalla de cocina.
* **Lógica de negocio:**
  * La Comanda 5001 se actualiza:
    * **Estado:** `EstadoDetalle.Listo`.
  * El sistema dispara una alerta visual en la pantalla de los meseros: *"Mesa 5 Comida lista para retirar"*.

### Paso 5: El Mesero sirve la comida
El mesero Juan retira el plato de la cocina, lo lleva a la Mesa 5 y, desde su dispositivo, confirma la entrega presionando **"Entregado"**.
* **Lógica de negocio:**
  * La Comanda 5001 se actualiza:
    * **Estado:** `EstadoDetalle.Entregado`.

### Paso 6: Los clientes piden postre (Ronda 2 - Opcional)
A las 21:40, la mesa decide pedir *1 Tiramisú*. El mesero toma el pedido y lo envía.
* **Lógica de negocio:**
  * Se genera una segunda **Comanda** asociada al mismo pedido:
    * **Id:** `5002` (autogenerada).
    * **Pedido:** Pedido 1001.
    * **FechaHora:** `2026-06-11 21:40`.
    * **Estado:** `EstadoDetalle.Pendiente`.
  * Se genera una línea en **DetallePedido** asociada a la Comanda 5002:
    * **Detalle 3:** Insumo: *Tiramisú*, Cantidad: 1, PrecioUnitario: `2500.00`.
  * El flujo de preparación de la cocina se repite para la Comanda 5002.

### Paso 7: Cierre y Facturación
A las 22:00, los clientes piden la cuenta. El mesero presiona **"Cobrar Mesa 5"**.
* **Lógica de negocio:**
  * El sistema busca el Pedido abierto de la Mesa 5 (que es el Pedido 1001).
  * Realiza una consulta SQL para traer todos los detalles de todas las comandas asociadas a ese pedido:
    * Trae Detalle 1 (`$4500.00`), Detalle 2 (`$1200.00`) y Detalle 3 (`$2500.00`).
  * El sistema calcula el Total: `$4500.00 + $1200.00 + $2500.00 = $8200.00`.
  * Se genera el ticket de cobro por `$8200.00`.
  * Una vez registrado el pago:
    * El Pedido 1001 se actualiza:
      * **Total:** `8200.00`.
      * **Estado:** `EstadoPedido.Cerrado`.
    * La Mesa 5 se actualiza:
      * **Estado:** `EstadoMesa.Libre`.
      * **Mesero asignado:** `null` (disponible para el siguiente turno).

