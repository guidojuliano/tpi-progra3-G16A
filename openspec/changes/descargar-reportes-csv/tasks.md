# Task List: Filtro por Fecha y Descarga de Reportes en Formato CSV

## 1. Phase 1: Business Logic Layer (BLL)
- [ ] **Task 1.1**: Add optional `DateTime? desde` and `DateTime? hasta` parameters to `ObtenerRecaudacionTotal` and `ObtenerCantidadPedidos` in `negocio/ReporteNegocio.cs`, appending the SQL filter if dates are provided.
- [ ] **Task 1.2**: Add the same optional parameters to `ObtenerProductosMasVendidos` and `ObtenerVentasPorMesero` in `negocio/ReporteNegocio.cs`, adapting the SQL query to join `Pedidos` and filter by its `FechaHora`.

## 2. Phase 2: User Interface & Presentation Layer (UI)
- [ ] **Task 2.1**: Edit [tpi-progra3-G16A/Reportes.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx) to add the range filter inputs (Date Desde, Date Hasta) and control buttons (Filtrar, Limpiar, Exportar CSV).
- [ ] **Task 2.2**: Update `CargarReportes` in [tpi-progra3-G16A/Reportes.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx.cs) to parse dates from the inputs and pass them down to the BLL methods.
- [ ] **Task 2.3**: Implement the click event handler for "Filtrar" (re-bind grids and KPIs) and "Limpiar" (clear textboxes and reload original dashboard data).

## 3. Phase 3: CSV Export Implementation
- [ ] **Task 3.1**: Implement `btnExportarCSV_Click` in [tpi-progra3-G16A/Reportes.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx.cs). Write data to response using `StringBuilder` formatted as CSV, using `;` as separator, prefixing UTF-8 BOM, and closing stream with `Response.End()`.

## 4. Phase 4: Verification & Compilation
- [ ] **Task 4.1**: Compile the project to ensure no syntax/type errors exist (to be done by the user as per workflow rules).
- [ ] **Task 4.2**: Verify that formatting and date boundaries logic works correctly (e.g. checking that `fechaHasta` is extended to `23:59:59` to include orders on the selected end date).
