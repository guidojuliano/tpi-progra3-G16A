<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="tpi_progra3_G16A.Pedidos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center mt-4">
    <div class="col-lg-10">

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

           <!-- Grila de ítems consumidos -->
           <asp:GridView ID="gvDetalles" runat="server" CssClass="table table-dark table-bordered mt-3"
            AutoGenerateColumns="false" EmptyDataText="Sin ítems registrados aún.">
            <Columns>
                <asp:BoundField DataField="Insumo.Nombre" HeaderText="Insumo" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:N2}" />
            </Columns>
        </asp:GridView>

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
                <div class="mb-2">
                    <label class="form-label text-white">Observaciones</label>
                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" placeholder="Ej: sin cebolla, bien cocido..."></asp:TextBox>
                </div>
                <asp:Button ID="btnAgregarInsumo" runat="server" Text="Agregar al Pedido"
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
