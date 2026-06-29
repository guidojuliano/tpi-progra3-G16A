# Design Document: Filtro por Fecha y Descarga de Reportes en Formato CSV

## 1. Architecture Decisions & Rationale

* **Filter parameters**:
  The range filters will be implemented using `DateTime?` in all `ReporteNegocio` methods.
  - Rationale: Making them optional ensures backward compatibility and keeps the default "all-time" behavior when no dates are selected.
  - Date boundaries: The start date will be set to `00:00:00` of the selected day. The end date will be set to `23:59:59` to capture all transactions made during that last day.

* **CSV Generation**:
  The CSV will be generated in-memory using `StringBuilder` and written directly to the HTTP Response stream.
  - Rationale: Avoids disk I/O, file locks, and permission issues on the server.
  - Excel Compatibility: The response will write a UTF-8 BOM (`EF BB BF`) preamble. Excel often struggles to display non-ASCII characters (like accents and symbols) in CSV files unless this BOM is present.

## 2. Sequence Diagram

```mermaid
sequenceDiagram
    actor Gerente
    participant Page as Reportes.aspx
    participant CS as Reportes.aspx.cs
    participant BLL as ReporteNegocio
    participant DAL as AccesoDatos
    participant DB as SQL Server

    Gerente->>Page: Ingresa fechas y hace clic en "Exportar Reportes (CSV)"
    Page->>CS: btnExportar_Click(sender, e)
    CS->>CS: Obtiene txtFechaDesde y txtFechaHasta
    CS->>BLL: ObtenerRecaudacionTotal(desde, hasta)
    BLL->>DAL: setearConsulta(SQL con WHERE FechaHora)
    BLL->>DAL: setearParametro("@desde", desde)
    BLL->>DAL: setearParametro("@hasta", hasta)
    BLL->>DAL: ejecutarLectura()
    DAL->>DB: SELECT SUM(Total)...
    DB-->>DAL: Resultado decimal
    DAL-->>BLL: SqlDataReader
    BLL-->>CS: decimal recaudacion
    Note over CS, BLL: Se repite el proceso para Pedidos, Productos y Meseros
    CS->>CS: Genera String con datos en formato CSV (BOM + delimitador ';')
    CS->>Page: Response.Write(CSV) + Response.End()
    Page-->>Gerente: Descarga de archivo ReportesRestoBar.csv
```

## 3. Class & Code Modifications

### 3.1. negocio/ReporteNegocio.cs

We will update the 4 query methods to accept optional `DateTime? desde` and `DateTime? hasta`.
Example:
```csharp
public decimal ObtenerRecaudacionTotal(DateTime? desde = null, DateTime? hasta = null)
{
    var datos = new AccesoDatos();
    try
    {
        string query = "SELECT ISNULL(SUM(Total), 0) FROM Pedidos WHERE Estado = 'Cerrado'";
        if (desde != null && hasta != null)
        {
            query += " AND FechaHora >= @desde AND FechaHora <= @hasta";
        }
        datos.setearConsulta(query);
        if (desde != null && hasta != null)
        {
            datos.setearParametro("@desde", desde.Value);
            datos.setearParametro("@hasta", hasta.Value);
        }
        datos.ejecutarLectura();
        if (datos.Lector != null && datos.Lector.Read())
        {
            return datos.Lector.GetDecimal(0);
        }
        return 0;
    }
    finally
    {
        datos.cerrarConexion();
    }
}
```
This pattern will be replicated in `ObtenerCantidadPedidos`, `ObtenerProductosMasVendidos` and `ObtenerVentasPorMesero`.

### 3.2. tpi-progra3-G16A/Reportes.aspx

Add filter inputs and action buttons in a control row right above the KPIs:
```html
<div class="row justify-content-center mb-4">
    <div class="col-lg-10">
        <div class="card bg-dark border-secondary text-white p-3">
            <div class="row g-3 align-items-end">
                <div class="col-md-3">
                    <label class="form-label text-secondary small fw-bold">Fecha Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="date"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label text-secondary small fw-bold">Fecha Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="date"></asp:TextBox>
                </div>
                <div class="col-md-6 d-flex gap-2">
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-warning fw-bold px-4" OnClick="btnFiltrar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary px-3" OnClick="btnLimpiar_Click" />
                    <asp:Button ID="btnExportarCSV" runat="server" Text="Exportar CSV" CssClass="btn btn-success fw-bold px-4 ms-auto" OnClick="btnExportarCSV_Click" />
                </div>
            </div>
        </div>
    </div>
</div>
```

### 3.3. tpi-progra3-G16A/Reportes.aspx.cs

- Implement `btnFiltrar_Click` and `btnLimpiar_Click`.
- Modify `CargarReportes` to retrieve value from controls and call BLL.
- Implement `btnExportarCSV_Click` to clean the response buffer, write the CSV contents (with UTF-8 BOM), and finalize the response.
