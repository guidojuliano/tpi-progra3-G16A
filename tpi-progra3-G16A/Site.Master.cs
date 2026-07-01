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

                // Mostrar items del menú según el rol del usuario
                liMesas.Visible = user.Rol == Rol.Mesero || user.Rol == Rol.Gerente;
                liPedidos.Visible = user.Rol == Rol.Mesero || user.Rol == Rol.Gerente;
                liCocina.Visible = user.Rol == Rol.Cocina || user.Rol == Rol.Gerente;
                liCatalogo.Visible = user.Rol == Rol.Gerente || user.Rol == Rol.Mesero;
                liAdmin.Visible = user.Rol == Rol.Gerente;
                liReportes.Visible = user.Rol == Rol.Gerente;
            }
            else
            {
                phAnonimo.Visible = true;
                phUsuario.Visible = false;

                // Ocultar todos los accesos restringidos para usuarios anónimos
                liMesas.Visible = false;
                liPedidos.Visible = false;
                liCocina.Visible = false;
                liCatalogo.Visible = false;
                liAdmin.Visible = false;
                liReportes.Visible = false;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("Default.aspx", false);
        }
    }
}
