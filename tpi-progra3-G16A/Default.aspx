<%@ Page Title="Inicio - TPI" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tpi_progra3_G16A.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .hero-section {
            background: linear-gradient(135deg, rgba(99, 102, 241, 0.15) 0%, rgba(168, 85, 247, 0.15) 100%);
            border: 1px solid var(--border-glow);
            border-radius: 24px;
            padding: 3.5rem 2.5rem;
            margin-bottom: 3rem;
            position: relative;
            overflow: hidden;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
        }

        .hero-section::before {
            content: '';
            position: absolute;
            top: -50%;
            right: -20%;
            width: 300px;
            height: 300px;
            background: radial-gradient(circle, var(--accent-primary) 0%, transparent 70%);
            opacity: 0.25;
            filter: blur(40px);
            pointer-events: none;
        }

        .hero-title {
            font-size: 2.8rem;
            font-weight: 800;
            background: linear-gradient(to right, #ffffff, #a5b4fc);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            margin-bottom: 1rem;
            letter-spacing: -1px;
        }

        .layer-card {
            background-color: var(--bg-secondary);
            border: 1px solid var(--border-glow);
            border-radius: 16px;
            padding: 2rem;
            height: 100%;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
        }

        .layer-card:hover {
            transform: translateY(-6px);
            border-color: var(--accent-primary);
            box-shadow: 0 12px 30px rgba(99, 102, 241, 0.2);
        }

        .icon-box {
            width: 50px;
            height: 50px;
            background-color: rgba(99, 102, 241, 0.1);
            border: 1px solid var(--accent-primary);
            border-radius: 12px;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 1.5rem;
            color: #a5b4fc;
            font-size: 1.5rem;
            filter: drop-shadow(0 0 4px rgba(99, 102, 241, 0.3));
        }

        .layer-title {
            font-size: 1.25rem;
            font-weight: 700;
            color: var(--text-main);
            margin-bottom: 0.75rem;
        }

        .layer-desc {
            font-size: 0.9rem;
            color: var(--text-muted);
            line-height: 1.6;
            margin-bottom: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        
        <!-- Hero Banner -->
        <div class="hero-section text-center text-md-start">
            <div class="row align-items-center g-4">
                <div class="col-md-9">
                    <h1 class="hero-title">¡Estructura de 3 Capas Lista!</h1>
                    <p class="lead text-muted mb-4">
                        Tu proyecto de <strong>ASP.NET Web Forms</strong> para el <strong>TPI (Grupo 16A)</strong> ha sido inicializado con éxito. La solución se encuentra completamente organizada bajo las buenas prácticas arquitectónicas del curso de Programación 3.
                    </p>
                    <div class="d-flex flex-wrap gap-3 justify-content-center justify-content-md-start">
                        <span class="badge p-2 px-3" style="background: rgba(99,102,241,0.1); color:#a5b4fc; border: 1px solid rgba(99,102,241,0.2) !important;">
                            <i class="bi bi-layers me-1"></i>.NET Framework 4.8
                        </span>
                        <span class="badge p-2 px-3" style="background: rgba(168,85,247,0.1); color:#d8b4fe; border: 1px solid rgba(168,85,247,0.2) !important;">
                            <i class="bi bi-diagram-3 me-1"></i>Arquitectura Desacoplada
                        </span>
                        <span class="badge p-2 px-3" style="background: rgba(6,182,212,0.1); color:#67e8f9; border: 1px solid rgba(6,182,212,0.2) !important;">
                            <i class="bi bi-check-circle me-1"></i>AccesoDatos Listo
                        </span>
                    </div>
                </div>
                <div class="col-md-3 text-center d-none d-md-block">
                    <i class="bi bi-cpu" style="font-size: 8rem; color: var(--accent-primary); filter: drop-shadow(0 0 20px rgba(99, 102, 241, 0.4));"></i>
                </div>
            </div>
        </div>

        <!-- Section: Architecture Layers -->
        <div class="mb-4">
            <h2 class="h3 font-weight-bold mb-1">Arquitectura de la Solución</h2>
            <p class="text-muted">Explora los tres componentes integrados que conforman la estructura física de tu aplicación.</p>
        </div>

        <div class="row g-4">
            <!-- Domain Layer -->
            <div class="col-md-4">
                <div class="layer-card">
                    <div class="icon-box">
                        <i class="bi bi-box-seam"></i>
                    </div>
                    <h3 class="layer-title">Capa de Dominio</h3>
                    <p class="layer-desc">
                        Biblioteca de clases que contendrá tus entidades y modelos de datos puros. Es la capa común referenciada tanto por negocio como por la aplicación web.
                    </p>
                </div>
            </div>

            <!-- Business Layer -->
            <div class="col-md-4">
                <div class="layer-card">
                    <div class="icon-box">
                        <i class="bi bi-database-gear"></i>
                    </div>
                    <h3 class="layer-title">Capa de Negocio</h3>
                    <p class="layer-desc">
                        Biblioteca que aloja las reglas del negocio y el acceso a la base de datos SQL Server. Ya incluye la clase auxiliar **`AccesoDatos.cs`** lista para usar con ADO.NET.
                    </p>
                </div>
            </div>

            <!-- Presentation Layer -->
            <div class="col-md-4">
                <div class="layer-card">
                    <div class="icon-box">
                        <i class="bi bi-globe2"></i>
                    </div>
                    <h3 class="layer-title">Capa de Presentación</h3>
                    <p class="layer-desc">
                        Sitio web de **ASP.NET Web Forms** clásico que define la interfaz de usuario con controles de servidor (`.aspx`), estilos responsivos en CSS3, Bootstrap 5 y páginas maestras.
                    </p>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
