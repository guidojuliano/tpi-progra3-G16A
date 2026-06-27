<%@ Page Title="Inicio - Resto Bar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tpi_progra3_G16A.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-lg-10">
            
            <div class="p-5 mb-4 bg-dark text-white border border-secondary rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold text-warning">Resto Bar</h1>
                    <p class="col-md-10 fs-5 text-secondary">
                        Sistema integral para la administración de mesas, pedidos y flujo de trabajo en cocina. Diseñado para optimizar los tiempos de atención y organizar los perfiles de Gerente, Meseros y Chefs.
                    </p>
                </div>
            </div>

            <div class="row g-4 mt-2">
                
                <div class="col-md-6">
                    <div class="card h-100 bg-dark text-white border-secondary">
                        <div class="card-body p-4">
                            <div class="d-flex align-items-center gap-2 mb-3">
                                <i class="bi bi-grid-3x3-gap text-warning fs-3"></i>
                                <h3 class="card-title h5 mb-0">Gestión de Mesas</h3>
                            </div>
                            <p class="card-text text-secondary small">
                                Control de mesas libres y ocupadas. Asignación diaria de meseros a mesas realizada por el Gerente para habilitar la operación.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card h-100 bg-dark text-white border-secondary">
                        <div class="card-body p-4">
                            <div class="d-flex align-items-center gap-2 mb-3">
                                <i class="bi bi-receipt text-warning fs-3"></i>
                                <h3 class="card-title h5 mb-0">Pedidos y Cuentas</h3>
                            </div>
                            <p class="card-text text-secondary small">
                                Apertura de pedidos por mesa, carga de insumos, descuento automático de stock y emisión de tickets al momento del cobro y liberación.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card h-100 bg-dark text-white border-secondary">
                        <div class="card-body p-4">
                            <div class="d-flex align-items-center gap-2 mb-3">
                                <i class="bi bi-egg-fried text-warning fs-3"></i>
                                <h3 class="card-title h5 mb-0">Flujo de Cocina</h3>
                            </div>
                            <p class="card-text text-secondary small">
                                Asignación automática de platos a los chefs con menor carga de trabajo. Trazabilidad completa desde platos pendientes hasta listos para entregar.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card h-100 bg-dark text-white border-secondary">
                        <div class="card-body p-4">
                            <div class="d-flex align-items-center gap-2 mb-3">
                                <i class="bi bi-person-gear text-warning fs-3"></i>
                                <h3 class="card-title h5 mb-0">Administración y Reportes</h3>
                            </div>
                            <p class="card-text text-secondary small">
                                Alta y administración de usuarios, control de insumos y precios, cierre de jornada y consolidación de reportes de rendimiento.
                            </p>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>
</asp:Content>
