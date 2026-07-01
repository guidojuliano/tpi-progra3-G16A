<%@ Page Title="Catálogo de Productos - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="tpi_progra3_G16A.Catalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <!-- Encabezado -->
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Catálogo de Productos</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Gestión interna de platos y bebidas para la carta del Resto Bar. Permite altas, bajas lógicas y modificaciones de precio o stock.
                    </p>
                 </div>
            </div>
        </div>
    </div>

    <!-- Contenido Principal -->
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <div class="card bg-dark border-secondary text-white p-4">
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h2 class="h5 fw-bold mb-0 text-warning"><i class="bi bi-card-list me-2"></i>Listado de Insumos</h2>
                    <!-- Botón para abrir el modal para agregar un nuevo insumo -->
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo Insumo" CssClass="btn btn-warning fw-bold btn-sm" OnClick="btnNuevo_Click" />
                </div>

                <!-- Filtro por Estado -->
                <div class="row mb-3">
                    <div class="col-md-4 col-sm-6">
                        <div class="input-group input-group-sm">
                            <span class="input-group-text bg-dark border-secondary text-secondary fw-bold">Estado</span>
                            <asp:DropDownList ID="ddlFiltroEstado" runat="server" CssClass="form-select bg-dark border-secondary text-white" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEstado_SelectedIndexChanged">
                                <asp:ListItem Text="Todos" Value="Todos"></asp:ListItem>
                                <asp:ListItem Text="Activos" Value="Activos"></asp:ListItem>
                                <asp:ListItem Text="Inactivos" Value="Inactivos"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                
                <div class="table-responsive">
                    <asp:GridView ID="dgvProductos" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle border-secondary" OnSelectedIndexChanged="dgvProductos_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="Stock" HeaderText="Stock" />
                            <asp:TemplateField HeaderText="Tipo">
                                <ItemTemplate>
                                    <span class='<%# Eval("Tipo").ToString() == "Plato" ? "badge bg-info" : "badge bg-primary" %>'>
                                        <%# Eval("Tipo") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge bg-success" : "badge bg-danger" %>'>
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

    <!-- Modal de Formulario (Bootstrap 5) -->
    <div class="modal fade" id="modalProducto" tabindex="-1" aria-labelledby="modalProductoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content bg-dark border-secondary text-white">
                <div class="modal-header border-secondary">
                    <h5 class="modal-title text-warning" id="modalProductoLabel">
                        <asp:Label ID="lblFormTitle" runat="server" Text="Agregar Insumo"></asp:Label>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtNombre" class="form-label text-secondary small fw-bold">Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control bg-dark border-secondary text-white"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtPrecio" class="form-label text-secondary small fw-bold">Precio ($)</label>
                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="number" step="0.01"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtStock" class="form-label text-secondary small fw-bold">Stock Inicial</label>
                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control bg-dark border-secondary text-white" type="number"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="ddlTipo" class="form-label text-secondary small fw-bold">Tipo de Insumo</label>
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select bg-dark border-secondary text-white">
                            <asp:ListItem Text="Plato" Value="Plato"></asp:ListItem>
                            <asp:ListItem Text="Bebida" Value="Bebida"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-check mb-2">
                        <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label text-secondary small fw-bold" for="MainContent_chkActivo">Insumo Activo</label>
                    </div>
                </div>
                <div class="modal-footer border-secondary">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-warning fw-bold" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
