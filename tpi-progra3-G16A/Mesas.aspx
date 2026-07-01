<%@ Page Title="Salón - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mesas.aspx.cs" Inherits="tpi_progra3_G16A.Mesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-mesa-libre {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        .card-mesa-libre:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(40, 167, 69, 0.25);
            border-color: #198754 !important;
        }
        .card-mesa-ocupada {
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }
        .card-mesa-ocupada:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(220, 53, 69, 0.25);
            border-color: #dc3545 !important;
        }
        .card-mesa-no-disponible {
            opacity: 0.5;
            cursor: not-allowed;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Salón de Mesas</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Distribución y estado en tiempo real del salón. Haz clic en cualquier mesa disponible para iniciar un nuevo pedido.
                    </p>
                 </div>
            </div>
        </div>
    </div>

    <!-- Panel de Mesas en 2D -->
    <div class="row justify-content-center">
        <div class="col-lg-10">
            <div class="row row-cols-1 row-cols-md-3 row-cols-lg-4 g-4 mt-2">
                <asp:Repeater ID="repMesas" runat="server">
                    <ItemTemplate>
                        <div class="col">
                            <!-- Mesa Disponible (Clicable) -->
                            <asp:PlaceHolder ID="phLibre" runat="server" Visible='<%# (bool)Eval("EsLibre") %>'>
                                <a href='Pedidos.aspx?mesaId=<%# Eval("Id") %>' class="card h-100 bg-dark border-success text-decoration-none text-white card-mesa-libre">
                                    <div class="card-body text-center p-4">
                                        <div class="display-6 text-success mb-2">
                                            <i class="bi bi-unlock-fill"></i>
                                        </div>
                                        <h5 class="card-title text-success fw-bold">Mesa <%# Eval("Numero") %></h5>
                                        <span class="badge bg-success bg-opacity-10 text-success border border-success px-3 py-2 mt-2">Disponible</span>
                                    </div>
                                </a>
                            </asp:PlaceHolder>

                            <!-- Mesa Ocupada (Informativa) -->
                            <asp:PlaceHolder ID="phOcupada" runat="server" Visible='<%# !(bool)Eval("EsLibre") && !(bool)Eval("EsNoDisponible") %>'>
                                <a href='Pedidos.aspx?mesaId=<%# Eval("Id") %>' class="card h-100 bg-dark border-danger text-decoration-none text-white card-mesa-ocupada">
                                <div class="card-body p-4">
                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <h5 class="card-title text-danger fw-bold mb-0">Mesa <%# Eval("Numero") %></h5>
                                        <span class="badge bg-danger bg-opacity-10 text-danger border border-danger px-2 py-1">Ocupada</span>
                                    </div>
                                    <div class="mt-3">
                                        <p class="text-secondary small mb-1"><i class="bi bi-person-fill me-1"></i>Atendida por:</p>
                                        <p class="fw-bold mb-2"><%# Eval("NombreMesero") %></p>
                                        <p class="text-secondary small mb-1"><i class="bi bi-receipt me-1"></i>Pedido Activo:</p>
                                        <p class="fw-bold text-warning mb-0">#<%# Eval("PedidoId") %></p>
                                    </div>
                                </div>
                            </a>
                            </asp:PlaceHolder>

                            <!-- Mesa No Disponible -->
                            <asp:PlaceHolder ID="phNoDisponible" runat="server" Visible='<%# (bool)Eval("EsNoDisponible") %>'>
                            <div class="card h-100 bg-dark border-secondary text-white card-mesa-no-disponible">
                                <div class="card-body text-center p-4">
                                    <div class="display-6 text-secondary mb-2">
                                        <i class="bi bi-lock-fill"></i>
                                    </div>
                                    <h5 class="card-title text-secondary fw-bold">Mesa <%# Eval("Numero") %></h5>
                                    <span class="badge bg-secondary px-3 py-2 mt-2">No Disponible</span>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
