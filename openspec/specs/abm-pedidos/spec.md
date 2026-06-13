# abm-pedidos Specification

## Purpose

This specification defines the behavior for managing restaurant orders (Pedidos), including opening an order for a table, adding items through orders (Comandas), and closing/billing the order.

## Requirements

### Requirement: Apertura de Pedido

The system MUST allow a Mesero to open a new Pedido for a Mesa only if the Mesa is currently `Libre` and has a Mesero assigned.

#### Scenario: Apertura Exitosa de Mesa
- GIVEN a Mesa with number 5 in state `Libre` and Mesero "Martin Gomez" assigned
- WHEN the Mesero opens a Pedido for Mesa 5
- THEN the system MUST set Mesa 5's state to `Ocupada`
- AND the system MUST generate a new Pedido with state `Abierto`, current timestamp, and Total 0.00

#### Scenario: Apertura Rechazada por Mesa Ocupada
- GIVEN a Mesa in state `Ocupada`
- WHEN the Mesero tries to open a Pedido for that Mesa
- THEN the system MUST reject the action and display an error message

### Requirement: Registro de Consumos (Comandas)

The system MUST allow adding one or more items (Insumos) to an open Pedido through a Comanda. The system MUST decrement the stock of the selected Insumo by the requested quantity.

#### Scenario: Agregar Ítems con Stock Disponible
- GIVEN an open Pedido for Mesa 5
- AND an Insumo "Milanesa" with Stock 20
- WHEN the Mesero registers a Comanda for 2 "Milanesa"
- THEN the system MUST create the Comanda in state `Pendiente`
- AND the system MUST create the Details linked to the Comanda
- AND the system MUST decrement the "Milanesa" Stock to 18

#### Scenario: Rechazar Registro por Stock Insuficiente
- GIVEN an open Pedido for Mesa 5
- AND an Insumo "Milanesa" with Stock 1
- WHEN the Mesero registers a Comanda for 2 "Milanesa"
- THEN the system MUST reject the Comanda
- AND the system MUST not modify the stock of "Milanesa"

### Requirement: Cierre y Facturación de Pedido

The system MUST allow closing the Pedido, calculating the final total sum of all delivered and preparation-finished details, printing a ticket, and freeing the Mesa.

#### Scenario: Facturación y Cierre de Pedido
- GIVEN an open Pedido for Mesa 5 with a total consumption of 8200.00
- WHEN the Mesero closes the Pedido
- THEN the system MUST update the Pedido state to `Cerrado`
- AND the system MUST update the Mesa 5 state to `Libre`
- AND the system MUST clear the assigned Mesero of Mesa 5 (set to NULL)

### Requirement: Modificar Estado Comanda

The system MUST allow updating the state of a Comanda (e.g. from `Pendiente` to `EnPreparacion`, `Listo`, or `Entregado`).

#### Scenario: Actualización de Estado de Comanda
- GIVEN a Comanda with Id 5001 in state `Pendiente`
- WHEN the system updates the Comanda state to `EnPreparacion`
- THEN the system MUST set the Comanda state in the database to `EnPreparacion`
