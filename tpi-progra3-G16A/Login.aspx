<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="tpi_progra3_G16A.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesión - Resto Bar</title>
    
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />

    <style>
        body {
            background-color: #121212;
            color: #e0e0e0;
        }
        .login-card {
            max-width: 420px;
            width: 100%;
        }
    </style>
</head>
<body class="min-vh-100 d-flex flex-column align-items-center justify-content-center p-3">
    <form id="form1" runat="server" class="login-card">
        <div class="card bg-dark border-secondary text-white shadow-lg p-4">
            <div class="text-center mb-4">
                <div class="d-inline-flex align-items-center justify-content-center bg-warning bg-opacity-10 border border-warning rounded-circle p-3 mb-2">
                    <i class="bi bi-shield-lock-fill text-warning fs-3"></i>
                </div>
                <h2 class="fw-bold h4">Sistema Resto Bar</h2>
                <p class="text-secondary small">Ingresa tus credenciales para acceder</p>
            </div>

            <div class="mb-3">
                <label for="txtEmail" class="form-label text-secondary small fw-bold">Correo Electrónico</label>
                <div class="input-group">
                    <span class="input-group-text bg-dark border-secondary text-secondary"><i class="bi bi-envelope"></i></span>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control bg-dark border-secondary text-white" placeholder="correo@ejemplo.com" TextMode="Email"></asp:TextBox>
                </div>
            </div>

            <div class="mb-4">
                <label for="txtPassword" class="form-label text-secondary small fw-bold">Contraseña</label>
                <div class="input-group">
                    <span class="input-group-text bg-dark border-secondary text-secondary"><i class="bi bi-key"></i></span>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control bg-dark border-secondary text-white" placeholder="••••••••" TextMode="Password"></asp:TextBox>
                </div>
            </div>

            <asp:Label ID="lblError" runat="server" CssClass="text-danger small mb-3 d-block" Visible="false"></asp:Label>

            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn btn-warning w-100 fw-bold mb-3" OnClick="btnLogin_Click" />

            <div class="text-center">
                <a href="Default.aspx" class="text-warning text-decoration-none small">
                    <i class="bi bi-arrow-left me-1"></i>Volver al Inicio
                </a>
            </div>
        </div>
    </form>

    <div class="mt-4 p-3 bg-dark border border-secondary rounded text-secondary shadow" style="max-width: 420px; width: 100%; font-size: 0.85em;">
        <h6 class="text-warning fw-bold mb-2 text-center"><i class="bi bi-info-circle me-1"></i>Credenciales de Prueba</h6>
        <table class="table table-dark table-borderless table-sm mb-0 align-middle">
            <thead>
                <tr class="text-secondary border-bottom border-secondary">
                    <th>Rol</th>
                    <th>Email</th>
                    <th>Password</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="text-white fw-bold">Gerente</td>
                    <td>juan.perez@restobar.com</td>
                    <td class="font-monospace text-warning">admin123</td>
                </tr>
                <tr>
                    <td class="text-white fw-bold">Mesero</td>
                    <td>martin.gomez@restobar.com</td>
                    <td class="font-monospace text-warning">mesero123</td>
                </tr>
                <tr>
                    <td class="text-white fw-bold">Cocina</td>
                    <td>carlos.rod@restobar.com</td>
                    <td class="font-monospace text-warning">chef123</td>
                </tr>
            </tbody>
        </table>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
