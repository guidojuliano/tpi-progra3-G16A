<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="tpi_progra3_G16A.Pedidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
    <div class="col-lg-10">

        <!-- Cartel -->
        <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
            <div class="container-fluid py-2">
                <h1 class="display-5 fw-bold text-warning">Gestión de Pedidos</h1>
                <p class="col-md-10 fs-5 text-secondary">
                    Apertura de pedidos por mesa, carga de insumos, descuento automático de stock y emisión de tickets al momento del cobro y liberación.
                </p>
            </div>
        </div>

        <!-- Selector de Mesa -->
        <div class="card bg-dark text-white border-secondary mb-4">
            <div class="card-body">
                <label for="ddlMesas" class="form-label">Seleccionar Mesa</label>
                <asp:DropDownList ID="ddlMesas" runat="server" CssClass="form-select" 
                    AutoPostBack="true" OnSelectedIndexChanged="ddlMesas_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <!-- Panel: Mesa Libre -->
        <asp:Panel ID="pnlMesaLibre" runat="server" Visible="false" CssClass="card bg-dark text-white border-secondary mb-4">
            <div class="card-body">
                <h4 class="text-warning">Mesa Libre</h4>
                <label for="ddlMeseros" class="form-label">Asignar Mesero</label>
                <asp:DropDownList ID="ddlMeseros" runat="server" CssClass="form-select mb-3">
                </asp:DropDownList>
                <asp:Button ID="btnAbrirPedido" runat="server" Text="Abrir Pedido" 
                    CssClass="btn btn-warning" OnClick="btnAbrirPedido_Click" />
            </div>
        </asp:Panel>

        <!-- Panel: Mesa Ocupada -->
        <asp:Panel ID="pnlMesaOcupada" runat="server" Visible="false" CssClass="card bg-dark text-white border-secondary mb-4">
            <div class="card-body">
                <h4 class="text-warning">Pedido Activo</h4>
                <p>Mesero: <asp:Label ID="lblMesero" runat="server"></asp:Label></p>
                <p>Total: $<asp:Label ID="lblTotal" runat="server"></asp:Label></p>

                <!-- Borrador de la ronda actual -->
        <asp:Panel ID="pnlBorrador" runat="server" CssClass="card bg-dark border-warning text-white p-3 mb-4" Visible="false">
            <h5 class="text-warning">Borrador de la Ronda Actual</h5>
            <asp:GridView ID="gvBorrador" runat="server" CssClass="table table-dark table-striped mt-2" 
                AutoGenerateColumns="false"
                OnRowCommand="gvBorrador_RowCommand"
                DataKeyNames="Id">
                <Columns>
                    <asp:BoundField DataField="Insumo.Nombre" HeaderText="Insumo" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:N2}" />
                    <asp:ButtonField CommandName="EliminarBorrador" Text="X" HeaderText="" 
                        ButtonType="Button" ControlStyle-CssClass="btn btn-danger btn-sm" />
                </Columns>
            </asp:GridView>
            <div class="mt-3">
                <label class="form-label text-white">Observaciones para la Cocina</label>
                <asp:TextBox ID="txtObservacionesComanda" runat="server" CssClass="form-control mb-3" 
                    placeholder="Ej: Las papas sin sal, la carne jugosa..."></asp:TextBox>
                <asp:Button ID="btnEnviarCocina" runat="server" Text="Enviar a Cocina (Marchar)" 
                    CssClass="btn btn-success fw-bold" OnClick="btnEnviarCocina_Click" />
            </div>
        </asp:Panel>

           <!-- Listado de Comandas agrupadas -->
        <asp:Repeater ID="repComandas" runat="server" 
            OnItemDataBound="repComandas_ItemDataBound" 
            OnItemCommand="repComandas_ItemCommand">
            <ItemTemplate>
                <div class="card bg-dark border-secondary mb-3 text-white">
                    <div class="card-header d-flex justify-content-between align-items-center bg-dark border-secondary py-2">
                        <h5 class="mb-0 text-warning fw-bold">Comanda #<%# Eval("Id") %></h5>
                        <span class='<%# GetEstadoBadgeClass(Eval("Estado")) %>'><%# Eval("Estado") %></span>
                    </div>
                    <div class="card-body p-3">
                        <asp:GridView ID="gvItems" runat="server" CssClass="table table-dark table-striped align-middle mb-0"
                            AutoGenerateColumns="false" GridLines="None" DataKeyNames="Id" 
                            OnRowCommand="gvItems_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Insumo.Nombre" HeaderText="Insumo" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="{0:N2}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:N2}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEliminarItem" runat="server" Text="X" 
                                            CommandName="EliminarItem" 
                                            CommandArgument='<%# Eval("Id") %>' 
                                            CssClass="btn btn-danger btn-sm"
                                            Visible='<%# Eval("Comanda.Estado").ToString() == "Pendiente" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <div class="mt-2 text-end">
                            <asp:Button ID="btnCancelarComanda" runat="server" Text="Cancelar Comanda"
                                CommandName="CancelarComanda" 
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-outline-danger btn-sm fw-bold"
                                Visible='<%# Eval("Estado").ToString() == "Pendiente" %>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

          <!-- Formulario agregar insumo -->
            <asp:Panel ID="pnlAgregarInsumo" runat="server" CssClass="card bg-secondary mt-3 p-3">
                <h5 class="text-white">Agregar Insumo</h5>
                <div class="mb-2">
                    <label class="form-label text-white">Insumo</label>
                    <asp:DropDownList ID="ddlInsumos" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="mb-2">
                    <label class="form-label text-white">Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" TextMode="Number" Text="1"></asp:TextBox>
                </div>
                <asp:Button ID="btnAgregarInsumo" runat="server" Text="Sumar a Comanda"
                    CssClass="btn btn-warning" OnClick="btnAgregarInsumo_Click" />
            </asp:Panel>

        <!-- Botón cerrar y cobrar -->
        <asp:Button ID="btnCerrarPedido" runat="server" Text="Cerrar y Cobrar" 
            CssClass="btn btn-danger mt-3" OnClick="btnCerrarPedido_Click" />
            </div>
        </asp:Panel>

    </div>
</div>
</asp:Content>
