# Delta for abm-pedidos

## ADDED Requirements

### Requirement: Visualización de Comandas Activas

The system MUST display a list of active comandas (states `Pendiente` or `EnPreparacion`) only to users with roles `Cocina` or `Gerente`. The list MUST display the elapsed waiting time since the comanda creation, its notes/observations, and its items. The system MUST refresh this list automatically every 30 seconds without reloading the entire page.

#### Scenario: Visualización Exitosa de Comandas Activas
- GIVEN a user with role `Cocina` logged in
- AND an active comanda created 5 minutes ago with notes "Sin cebolla"
- WHEN the user opens the Cocina page
- THEN the system MUST display the comanda card showing "Tiempo de espera: 5 minutos" and the note "Sin cebolla"
- AND the system MUST refresh the page asynchronously after 30 seconds

#### Scenario: Acceso Denegado a Usuarios sin Rol Autorizado
- GIVEN a user with role `Mesero` logged in
- WHEN the user tries to access the Cocina page
- THEN the system MUST reject access and redirect to the Error page

## MODIFIED Requirements

### Requirement: Modificar Estado Comanda

The system MUST allow updating the state of a Comanda (e.g. from `Pendiente` to `EnPreparacion` or `Listo`) from the kitchen interface, and updating `Listo` to `Entregado` from the orders interface.
(Previously: The system MUST allow updating the state of a Comanda (e.g. from `Pendiente` to `EnPreparacion`, `Listo`, or `Entregado`).)

#### Scenario: Actualización de Estado de Comanda
- GIVEN a Comanda in state `Pendiente` visible on the Cocina page
- WHEN the cook clicks "Empezar preparación"
- THEN the system MUST set the Comanda state in the database to `EnPreparacion`
- AND the system MUST update the UI asynchronously to show the new state

#### Scenario: Completado de Comanda en Cocina
- GIVEN a Comanda in state `EnPreparacion` visible on the Cocina page
- WHEN the cook clicks "Completar"
- THEN the system MUST set the Comanda state in the database to `Listo`
- AND the system MUST remove the comanda card from the active kitchen view
