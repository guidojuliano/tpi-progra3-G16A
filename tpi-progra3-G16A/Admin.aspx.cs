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
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.EsGerente(Session["usuario"]))
            {
                Session.Add("error", "No tienes permisos de Gerente para acceder a la administración.");
                Response.Redirect("Error.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosYGrillas();
            }
        }

        private void CargarDatosYGrillas()
        {
            try
            {
                MesaNegocio mesaNegocio = new MesaNegocio();
                dgvMesas.DataSource = mesaNegocio.ObtenerMesas();
                dgvMesas.DataBind();

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                dgvEmpleados.DataSource = usuarioNegocio.ObtenerUsuarios();
                dgvEmpleados.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        // --- Acciones de Mesas ---
        protected void btnNuevoMesa_Click(object sender, EventArgs e)
        {
            LimpiarCamposMesa();
            lblMesaFormTitle.Text = "Nueva Mesa";
            pnlEstadoMesa.Visible = false;
            // Inyectar script para abrir el modal de Mesa
            ClientScript.RegisterStartupScript(this.GetType(), "OpenMesaModal", "var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
        }

        protected void dgvMesas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = dgvMesas.SelectedRow.Cells[0].Text;
                MesaNegocio mesaNegocio = new MesaNegocio();
                Mesa mesa = mesaNegocio.ObtenerMesaPorId(int.Parse(id));
                pnlEstadoMesa.Visible = true;

                if (mesa != null)
                {
                    if (mesa.Estado == EstadoMesa.Ocupada)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                            "showToast('No se puede editar una mesa ocupada.', 'warning');", true);
                        return;
                    }
                    txtMesaNumero.Text = mesa.Numero.ToString();
                    ddlMesaEstado.SelectedValue = mesa.Estado.ToString();
                    lblMesaFormTitle.Text = "Editar Mesa (ID " + id + ")";

                    // Inyectar script para abrir el modal de Mesa
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenMesaModal", "var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardarMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMesaNumero.Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('El número de mesa es requerido.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
                    return;
                }

                if (!int.TryParse(txtMesaNumero.Text.Trim(), out int numeroMesa) || numeroMesa <= 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('El número de mesa debe ser un entero positivo.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
                    return;
                }

                MesaNegocio mesaNegocio = new MesaNegocio();
                Mesa mesa = new Mesa();

                mesa.Numero = numeroMesa;
                mesa.Estado = (EstadoMesa)Enum.Parse(typeof(EstadoMesa), ddlMesaEstado.SelectedValue);

                mesa.Mesero = null;

                string msg = "";
                if (lblMesaFormTitle.Text.Contains("ID"))
                {
                    // Editar
                    string titleText = lblMesaFormTitle.Text;
                    int startIndex = titleText.IndexOf("(ID ") + 4;
                    int endIndex = titleText.IndexOf(")");
                    string idStr = titleText.Substring(startIndex, endIndex - startIndex);
                    mesa.Id = int.Parse(idStr);

                    // Verificar que el número no exista en otra mesa distinta
                    var mesas = mesaNegocio.ObtenerMesas();
                    if (mesas.Any(m => m.Numero == numeroMesa && m.Id != mesa.Id))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                            "showToast('Ya existe una mesa con ese numero. Ingrese un numero diferente.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
                        return;
                    }

                    mesaNegocio.ModificarMesa(mesa);
                    msg = "Mesa modificada con éxito.";
                }
                else
                {
                    // Verificar que el número de mesa no exista
                    var mesas = mesaNegocio.ObtenerMesas();
                    if (mesas.Any(m => m.Numero == numeroMesa))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning",
                            "showToast('Ya existe una mesa con ese numero. Ingrese un numero diferente.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalMesa')); myModal.show();", true);
                        return;
                    }

                    mesa.Estado = EstadoMesa.Libre; // siempre libre al crear
                    mesaNegocio.AgregarMesa(mesa);
                    msg = "Mesa agregada con exito.";
                }

                LimpiarCamposMesa();
                CargarDatosYGrillas();
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess", $"showToast('{msg}', 'success');", true);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        private void LimpiarCamposMesa()
        {
            txtMesaNumero.Text = string.Empty;
            ddlMesaEstado.SelectedIndex = 0;
        }

        // --- Acciones de Empleados ---
        protected void btnNuevoEmpleado_Click(object sender, EventArgs e)
        {
            LimpiarCamposEmpleado();
            lblEmpleadoFormTitle.Text = "Agregar Empleado";
            
            // Inyectar script para abrir el modal de Empleado
            ClientScript.RegisterStartupScript(this.GetType(), "OpenEmpModal", "var myModal = new bootstrap.Modal(document.getElementById('modalEmpleado')); myModal.show();", true);
        }

        protected void dgvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = dgvEmpleados.SelectedRow.Cells[0].Text;
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario emp = usuarioNegocio.ObtenerUsuarioPorId(int.Parse(id));

                if (emp != null)
                {
                    txtEmpNombre.Text = emp.Nombre;
                    txtEmpApellido.Text = emp.Apellido;
                    txtEmpEmail.Text = emp.Email;
                    txtEmpPass.Text = string.Empty; // No mostrar la contraseña existente por seguridad
                    ddlEmpRol.SelectedValue = emp.Rol.ToString();
                    chkEmpActivo.Checked = emp.Activo;
                    lblEmpleadoFormTitle.Text = "Editar Empleado (ID " + id + ")";

                    // Inyectar script para abrir el modal de Empleado
                    ClientScript.RegisterStartupScript(this.GetType(), "OpenEmpModal", "var myModal = new bootstrap.Modal(document.getElementById('modalEmpleado')); myModal.show();", true);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnGuardarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmpNombre.Text.Trim()) || string.IsNullOrEmpty(txtEmpApellido.Text.Trim()) || string.IsNullOrEmpty(txtEmpEmail.Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('Complete los campos obligatorios (Nombre, Apellido, Email).', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalEmpleado')); myModal.show();", true);
                    return;
                }

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario emp = new Usuario();

                emp.Nombre = txtEmpNombre.Text.Trim();
                emp.Apellido = txtEmpApellido.Text.Trim();
                emp.Email = txtEmpEmail.Text.Trim();
                emp.Password = txtEmpPass.Text;
                emp.Rol = (Rol)Enum.Parse(typeof(Rol), ddlEmpRol.SelectedValue);
                emp.Activo = chkEmpActivo.Checked;

                string msg = "";
                if (lblEmpleadoFormTitle.Text.Contains("ID"))
                {
                    // Editar
                    string titleText = lblEmpleadoFormTitle.Text;
                    int startIndex = titleText.IndexOf("(ID ") + 4;
                    int endIndex = titleText.IndexOf(")");
                    string idStr = titleText.Substring(startIndex, endIndex - startIndex);
                    emp.Id = int.Parse(idStr);

                    if (!emp.Activo)
                    {
                        MesaNegocio mesaNegocio = new MesaNegocio();
                        List<Mesa> mesas = mesaNegocio.ObtenerMesas();
                        bool atendiendoMesa = mesas.Any(m => m.Mesero != null && m.Mesero.Id == emp.Id);

                        if (atendiendoMesa)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToastWarning", "showToast('No se puede desactivar un empleado que tiene una mesa asignada.', 'warning'); var myModal = new bootstrap.Modal(document.getElementById('modalEmpleado')); myModal.show();", true);
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(emp.Password))
                    {
                        Usuario empExistente = usuarioNegocio.ObtenerUsuarioPorId(emp.Id);
                        if (empExistente != null)
                        {
                            emp.Password = empExistente.Password;
                        }
                    }

                    usuarioNegocio.ModificarUsuario(emp);
                    msg = "Empleado modificado con éxito.";
                }
                else
                {
                    // Nuevo
                    if (string.IsNullOrEmpty(emp.Password))
                    {
                        emp.Password = "123456"; // Contraseña por defecto si se deja vacía
                    }
                    usuarioNegocio.AgregarUsuario(emp);
                    msg = "Empleado agregado con éxito.";
                }

                LimpiarCamposEmpleado();
                CargarDatosYGrillas();
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToastSuccess", $"showToast('{msg}', 'success');", true);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx", false);
            }
        }

        private void LimpiarCamposEmpleado()
        {
            txtEmpNombre.Text = string.Empty;
            txtEmpApellido.Text = string.Empty;
            txtEmpEmail.Text = string.Empty;
            txtEmpPass.Text = string.Empty;
            ddlEmpRol.SelectedIndex = 0;
            chkEmpActivo.Checked = true;
        }
    }
}
