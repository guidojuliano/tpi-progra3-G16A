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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Por favor, complete todos los campos.";
                    lblError.Visible = true;
                    return;
                }

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                if (usuarioNegocio.ValidarUsuario(email, password))
                {
                    List<Usuario> usuarios = usuarioNegocio.ObtenerUsuarios();
                    Usuario user = usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                    if (user != null)
                    {
                        Session.Add("usuario", user);
                    }
                    else
                    {
                        Session.Add("usuario", new Usuario { Email = email });
                    }
                    
                    Response.Redirect("Default.aspx", false);
                }
                else
                {
                    lblError.Text = "Credenciales incorrectas o usuario inactivo.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
