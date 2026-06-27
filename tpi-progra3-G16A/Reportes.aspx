<%@ Page Title="Reportes y Estadísticas - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="tpi_progra3_G16A.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .kpi-card {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        .kpi-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.3);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <!-- Encabezado -->
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Dashboard de Reportes</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Métricas de rendimiento comercial, volumen de ventas y consolidación de jornada para la toma de decisiones estratégicas.
                    </p>
                 </div>
            </div>
        </div>
    </div>

    <!-- Sección KPIs -->
    <div class="row justify-content-center g-3 mb-5">
        <div class="col-lg-10">
            <div class="row g-3">
                <!-- KPI 1: Recaudación Total -->
                <div class="col-md-4">
                    <div class="card bg-dark border-success text-white kpi-card">
                        <div class="card-body d-flex align-items-center justify-content-between p-4">
                            <div>
                                <h6 class="text-secondary fw-bold text-uppercase small mb-1">Recaudación Total</h6>
                                <h3 class="text-success fw-bold mb-0">
                                    <asp:Label ID="lblRecaudacion" runat="server" Text="$0.00"></asp:Label>
                                </h3>
                            </div>
                            <div class="text-success fs-2">
                                <i class="bi bi-cash-coin"></i>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- KPI 2: Pedidos Totales -->
                <div class="col-md-4">
                    <div class="card bg-dark border-primary text-white kpi-card">
                        <div class="card-body d-flex align-items-center justify-content-between p-4">
                            <div>
                                <h6 class="text-secondary fw-bold text-uppercase small mb-1">Pedidos Totales</h6>
                                <h3 class="text-primary fw-bold mb-0">
                                    <asp:Label ID="lblCantidadPedidos" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                            <div class="text-primary fs-2">
                                <i class="bi bi-receipt"></i>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- KPI 3: Ticket Promedio -->
                <div class="col-md-4">
                    <div class="card bg-dark border-warning text-white kpi-card">
                        <div class="card-body d-flex align-items-center justify-content-between p-4">
                            <div>
                                <h6 class="text-secondary fw-bold text-uppercase small mb-1">Ticket Promedio</h6>
                                <h3 class="text-warning fw-bold mb-0">
                                    <asp:Label ID="lblTicketPromedio" runat="server" Text="$0.00"></asp:Label>
                                </h3>
                            </div>
                            <div class="text-warning fs-2">
                                <i class="bi bi-calculator"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Sección de Grillas / Reportes Detallados -->
    <div class="row justify-content-center g-4 mb-5">
        <div class="col-lg-10">
            <div class="row g-4">
                <!-- Columna Izquierda: Productos más Vendidos -->
                <div class="col-md-6">
                    <div class="card bg-dark border-secondary text-white p-4 h-100">
                        <h4 class="h5 text-warning mb-4">
                            <i class="bi bi-graph-up-arrow me-2"></i>Top 5 Productos más Vendidos
                        </h4>
                        <div class="table-responsive">
                            <asp:GridView ID="dgvProductosMasVendidos" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle border-secondary mb-0">
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                    <asp:BoundField DataField="CantidadVendida" HeaderText="Cantidad" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Total Recaudado" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end">
                                        <ItemTemplate>
                                            <%# string.Format("${0:N2}", Eval("TotalVendido")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <!-- Columna Derecha: Ventas por Mesero -->
                <div class="col-md-6">
                    <div class="card bg-dark border-secondary text-white p-4 h-100">
                        <h4 class="h5 text-warning mb-4">
                            <i class="bi bi-people-fill me-2"></i>Rendimiento por Mesero
                        </h4>
                        <div class="table-responsive">
                            <asp:GridView ID="dgvVentasPorMesero" runat="server" AutoGenerateColumns="false" CssClass="table table-dark table-striped align-middle border-secondary mb-0">
                                <Columns>
                                    <asp:BoundField DataField="NombreMesero" HeaderText="Mesero" />
                                    <asp:BoundField DataField="CantidadPedidos" HeaderText="Servicios" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Total Ventas" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end">
                                        <ItemTemplate>
                                            <%# string.Format("${0:N2}", Eval("TotalVendido")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
