using System;
using System.Web.UI;

namespace tpi_progra3_G16A
{
    public partial class Error : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["error"] != null)
            {
                lblMensajeError.Text = Session["error"].ToString();
                divErrorDetalle.Visible = true;
            }
        }
    }
}

