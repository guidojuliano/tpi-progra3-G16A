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
            </div>
        </asp:Panel>

    </div>
</div>
</asp:Content>
