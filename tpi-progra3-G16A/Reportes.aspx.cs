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
                ReporteNegocio reporteNegocio = new ReporteNegocio();
                decimal recaudacion = reporteNegocio.ObtenerRecaudacionTotal();
                int cantidadPedidos = reporteNegocio.ObtenerCantidadPedidos();

                lblRecaudacion.Text = string.Format("${0:N2}", recaudacion);
                lblCantidadPedidos.Text = cantidadPedidos.ToString();

                decimal ticketPromedio = 0;
                if (cantidadPedidos > 0)
                {
                    ticketPromedio = recaudacion / cantidadPedidos;
                }
                lblTicketPromedio.Text = string.Format("${0:N2}", ticketPromedio);

                dgvProductosMasVendidos.DataSource = reporteNegocio.ObtenerProductosMasVendidos();
                dgvProductosMasVendidos.DataBind();

                dgvVentasPorMesero.DataSource = reporteNegocio.ObtenerVentasPorMesero();
                dgvVentasPorMesero.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}

