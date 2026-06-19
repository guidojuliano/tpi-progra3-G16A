using System;
using System.Web.UI;
using dominio;

namespace tpi_progra3_G16A
{
    public partial class Site : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                Usuario user = (Usuario)Session["usuario"];
                lblUserDisplay.Text = $"{user.Nombre} {user.Apellido} ({user.Rol})";
                phAnonimo.Visible = false;
                phUsuario.Visible = true;
            }
            else
            {
                phAnonimo.Visible = true;
                phUsuario.Visible = false;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("Default.aspx", false);
        }
    }
}
