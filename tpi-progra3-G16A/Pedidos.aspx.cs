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
            if (!IsPostBack)
            {
                CargarMesas();
                CargarMeseros();
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
                    $"showToast('{ex.Message}', 'warning'", true);
            }
}
}
}
