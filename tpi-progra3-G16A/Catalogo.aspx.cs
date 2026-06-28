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
    public partial class Catalogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.EsGerente(Session["usuario"]))
            {
                Session.Add("error", "No tienes permisos de Gerente para acceder al catálogo.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarInsumosEnGrilla();
            }
        }

        private void CargarInsumosEnGrilla()
        {
            try
            {
                InsumoNegocio insumoNegocio = new InsumoNegocio();
                dgvProductos.DataSource = insumoNegocio.ObtenerInsumos();
                dgvProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            lblFormTitle.Text = "Agregar Insumo";
            
            // Inyectar script para abrir el modal desde el servidor
            ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "var myModal = new bootstrap.Modal(document.getElementById('modalProducto')); myModal.show();", true);
        }

        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = dgvProductos.SelectedRow.Cells[0].Text;
                InsumoNegocio insumoNegocio = new InsumoNegocio();
                Insumo insumo = insumoNegocio.ObtenerInsumoPorId(int.Parse(id));

                if (insumo != null)
                {
                    txtNombre.Text = insumo.Nombre;
                    txtPrecio.Text = insumo.Precio.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                    txtStock.Text = insumo.Stock.ToString();
                    ddlTipo.SelectedValue = insumo.Tipo.ToString();
                    chkActivo.Checked = insumo.Activo;
                    lblFormTitle.Text = "Editar Insumo (ID " + id + ")";

                    // Inyectar script para abrir el modal con los datos cargados
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "var myModal = new bootstrap.Modal(document.getElementById('modalProducto')); myModal.show();", true);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNombre.Text.Trim()) || string.IsNullOrEmpty(txtPrecio.Text.Trim()) || string.IsNullOrEmpty(txtStock.Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('Complete los campos obligatorios (Nombre, Precio, Stock).', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalProducto')); myModal.show();", true);
                    return;
                }

                decimal precio;
                int stock;
                if (!decimal.TryParse(txtPrecio.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out precio) || !int.TryParse(txtStock.Text.Trim(), out stock))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('Precio y Stock deben ser números válidos.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalProducto')); myModal.show();", true);
                    return;
                }

                InsumoNegocio insumoNegocio = new InsumoNegocio();
                Insumo insumo = new Insumo();

                insumo.Nombre = txtNombre.Text.Trim();
                insumo.Precio = precio;
                insumo.Stock = stock;
                insumo.Tipo = (TipoInsumo)Enum.Parse(typeof(TipoInsumo), ddlTipo.SelectedValue);
                insumo.Activo = chkActivo.Checked;

                // Si stock es 0, forzar inactivo automaticamente
                if (insumo.Stock == 0)
                    insumo.Activo = false;

                string msg = "";
                // Si el titulo contiene "ID", estamos editando
                if (lblFormTitle.Text.Contains("ID"))
                {
                    string titleText = lblFormTitle.Text;
                    int startIndex = titleText.IndexOf("(ID ") + 4;
                    int endIndex = titleText.IndexOf(")");
                    string idStr = titleText.Substring(startIndex, endIndex - startIndex);
                    insumo.Id = int.Parse(idStr);

                    insumoNegocio.ModificarInsumo(insumo);
                    msg = "Producto modificado con éxito.";
                }
                else
                {
                    insumoNegocio.AgregarInsumo(insumo);
                    msg = "Producto agregado con éxito.";
                }

                LimpiarCampos();
                CargarInsumosEnGrilla();
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess", $"showToast('{msg}', 'success');", true);
            }
            catch (InvalidOperationException ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                    $"showToast('{ex.Message}', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalProducto')); myModal.show();", true);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
            ddlTipo.SelectedIndex = 0;
            chkActivo.Checked = true;
            lblFormTitle.Text = "Agregar Insumo";
        }
    }
}
