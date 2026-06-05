<%@ Page Title="Error - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="tpi_progra3_G16A.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-8 col-md-10">
            <div class="card bg-dark text-white border-danger">
                <div class="card-header bg-danger text-white py-3">
                    <h2 class="h5 mb-0"><i class="bi bi-exclamation-triangle-fill me-2"></i>Error en la Aplicación</h2>
                </div>
                <div class="card-body p-4">
                    <p class="fs-5 text-secondary">
                        Ha ocurrido un inconveniente al procesar la solicitud.
                    </p>
                    <div class="p-3 bg-black text-danger font-monospace rounded mb-4 style-code" style="max-height: 300px; overflow-y: auto; font-size: 0.85rem; border: 1px solid #3c1414;">
                        <asp:Label ID="lblMensajeError" runat="server" Text="Detalle del error no disponible."></asp:Label>
                    </div>
                    <div class="d-flex gap-2">
                        <a href="Default.aspx" class="btn btn-outline-light px-4">Volver al Inicio</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
