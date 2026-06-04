#!/bin/bash
# ============================================
# Entrypoint: Inicia SQL Server y ejecuta setup
# ============================================

# Arranca SQL Server en background
/opt/mssql/bin/sqlservr &

# Espera a que el puerto 1433 esté disponible
echo "Esperando a que SQL Server arranque..."
for i in {1..50}; do
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P RestoBar2026 -C -Q "SELECT 1" > /dev/null 2>&1 && break
    echo "Intento $i..."
    sleep 2
done

# Ejecuta el script de creación de la base de datos
echo "Ejecutando RestoBarDb.sql..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P RestoBar2026 -C -i /scripts/RestoBarDb.sql
echo "Base de datos inicializada correctamente"

# Pasa SQL Server a foreground
wait
