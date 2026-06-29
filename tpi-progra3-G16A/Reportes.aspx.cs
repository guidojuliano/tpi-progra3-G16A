using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;

namespace tpi_progra3_G16A
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.EsGerente(Session["usuario"]))
            {
                Session.Add("error", "No tienes permisos de Gerente para acceder a los reportes.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarReportes();
            }
        }

        private void CargarReportes()
        {
            try
            {
                DateTime? desde = null;
                DateTime? hasta = null;

                if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    desde = DateTime.Parse(txtFechaDesde.Text);
                }
                if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    hasta = DateTime.Parse(txtFechaHasta.Text).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                ReporteNegocio reporteNegocio = new ReporteNegocio();
                decimal recaudacion = reporteNegocio.ObtenerRecaudacionTotal(desde, hasta);
                int cantidadPedidos = reporteNegocio.ObtenerCantidadPedidos(desde, hasta);

                lblRecaudacion.Text = string.Format("${0:N2}", recaudacion);
                lblCantidadPedidos.Text = cantidadPedidos.ToString();

                decimal ticketPromedio = 0;
                if (cantidadPedidos > 0)
                {
                    ticketPromedio = recaudacion / cantidadPedidos;
                }
                lblTicketPromedio.Text = string.Format("${0:N2}", ticketPromedio);

                dgvProductosMasVendidos.DataSource = reporteNegocio.ObtenerProductosMasVendidos(desde, hasta);
                dgvProductosMasVendidos.DataBind();

                dgvVentasPorMesero.DataSource = reporteNegocio.ObtenerVentasPorMesero(desde, hasta);
                dgvVentasPorMesero.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarReportes();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            CargarReportes();
        }

        protected void btnExportarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? desde = null;
                DateTime? hasta = null;

                if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    desde = DateTime.Parse(txtFechaDesde.Text);
                }
                if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    hasta = DateTime.Parse(txtFechaHasta.Text).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                ReporteNegocio reporteNegocio = new ReporteNegocio();
                decimal recaudacion = reporteNegocio.ObtenerRecaudacionTotal(desde, hasta);
                int cantidadPedidos = reporteNegocio.ObtenerCantidadPedidos(desde, hasta);
                decimal ticketPromedio = cantidadPedidos > 0 ? recaudacion / cantidadPedidos : 0;

                var productos = reporteNegocio.ObtenerProductosMasVendidos(desde, hasta);
                var meseros = reporteNegocio.ObtenerVentasPorMesero(desde, hasta);

                System.Text.StringBuilder csv = new System.Text.StringBuilder();

                // Cabecera / Info del Filtro
                csv.AppendLine(string.Format("\"Reporte\";\"Rango de Fechas\";\"{0} al {1}\"", 
                    desde.HasValue ? desde.Value.ToString("dd/MM/yyyy") : "Inicio", 
                    hasta.HasValue ? hasta.Value.ToString("dd/MM/yyyy") : "Fin"));
                csv.AppendLine();

                // KPIs
                csv.AppendLine("\"Seccion\";\"Metrica\";\"Valor\"");
                csv.AppendLine(string.Format("\"Resumen\";\"Recaudacion Total\";\"{0:N2}\"", recaudacion));
                csv.AppendLine(string.Format("\"Resumen\";\"Pedidos Totales\";\"{0}\"", cantidadPedidos));
                csv.AppendLine(string.Format("\"Resumen\";\"Ticket Promedio\";\"{0:N2}\"", ticketPromedio));
                csv.AppendLine();

                // Top Productos
                csv.AppendLine("\"Producto\";\"Cantidad\";\"Total\"");
                foreach (var p in productos)
                {
                    csv.AppendLine(string.Format("\"{0}\";{1};\"{2:N2}\"", p.Nombre.Replace("\"", "\"\""), p.CantidadVendida, p.TotalVendido));
                }
                csv.AppendLine();

                // Ventas por Mesero
                csv.AppendLine("\"Mesero\";\"Servicios\";\"Total\"");
                foreach (var m in meseros)
                {
                    csv.AppendLine(string.Format("\"{0}\";{1};\"{2:N2}\"", m.NombreMesero.Replace("\"", "\"\""), m.CantidadPedidos, m.TotalVendido));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ReportesRestoBar.csv");
                Response.Charset = "UTF-8";
                Response.ContentType = "text/csv";

                // Preamble UTF-8 BOM
                byte[] bom = System.Text.Encoding.UTF8.GetPreamble();
                Response.BinaryWrite(bom);

                // Contenido
                byte[] content = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
                Response.BinaryWrite(content);

                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Evitamos redirigir en caso de abortar el hilo (comportamiento normal de Response.End)
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}

