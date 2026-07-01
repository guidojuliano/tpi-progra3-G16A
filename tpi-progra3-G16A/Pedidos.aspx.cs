using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tpi_progra3_G16A
{
    public partial class Pedidos : Page
    {
        private List<DetallePedido> CarritoTemporal
        {
            get
            {
                if (Session["CarritoTemporal"] == null)
                    Session["CarritoTemporal"] = new List<DetallePedido>();
                return (List<DetallePedido>)Session["CarritoTemporal"];
            }
            set
            {
                Session["CarritoTemporal"] = value; 
            }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.EsMesero(Session["usuario"]) && !Seguridad.EsGerente(Session["usuario"]))
            {
                Session.Add("error", "No tienes permisos para acceder a los pedidos.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarMesas();
                CargarMeseros();
                CargarInsumos();

                // Pre-seleccionar mesa desde QueryString si viene de la pantalla de Salón
                if (Request.QueryString["mesaId"] != null)
                {
                    string mesaIdStr = Request.QueryString["mesaId"];
                    if (ddlMesas.Items.FindByValue(mesaIdStr) != null)
                    {
                        ddlMesas.SelectedValue = mesaIdStr;
                        MostrarEstadoMesa();
                    }
                }
            }

            AplicarRestriccionRol();
        }

        private void AplicarRestriccionRol()
        {
            var usuario = Session["usuario"] as Usuario;
            if (usuario == null) return;

            if(usuario.Rol == Rol.Gerente)
            {
                //gerente solo puede ver
                pnlAgregarInsumo.Visible = false;
                btnAbrirPedido.Visible = false;
                btnCerrarPedido.Visible = true;
                pnlAsignarMesero.Visible = false;
                pnlMeseroInfo.Visible = true;
            }
            else if(usuario.Rol == Rol.Mesero)
            {
                btnCerrarPedido.Visible = false; //mesero no puede cerrar pedidos
                ddlMeseros.Visible = false; // el mesero no elige, se asigna solo
            }
        }

        private void CargarMesas()
        {
            var mesasNegocio = new MesaNegocio();
            var mesas = mesasNegocio.ObtenerMesas();

            ddlMesas.DataSource = mesas;
            ddlMesas.DataTextField = "Numero";
            ddlMesas.DataValueField = "Id";
            ddlMesas.DataBind();

            ddlMesas.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione una mesa --", "0"));
        }

        private void CargarMeseros()
        {
            var usuarioNegocio = new UsuarioNegocio();
            var meseros = usuarioNegocio.ObtenerUsuarios().Where(u => u.Rol == Rol.Mesero && u.Activo).ToList();

            ddlMeseros.DataSource = meseros;
            ddlMeseros.DataTextField = "Nombre";
            ddlMeseros.DataValueField = "Id";
            ddlMeseros.DataBind();
        }

        protected void ddlMesas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarritoTemporal = null;
            pnlBorrador.Visible = false;
            MostrarEstadoMesa();
        }

        private void MostrarEstadoMesa()
        {
            int idMesa = int.Parse(ddlMesas.SelectedValue);

            if(idMesa == 0)
            {
                pnlMesaLibre.Visible = false;
                pnlMesaOcupada.Visible = false;
                return;
            }

            var pedidoNegocio = new PedidoNegocio();
            var pedidoAbierto = pedidoNegocio.ObtenerPedidoAbiertoPorMesa(idMesa);

            if(pedidoAbierto == null)
            { 
                pnlMesaLibre.Visible = true;
                pnlMesaOcupada.Visible = false;

                var usuario = Session["usuario"] as Usuario;
                if (usuario != null && usuario.Rol == Rol.Gerente)
                {
                    var mesaNegocio = new MesaNegocio();
                    var mesa = mesaNegocio.ObtenerMesaPorId(idMesa);
                    lblMeseroAsignado.Text = mesa.Mesero != null
                        ? mesa.Mesero.Nombre + " " + mesa.Mesero.Apellido
                        : "Sin mesero asignado";
                }
            }
            else
            {
                pnlMesaLibre.Visible = false;
                pnlMesaOcupada.Visible = true;
                lblMesero.Text = pedidoAbierto.Mesero.Nombre + " " + pedidoAbierto.Mesero.Apellido;
                lblTotal.Text = pedidoAbierto.Total.ToString("N2");
                ViewState["idPedidoActivo"] = pedidoAbierto.Id; //importante para poder usarlo en btnCerrarPedido_click para saber que pedido cerrar, sin esto al hacer postback edel botón perdería el id del pedido activo.
                CargarDetallePedido(pedidoAbierto.Id);
            }
        }

        protected void btnAbrirPedido_Click(object sender, EventArgs e)
        {
            try
            {
                int idMesa = int.Parse(ddlMesas.SelectedValue);
                var usuario = Session["usuario"] as Usuario;

                int idMesero = usuario.Rol == Rol.Mesero
                    ? usuario.Id
                    : int.Parse(ddlMeseros.SelectedValue);

                var pedidoNegocio = new PedidoNegocio();
                pedidoNegocio.AbrirPedido(idMesa, idMesero);

                MostrarEstadoMesa();
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    "showToast('Pedido abierto con exito.', 'success');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning');", true);
            }
        }

        private void CargarDetallePedido(int idPedido)
        {
            try
            {
                var pedidoNegocio = new PedidoNegocio();
                var comandas = pedidoNegocio.ObtenerComandasPorPedido(idPedido);

                repComandas.DataSource = comandas;
                repComandas.DataBind();

                ActualizarTotal(idPedido);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning');", true);
            }
        }

        private void CargarInsumos()
        {
            var insumoNegocio = new InsumoNegocio();
            var insumos = insumoNegocio.ObtenerInsumos().Where(i => i.Activo && i.Stock > 0).ToList();

            ddlInsumos.DataSource = insumos;
            ddlInsumos.DataTextField = "Nombre";
            ddlInsumos.DataValueField = "Id";
            ddlInsumos.DataBind();
        }

        protected void btnAgregarInsumo_Click(object sender, EventArgs e)
        {
            try
            {
                int idInsumo = int.Parse(ddlInsumos.SelectedValue);
                int cantidad = int.Parse(txtCantidad.Text);

                if (cantidad <= 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                        "showToast('La cantidad debe ser mayor a 0.', 'warning');", true);
                    return;
                }

                var insumoNegocio = new InsumoNegocio();
                var insumo = insumoNegocio.ObtenerInsumoPorId(idInsumo);

                var detalle = new DetallePedido
                {
                    Insumo = insumo,
                    Cantidad = cantidad,
                    PrecioUnitario = insumo.Precio
                };

                CarritoTemporal.Add(detalle);

                gvBorrador.DataSource = CarritoTemporal;
                gvBorrador.DataBind();
                pnlBorrador.Visible = true;

                txtCantidad.Text = "1";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    "showToast('Agregado al borrador.', 'success');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning');", true);
            }
        }

        private void ActualizarTotal(int idPedido)
        {
            var pedidoNegocio = new PedidoNegocio();
            var comandas = pedidoNegocio.ObtenerComandasPorPedido(idPedido);
            var total = comandas
                .Where(c => c.Estado != EstadoDetalle.Cancelado)
                .SelectMany(c => c.Detalles)
                .Sum(d => d.Subtotal);
            lblTotal.Text = total.ToString("N2");
        }

        protected void btnCerrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                int idPedido = (int)ViewState["idPedidoActivo"];

                var pedidoNegocio = new PedidoNegocio();

                var comandas = pedidoNegocio.ObtenerComandasPorPedido(idPedido);
                bool tienePendientes = comandas.Any(c =>
                    c.Estado != EstadoDetalle.Entregado &&
                    c.Estado != EstadoDetalle.Cancelado);

                if (tienePendientes)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                        "showToast('No se puede cerrar. Hay comandas que no fueron entregadas o canceladas.', 'warning');", true);
                    return;
                }

                decimal total = pedidoNegocio.CerrarYCobrarPedido(idPedido);
                ViewState["idPedidoActivo"] = null;
                MostrarEstadoMesa();

                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    $"showToast('Pedido cerrado con exito. Total cobrado: ${total.ToString("N2")}', 'success');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning');", true);
            }
        }
        protected void gvBorrador_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarBorrador")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                CarritoTemporal.RemoveAt(index);

                gvBorrador.DataSource = CarritoTemporal;
                gvBorrador.DataBind();

                if (CarritoTemporal.Count == 0)
                    pnlBorrador.Visible = false;
            }
        }
        protected void btnEnviarCocina_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (CarritoTemporal.Count == 0) return;

                int idPedido = (int)ViewState["idPedidoActivo"];
                string observaciones = txtObservacionesComanda.Text.Trim();

                var pedidoNegocio = new PedidoNegocio();
                pedidoNegocio.RegistrarComanda(idPedido, CarritoTemporal, observaciones);

                CarritoTemporal = null;
                gvBorrador.DataSource = null;
                gvBorrador.DataBind();
                pnlBorrador.Visible = false;
                txtObservacionesComanda.Text = string.Empty;

                CargarDetallePedido(idPedido);

                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    "showToast('Comanda enviada a cocina con exito.', 'success');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning');", true);
            }
        }
        protected string GetEstadoBadgeClass(object estadoObj)
        {
            if (estadoObj == null) return "badge bg-secondary";

            string estado = estadoObj.ToString();
            switch (estado)
            {
                case "Pendiente":
                    return "badge bg-warning text-dark";
                case "EnPreparacion":
                    return "badge bg-info text-dark";
                case "Listo":
                    return "badge bg-primary text-white";
                case "Entregado":
                    return "badge bg-success text-white";
                case "Cancelado":
                    return "badge bg-danger text-white";
                default:
                    return "badge bg-secondary text-white";
            }
        }
        protected void repComandas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var comanda = (Comanda)e.Item.DataItem;
                var gvItems = (GridView)e.Item.FindControl("gvItems");

                foreach (var d in comanda.Detalles)
                    d.Comanda = comanda;

                gvItems.DataSource = comanda.Detalles;
                gvItems.DataBind();

                // Controla la visibilidad de botones según rol
                var usuario = Session["usuario"] as Usuario;
                var btnMarcarEntregado = (Button)e.Item.FindControl("btnMarcarEntregado");
                var btnCancelarComanda = (Button)e.Item.FindControl("btnCancelarComanda");

                if (usuario != null && usuario.Rol == Rol.Gerente)
                {
                    if (btnMarcarEntregado != null) btnMarcarEntregado.Visible = false;
                    if (btnCancelarComanda != null) btnCancelarComanda.Visible = false;
                }
            }
        }
        protected void repComandas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CancelarComanda")
            {
                try
                {
                    int idComanda = int.Parse(e.CommandArgument.ToString());
                    int idPedido = (int)ViewState["idPedidoActivo"];

                    var pedidoNegocio = new PedidoNegocio();
                    pedidoNegocio.CancelarComanda(idComanda);

                    CargarDetallePedido(idPedido);
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                        "showToast('Comanda cancelada con exito.', 'success');", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                        $"showToast('{ex.Message}', 'warning');", true);
                }
            }
            else if (e.CommandName == "MarcarEntregado")
            {
                try
                {
                    int idComanda = int.Parse(e.CommandArgument.ToString());
                    int idPedido = (int)ViewState["idPedidoActivo"];

                    var pedidoNegocio = new PedidoNegocio();
                    pedidoNegocio.ActualizarEstadoComanda(idComanda, EstadoDetalle.Entregado);

                    CargarDetallePedido(idPedido);
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                        "showToast('Comanda marcada como entregada.', 'success');", true);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                        $"showToast('{ex.Message}', 'warning');", true);
                }
            }
        }
    }
}
