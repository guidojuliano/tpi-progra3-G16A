<%@ Page Title="Panel de Administración - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="tpi_progra3_G16A.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <!-- Encabezado -->
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Panel de Administración</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Área reservada para Gerentes. Permite la administración integral de las mesas del salón y el personal del Resto Bar.
                    </p>
                 </div>
            </div>
        </div>
    </div>

    <!-- Sección de Mesas -->
    <div class="row justify-content-center mb-5">
        <div class="col-lg-10">
            <div class="card bg-dark border-secondary text-white p-4">
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h2 class="h4 text-warning mb-0"><i class="bi bi-grid-3x3-gap me-2"></i>Gestión de Mesas</h2>
                    <asp:Button ID="btnNuevoMesa" runat="server" Text="Nueva Mesa" CssClass="btn btn-warning fw-bold btn-sm" OnClick="btnNuevoMesa_Click" />
                </div>
                
                <div class="table-responsive">
                    <asp:GridView ID="dgvMesas" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle border-secondary" OnSelectedIndexChanged="dgvMesas_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="Numero" HeaderText="Número" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='<%# Eval("Estado").ToString() == "Libre" ? "badge bg-success" : Eval("Estado").ToString() == "Ocupada" ? "badge bg-danger" : "badge bg-secondary" %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mesero Asignado">
                                <ItemTemplate>
                                    <%# Eval("Mesero") != null ? ((dominio.Usuario)Eval("Mesero")).Nombre + " " + ((dominio.Usuario)Eval("Mesero")).Apellido : "Sin asignar" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" SelectText="<i class='bi bi-pencil-square'></i> Editar" ControlStyle-CssClass="btn btn-outline-warning btn-sm" HeaderText="Acción" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- Sección de Empleados -->
    <div class="row justify-content-center mb-5">
        <div class="col-lg-10">
            <div class="card bg-dark border-secondary text-white p-4">
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h2 class="h4 text-warning mb-0"><i class="bi bi-people-fill me-2"></i>Gestión de Empleados</h2>
                    <asp:Button ID="btnNuevoEmpleado" runat="server" Text="Nuevo Empleado" CssClass="btn btn-warning fw-bold btn-sm" OnClick="btnNuevoEmpleado_Click" />
                </div>
                
                <div class="table-responsive">
                    <asp:GridView ID="dgvEmpleados" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle border-secondary" OnSelectedIndexChanged="dgvEmpleados_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Rol">
                                <ItemTemplate>
                                    <span class='<%# Eval("Rol").ToString() == "Gerente" ? "badge bg-danger" : Eval("Rol").ToString() == "Cocina" ? "badge bg-info" : "badge bg-primary" %>'>
                                        <%# Eval("Rol") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge bg-success" : "badge bg-secondary" %>'>
                                        <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" SelectText="<i class='bi bi-pencil-square'></i> Editar" ControlStyle-CssClass="btn btn-outline-warning btn-sm" HeaderText="Acción" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Formulario de Mesa (Bootstrap 5) -->
    <div class="modal fade" id="modalMesa" tabindex="-1" aria-labelledby="modalMesaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content bg-dark border-secondary text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title text-warning" id="modalMesaLabel">
                        <asp:Label ID="lblMesaFormTitle" runat="server" Text="Agregar / Modificar Mesa"></asp:Label>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtMesaNumero" class="form-label text-secondary small fw-bold">Número de Mesa</label>
                        <asp:TextBox ID="txtMesaNumero" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="number" min="1"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="ddlMesaEstado" class="form-label text-secondary small fw-bold">Estado</label>
                        <asp:DropDownList ID="ddlMesaEstado" runat="server" CssClass="form-select bg-dark border-secondary text-white">
                            <asp:ListItem Text="Libre" Value="Libre"></asp:ListItem>
                            <asp:ListItem Text="Ocupada" Value="Ocupada"></asp:ListItem>
                            <asp:ListItem Text="No Disponible" Value="NoDisponible"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="mb-2">
                        <label for="ddlMesaMesero" class="form-label text-secondary small fw-bold">Asignar Mesero</label>
                        <asp:DropDownList ID="ddlMesaMesero" runat="server" CssClass="form-select bg-dark border-secondary text-white">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer border-secondary">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarMesa" runat="server" Text="Guardar Mesa" CssClass="btn btn-warning fw-bold" OnClick="btnGuardarMesa_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Formulario de Empleado (Bootstrap 5) -->
    <div class="modal fade" id="modalEmpleado" tabindex="-1" aria-labelledby="modalEmpleadoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content bg-dark border-secondary text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title text-warning" id="modalEmpleadoLabel">
                        <asp:Label ID="lblEmpleadoFormTitle" runat="server" Text="Agregar / Modificar Empleado"></asp:Label>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row g-2">
                        <div class="col-md-6 mb-3">
                            <label for="txtEmpNombre" class="form-label text-secondary small fw-bold">Nombre</label>
                            <asp:TextBox ID="txtEmpNombre" runat="server" CssClass="form-control bg-dark border-secondary text-white"></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="txtEmpApellido" class="form-label text-secondary small fw-bold">Apellido</label>
                            <asp:TextBox ID="txtEmpApellido" runat="server" CssClass="form-control bg-dark border-secondary text-white"></asp:TextBox>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label for="txtEmpEmail" class="form-label text-secondary small fw-bold">Email</label>
                        <asp:TextBox ID="txtEmpEmail" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="email"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtEmpPass" class="form-label text-secondary small fw-bold">Contraseña</label>
                        <asp:TextBox ID="txtEmpPass" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="password" placeholder="Solo para nuevos/cambios"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="ddlEmpRol" class="form-label text-secondary small fw-bold">Rol</label>
                        <asp:DropDownList ID="ddlEmpRol" runat="server" CssClass="form-select bg-dark border-secondary text-white">
                            <asp:ListItem Text="Gerente" Value="Gerente"></asp:ListItem>
                            <asp:ListItem Text="Mesero" Value="Mesero"></asp:ListItem>
                            <asp:ListItem Text="Cocina" Value="Cocina"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkEmpActivo" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label text-secondary small fw-bold" for="MainContent_chkEmpActivo">Empleado Activo</label>
                    </div>
                </div>
                <div class="modal-footer border-secondary">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarEmpleado" runat="server" Text="Guardar Empleado" CssClass="btn btn-warning fw-bold" OnClick="btnGuardarEmpleado_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
