# Specification: Filtro por Fecha y Descarga de Reportes en Formato CSV

## Purpose
This specification defines the behavior for filtering the restaurant's commercial reports by a date range, both in the visual dashboard and in the downloadable CSV file.

## Requirements

### Requirement: Date Range Filter
The dashboard MUST allow a user with the `Gerente` role to filter the sales data by a start date (Fecha Desde) and an end date (Fecha Hasta).
- The inputs MUST be HTML5 date pickers.
- If no dates are specified, the system SHOULD default to displaying all-time historical data.
- The filter MUST apply to all KPIs (Recaudación, Cantidad de Pedidos, Ticket Promedio) and all detail tables (Top 5 Productos, Rendimiento por Mesero).

#### Scenario: Successfully Filtering by Date Range
- GIVEN a user logged in with the `Gerente` role on the [Reportes.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx) page
- AND the database contains orders closed on `2026-06-25` and `2026-06-28`
- WHEN the user sets "Fecha Desde" to `2026-06-27` and "Fecha Hasta" to `2026-06-29`
- AND clicks "Filtrar"
- THEN the system MUST display only the data and metrics for orders closed between `2026-06-27 00:00:00` and `2026-06-29 23:59:59`.

---

### Requirement: CSV Export of Filtered Data
The system MUST allow downloading the filtered metrics in a CSV format.
- The button MUST be labeled "Exportar Reportes (CSV)".
- The export MUST respect the active date range filter selected in the inputs.
- The CSV file MUST use UTF-8 BOM encoding for proper spreadsheet character rendering.
- Semicolons (`;`) MUST be used as delimiters.

#### Scenario: Exporting the CSV with Active Filters
- GIVEN that the user has filtered the reports for the range `2026-06-27` to `2026-06-29`
- WHEN the user clicks "Exportar Reportes (CSV)"
- THEN the system MUST generate a CSV file named `ReportesRestoBar.csv`
- AND the content of the CSV MUST only contain data from orders within that range.
- AND the file header or first rows SHOULD include the active date range for reference:
  ```csv
  Reporte;Rango de Fechas;2026-06-27 al 2026-06-29
  
  Seccion;Metrica;Valor
  Resumen;Recaudacion Total;$15.400,00
  Resumen;Pedidos Totales;3
  Resumen;Ticket Promedio;$5.133,33
  
  Producto;Cantidad;Total
  [Top Productos in range...]
  
  Mesero;Servicios;Total
  [Rendimiento Meseros in range...]
  ```
- AND the system MUST immediately complete the request to prevent page refresh side-effects.
