# Script para ejecutar setup de base de datos
# Uso: .\setup-db.ps1

# Configuración
$SqlServer = ".\SQLEXPRESS"
$ScriptPath = "D:\Programacion3\scripts\RestoBarDb.sql"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Setup de Base de Datos - RestoBarDb" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar que el archivo SQL existe
if (-not (Test-Path $ScriptPath)) {
    Write-Host "ERROR: No se encontró el archivo SQL en $ScriptPath" -ForegroundColor Red
    exit 1
}

Write-Host "Conectando a SQL Server: $SqlServer" -ForegroundColor Yellow
Write-Host "Ejecutando script SQL..." -ForegroundColor Yellow
Write-Host ""

# Ejecutar el script SQL
sqlcmd -S $SqlServer -E -i $ScriptPath

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "✓ Base de datos creada exitosamente" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "✗ Error al crear la base de datos" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    exit 1
}
