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
