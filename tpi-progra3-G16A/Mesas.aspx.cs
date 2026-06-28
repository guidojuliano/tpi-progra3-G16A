using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace tpi_progra3_G16A
{
    public partial class Mesas : System.Web.UI.Page
    {
        public class MesaDashboardViewModel
        {
            public int Id { get; set; }
            public int Numero { get; set; }
            public bool EsLibre { get; set; }
            public string NombreMesero { get; set; }
            public int PedidoId { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SesionActiva(Session["usuario"]) || 
                (!Seguridad.EsMesero(Session["usuario"]) && !Seguridad.EsGerente(Session["usuario"])))
            {
                Session.Add("error", "Sección reservada para el personal de Meseros o Gerencia.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarSalón();
            }
        }

        private void CargarSalón()
        {
            try
            {
                var negocioMesa = new MesaNegocio();
                var negocioPedido = new PedidoNegocio();
                var mesas = negocioMesa.ObtenerMesas();
                
                var listaVM = new List<MesaDashboardViewModel>();
                
                foreach (var m in mesas)
                {
                    var vm = new MesaDashboardViewModel
                    {
                        Id = m.Id,
                        Numero = m.Numero,
                        EsLibre = m.Estado.ToString() == "Libre"
                    };
                    
                    if (!vm.EsLibre)
                    {
                        var pedido = negocioPedido.ObtenerPedidoAbiertoPorMesa(m.Id);
                        if (pedido != null)
                        {
                            vm.NombreMesero = $"{pedido.Mesero.Nombre} {pedido.Mesero.Apellido}";
                            vm.PedidoId = pedido.Id;
                        }
                        else
                        {
                            vm.NombreMesero = m.Mesero != null ? $"{m.Mesero.Nombre} {m.Mesero.Apellido}" : "Sin asignar";
                            vm.PedidoId = 0;
                        }
                    }
                    
                    listaVM.Add(vm);
                }
                
                repMesas.DataSource = listaVM;
                repMesas.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
