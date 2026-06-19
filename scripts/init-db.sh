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

# Comprobar si la base de datos RestoBarDb ya existe
DB_EXISTS=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P RestoBar2026 -C -h -1 -Q "SET NOCOUNT ON; SELECT DB_ID('RestoBarDb')" | xargs)

if [ "$DB_EXISTS" = "NULL" ] || [ -z "$DB_EXISTS" ]; then
    echo "La base de datos no existe. Ejecutando RestoBarDb.sql..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P RestoBar2026 -C -i /scripts/RestoBarDb.sql
    echo "Base de datos inicializada correctamente"
else
    echo "La base de datos RestoBarDb ya existe (ID: $DB_EXISTS). Omitiendo inicializacion para preservar datos."
fi

# Pasa SQL Server a foreground
wait
