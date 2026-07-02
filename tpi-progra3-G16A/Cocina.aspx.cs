using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using negocio;
using dominio;

namespace tpi_progra3_G16A
{
    public partial class Cocina : System.Web.UI.Page
    {
        private PedidoNegocio _pedidoNegocio = new PedidoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SesionActiva(Session["usuario"]) || 
                (!Seguridad.EsCocina(Session["usuario"]) && !Seguridad.EsGerente(Session["usuario"])))
            {
                Session.Add("error", "Sección reservada para el personal de Cocina o Gerencia.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarComandas();
            }
        }

        private void CargarComandas()
        {
            try
            {
                List<Comanda> comandasActivas = _pedidoNegocio.ObtenerComandasActivas();

                if (comandasActivas == null || comandasActivas.Count == 0)
                {
                    phSinComandas.Visible = true;
                    repComandas.DataSource = null;
                    repComandas.DataBind();
                }
                else
                {
                    phSinComandas.Visible = false;
                    repComandas.DataSource = comandasActivas;
                    repComandas.DataBind();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al cargar las comandas de cocina: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void timerRefresh_Tick(object sender, EventArgs e)
        {
            CargarComandas();
        }

        protected void repComandas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int idComanda = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Empezar")
                {
                    // Validar estado antes de actualizar para evitar problemas de concurrencia
                    Comanda comanda = _pedidoNegocio.ObtenerComandaPorId(idComanda);
                    if (comanda != null && comanda.Estado == EstadoDetalle.Pendiente)
                    {
                        _pedidoNegocio.ActualizarEstadoComanda(idComanda, EstadoDetalle.EnPreparacion);
                        CargarComandas();
                    }
                    else
                    {
                        CargarComandas();
                        // Mostrar alerta de que ya no está pendiente
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertState", "showToast('La comanda ya fue tomada por otro cocinero o cancelada.', 'warning');", true);
                    }
                }
                else if (e.CommandName == "Completar")
                {
                    Comanda comanda = _pedidoNegocio.ObtenerComandaPorId(idComanda);
                    if (comanda != null && comanda.Estado == EstadoDetalle.EnPreparacion)
                    {
                        _pedidoNegocio.ActualizarEstadoComanda(idComanda, EstadoDetalle.Listo);
                        CargarComandas();
                        ScriptManager.RegisterStartupScript(this, GetType(), "AlertSuccess", "showToast('Comanda marcada como LISTA para retirar.', 'success');", true);
                    }
                    else
                    {
                        CargarComandas();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertError", $"showToast('Error al actualizar el estado: {ex.Message}', 'danger');", true);
            }
        }

        protected void repComandas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Comanda comanda = (Comanda)e.Item.DataItem;

                // Cargar detalles de insumos en el repeater anidado
                Repeater repDetalles = (Repeater)e.Item.FindControl("repDetalles");
                if (repDetalles != null)
                {
                    repDetalles.DataSource = comanda.Detalles;
                    repDetalles.DataBind();
                }

                // Configurar visibilidad de botones según estado de la comanda
                Button btnEmpezar = (Button)e.Item.FindControl("btnEmpezar");
                Button btnCompletar = (Button)e.Item.FindControl("btnCompletar");

                if (comanda.Estado == EstadoDetalle.Pendiente)
                {
                    btnEmpezar.Visible = true;
                    btnCompletar.Visible = false;
                }
                else if (comanda.Estado == EstadoDetalle.EnPreparacion)
                {
                    btnEmpezar.Visible = false;
                    btnCompletar.Visible = true;
                }
                else
                {
                    btnEmpezar.Visible = false;
                    btnCompletar.Visible = false;
                }
                // Si es Gerente, ocultar ambos botones
                if (Seguridad.EsGerente(Session["usuario"]))
                {
                    btnEmpezar.Visible = false;
                    btnCompletar.Visible = false;
                }
            }
        }

        // --- Funciones auxiliares de binding ---

        public string GetTiempoEspera(DateTime fechaHora)
        {
            TimeSpan diferencia = DateTime.Now - fechaHora;
            if (diferencia.TotalMinutes < 1)
            {
                return "Menos de 1 min";
            }
            return $"{(int)diferencia.TotalMinutes} min";
        }

        public string GetEstadoBadgeClass(EstadoDetalle estado)
        {
            if (estado == EstadoDetalle.Pendiente)
            {
                return "badge bg-warning text-dark";
            }
            if (estado == EstadoDetalle.EnPreparacion)
            {
                return "badge bg-info text-dark";
            }
            return "badge bg-secondary";
        }

        public string GetCardBorderClass(EstadoDetalle estado)
        {
            if (estado == EstadoDetalle.Pendiente)
            {
                return "card h-100 bg-dark border-warning text-white";
            }
            if (estado == EstadoDetalle.EnPreparacion)
            {
                return "card h-100 bg-dark border-info text-white";
            }
            return "card h-100 bg-dark border-secondary text-white";
        }
    }
}
