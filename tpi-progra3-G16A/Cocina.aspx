<%@ Page Title="Cocina - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cocina.aspx.cs" Inherits="tpi_progra3_G16A.Cocina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-comanda {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        .card-comanda:hover {
            transform: translateY(-3px);
            box-shadow: 0 5px 15px rgba(255, 193, 7, 0.15);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="row justify-content-center">
        <div class="col-lg-10">
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Flujo de Cocina</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Trazabilidad completa de comandas en preparación. El listado se actualiza automáticamente cada 30 segundos.
                    </p>
                 </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="upCocina" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timerRefresh" runat="server" Interval="30000" OnTick="timerRefresh_Tick" />

            <div class="row justify-content-center">
                <div class="col-lg-10">
                    <!-- Mensaje si no hay comandas -->
                    <asp:PlaceHolder ID="phSinComandas" runat="server" Visible="false">
                        <div class="alert alert-secondary text-center py-5 border-secondary bg-dark text-white" role="alert">
                            <i class="bi bi-emoji-smile display-4 text-warning d-block mb-3"></i>
                            <h4 class="alert-heading fw-bold">¡Cocina al día!</h4>
                            <p class="mb-0 text-secondary">No hay comandas pendientes ni en preparación en este momento.</p>
                        </div>
                    </asp:PlaceHolder>

                    <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4 mt-2">
                        <asp:Repeater ID="repComandas" runat="server" OnItemCommand="repComandas_ItemCommand" OnItemDataBound="repComandas_ItemDataBound">
                            <ItemTemplate>
                                <div class="col">
                                    <div class='<%# GetCardBorderClass(((dominio.Comanda)Container.DataItem).Estado) %> card-comanda'>
                                        <div class="card-header border-secondary d-flex justify-content-between align-items-center py-3">
                                            <div>
                                                <h5 class="fw-bold mb-0 text-warning">Mesa <%# ((dominio.Comanda)Container.DataItem).Pedido.Mesa.Numero %></h5>
                                                <small class="text-secondary">Comanda #<%# ((dominio.Comanda)Container.DataItem).Id %></small>
                                            </div>
                                            <span class='<%# GetEstadoBadgeClass(((dominio.Comanda)Container.DataItem).Estado) %> px-2 py-1'>
                                                <%# ((dominio.Comanda)Container.DataItem).Estado %>
                                            </span>
                                        </div>
                                        <div class="card-body">
                                            <p class="text-secondary small mb-2"><i class="bi bi-clock me-1"></i>Espera: <strong class="text-white"><%# GetTiempoEspera(((dominio.Comanda)Container.DataItem).FechaHora) %></strong></p>
                                            
                                            <h6 class="fw-bold text-secondary border-bottom border-secondary pb-1 mt-3">Ítems</h6>
                                            <ul class="list-unstyled mb-3">
                                                <asp:Repeater ID="repDetalles" runat="server">
                                                    <ItemTemplate>
                                                        <li class="d-flex justify-content-between mb-1">
                                                            <span><strong class="text-warning"><%# ((dominio.DetallePedido)Container.DataItem).Cantidad %>x</strong> <%# ((dominio.DetallePedido)Container.DataItem).Insumo.Nombre %></span>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>

                                            <asp:PlaceHolder ID="phObservaciones" runat="server" Visible='<%# !string.IsNullOrEmpty(((dominio.Comanda)Container.DataItem).Observaciones) %>'>
                                                <div class="alert alert-warning py-2 px-3 mb-0" style="font-size: 0.9em;">
                                                    <i class="bi bi-exclamation-triangle-fill me-1"></i><strong>Nota:</strong> <%# ((dominio.Comanda)Container.DataItem).Observaciones %>
                                                </div>
                                            </asp:PlaceHolder>
                                        </div>
                                        <div class="card-footer border-secondary text-end py-3">
                                            <asp:Button ID="btnEmpezar" runat="server" Text="Empezar preparación" CssClass="btn btn-outline-warning btn-sm fw-bold w-100 mb-2" CommandName="Empezar" CommandArgument='<%# ((dominio.Comanda)Container.DataItem).Id %>' />
                                            <asp:Button ID="btnCompletar" runat="server" Text="Completar preparación" CssClass="btn btn-success btn-sm fw-bold w-100" CommandName="Completar" CommandArgument='<%# ((dominio.Comanda)Container.DataItem).Id %>' />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
