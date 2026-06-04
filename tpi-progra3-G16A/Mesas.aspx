<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="tpi_progra3_G16A.Mesas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Gestión de Mesas</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Control de mesas libres y ocupadas. Asignación diaria de meseros a mesas realizada por el Gerente para habilitar la operación.
                    </p>
                 </div>
            </div>
        </div>
    </div>
    <div class="row justify-content-center mt-4">
        <div class="col-lg-10">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="Numero" HeaderText="Número" />
                    <asp:BoundField DataField="MeseroId" HeaderText="ID Mesero" NullDisplayText="-" />
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# Eval("Estado").ToString() == "Libre" ? "badge bg-success" : "badge bg-danger" %>'>
                                <%# Eval("Estado") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
