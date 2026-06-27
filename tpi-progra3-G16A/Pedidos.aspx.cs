using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tpi_progra3_G16A
{
    public partial class Pedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.SesionActiva(Session["usuario"]) || 
                (!Seguridad.EsMesero(Session["usuario"]) && !Seguridad.EsGerente(Session["usuario"])))
            {
                Session.Add("error", "Sección reservada para el personal de Meseros o Gerencia.");
                Response.Redirect("Error.aspx", false);
                return;
            }
        }
    }
}
