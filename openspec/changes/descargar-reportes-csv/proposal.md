# Proposal: Filtro por Fecha y Descarga de Reportes en Formato CSV

## 1. Intent & Context
Permitir a los usuarios con rol de Gerente filtrar los reportes comerciales del restaurante por rango de fechas (Fecha Desde y Fecha Hasta) en la pantalla de Reportes ([Reportes.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx)). Adicionalmente, habilitar un botón para exportar estos reportes ya filtrados directamente a un archivo CSV. Esto soluciona la falta de filtros históricos y mejora la portabilidad de los datos.

## 2. Approach & Affected Modules
El cambio afectará las capas de Presentación y de Negocio:

- **[ReporteNegocio.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/negocio/ReporteNegocio.cs)** (Capa de Negocio):
  - Modificar los métodos para que acepten dos parámetros opcionales de tipo `DateTime?` (`fechaDesde` y `fechaHasta`).
  - Si los parámetros de fecha se proveen, se añadirán filtros en la cláusula `WHERE` contra la columna `FechaHora` de la tabla `Pedidos` (`p.FechaHora >= @fechaDesde AND p.FechaHora <= @fechaHasta`).

- **[Reportes.aspx](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx)** (Capa de Presentación):
  - Añadir campos de entrada de tipo fecha (`<asp:TextBox ID="txtFechaDesde" runat="server" TextMode="Date" ...>`) y un botón "Filtrar" para realizar la consulta.
  - Añadir un botón con estilo Bootstrap (`btn-success`) para disparar la descarga de reportes en formato CSV.

- **[Reportes.aspx.cs](file:///C:/Users/moca_/source/repos/tpi-progra3-G16A/tpi-progra3-G16A/Reportes.aspx.cs)** (Capa de Presentación):
  - Actualizar `CargarReportes` para recuperar el rango de fechas de la interfaz y enviarlo a los métodos de `ReporteNegocio`.
  - Implementar el manejador del botón de exportación CSV, recuperando los datos filtrados, formateándolos en un stream de texto CSV con codificación UTF-8 BOM, y respondiendo en el flujo HTTP.

## 3. Rollback Plan
Si el cambio presenta fallos o inestabilidad, la regresión se realiza mediante:
1. Revertir los cambios en los archivos modificados a su estado original usando `git restore`:
   - `git restore negocio/ReporteNegocio.cs`
   - `git restore tpi-progra3-G16A/Reportes.aspx`
   - `git restore tpi-progra3-G16A/Reportes.aspx.cs`
2. No hay migraciones de base de datos que revertir.
