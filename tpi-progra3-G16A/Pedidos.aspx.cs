using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using dominio;
using negocio;

namespace tpi_progra3_G16A
{
    public partial class Pedidos : Page
    {
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
                gvDetalles.Columns[4].Visible = false; //no ve la columna del boton eliminar
            }
            else if(usuario.Rol == Rol.Mesero)
            {
                btnCerrarPedido.Visible = false; //mesero no puede cerrar pedidos
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
                int idMesero = int.Parse(ddlMeseros.SelectedValue);

                var pedidoNegocio = new PedidoNegocio();
                pedidoNegocio.AbrirPedido(idMesa, idMesero);

                MostrarEstadoMesa();
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    "showToast('Pedido abierto con éxito.', 'success');", true);
            }
            catch(Exception ex)
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

                var detalles = comandas.SelectMany(c => c.Detalles).ToList();

                gvDetalles.DataSource = detalles;
                gvDetalles.DataBind();
                ActualizarTotal(idPedido);
            }
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                $"showToast('{ex.Message}', 'warning');", true);
            }
        }

        private void CargarInsumos()
        {
            var insumoNegocio = new InsumoNegocio();
            var insumos = insumoNegocio.ObtenerInsumos();

            ddlInsumos.DataSource = insumos;
            ddlInsumos.DataTextField = "Nombre";
            ddlInsumos.DataValueField = "Id";
            ddlInsumos.DataBind();
        }

        protected void btnAgregarInsumo_Click(object sender, EventArgs e)
        {
            try
            {
                int idPedido = (int)ViewState["idPedidoActivo"];
                int idInsumo = int.Parse(ddlInsumos.SelectedValue);
                int cantidad = int.Parse(txtCantidad.Text);
                string observaciones = txtObservaciones.Text.Trim();

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

                var pedidoNegocio = new PedidoNegocio();
                pedidoNegocio.RegistrarComanda(idPedido, new List<DetallePedido> { detalle }, observaciones);

                txtCantidad.Text = "1";
                txtObservaciones.Text = string.Empty;
                CargarDetallePedido(idPedido);

                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                    "showToast('Insumo agregado con exito.', 'success');", true);
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
            var total = comandas.SelectMany(c => c.Detalles).Sum(d => d.Subtotal);
            lblTotal.Text = total.ToString("N2");
        }

        protected void btnCerrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                int idPedido = (int)ViewState["idPedidoActivo"];

                var pedidoNegocio = new PedidoNegocio();
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

        protected void gvDetalles_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int idDetalle = int.Parse(gvDetalles.DataKeys[index].Value.ToString());
                    int idPedido = (int)ViewState["idPedidoActivo"];

                    var pedidoNegocio = new PedidoNegocio();
                    pedidoNegocio.EliminarDetalle(idDetalle);

                    CargarDetallePedido(idPedido);
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess",
                        "showToast('Insumo eliminado con exito.', 'success');", true);
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
