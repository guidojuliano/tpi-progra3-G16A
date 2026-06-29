using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ReporteNegocio
    {
        public decimal ObtenerRecaudacionTotal(DateTime? desde = null, DateTime? hasta = null)
        {
            var datos = new AccesoDatos();
            try
            {
                string query = "SELECT ISNULL(SUM(Total), 0) FROM Pedidos WHERE Estado = 'Cerrado'";
                if (desde != null)
                {
                    query += " AND FechaHora >= @desde";
                }
                if (hasta != null)
                {
                    query += " AND FechaHora <= @hasta";
                }
                datos.setearConsulta(query);
                if (desde != null)
                {
                    datos.setearParametro("@desde", desde.Value);
                }
                if (hasta != null)
                {
                    datos.setearParametro("@hasta", hasta.Value);
                }
                datos.ejecutarLectura();
                if (datos.Lector != null && datos.Lector.Read())
                {
                    return datos.Lector.GetDecimal(0);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int ObtenerCantidadPedidos(DateTime? desde = null, DateTime? hasta = null)
        {
            var datos = new AccesoDatos();
            try
            {
                string query = "SELECT COUNT(*) FROM Pedidos WHERE Estado = 'Cerrado'";
                if (desde != null)
                {
                    query += " AND FechaHora >= @desde";
                }
                if (hasta != null)
                {
                    query += " AND FechaHora <= @hasta";
                }
                datos.setearConsulta(query);
                if (desde != null)
                {
                    datos.setearParametro("@desde", desde.Value);
                }
                if (hasta != null)
                {
                    datos.setearParametro("@hasta", hasta.Value);
                }
                datos.ejecutarLectura();
                if (datos.Lector != null && datos.Lector.Read())
                {
                    return datos.Lector.GetInt32(0);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<ProductoMasVendido> ObtenerProductosMasVendidos(DateTime? desde = null, DateTime? hasta = null)
        {
            var lista = new List<ProductoMasVendido>();
            var datos = new AccesoDatos();
            try
            {
                string query = @"
                    SELECT TOP 5 i.Nombre, SUM(dp.Cantidad) as CantidadVendida, SUM(dp.Cantidad * dp.PrecioUnitario) as TotalVendido
                    FROM DetallesPedidos dp
                    INNER JOIN Insumos i ON dp.InsumoId = i.Id
                    INNER JOIN Comandas c ON dp.ComandaId = c.Id
                    INNER JOIN Pedidos p ON c.PedidoId = p.Id
                    WHERE p.Estado = 'Cerrado'";
                
                if (desde != null)
                {
                    query += " AND p.FechaHora >= @desde";
                }
                if (hasta != null)
                {
                    query += " AND p.FechaHora <= @hasta";
                }
                
                query += @"
                    GROUP BY i.Nombre
                    ORDER BY CantidadVendida DESC";

                datos.setearConsulta(query);
                if (desde != null)
                {
                    datos.setearParametro("@desde", desde.Value);
                }
                if (hasta != null)
                {
                    datos.setearParametro("@hasta", hasta.Value);
                }
                datos.ejecutarLectura();
                while (datos.Lector != null && datos.Lector.Read())
                {
                    lista.Add(new ProductoMasVendido
                    {
                        Nombre = datos.Lector.GetString(0),
                        CantidadVendida = datos.Lector.GetInt32(1),
                        TotalVendido = datos.Lector.GetDecimal(2)
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<VentasMesero> ObtenerVentasPorMesero(DateTime? desde = null, DateTime? hasta = null)
        {
            var lista = new List<VentasMesero>();
            var datos = new AccesoDatos();
            try
            {
                string query = @"
                    SELECT u.Nombre, u.Apellido, COUNT(p.Id) as CantidadPedidos, SUM(p.Total) as TotalVendido
                    FROM Pedidos p
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE p.Estado = 'Cerrado'";

                if (desde != null)
                {
                    query += " AND p.FechaHora >= @desde";
                }
                if (hasta != null)
                {
                    query += " AND p.FechaHora <= @hasta";
                }

                query += @"
                    GROUP BY u.Nombre, u.Apellido
                    ORDER BY TotalVendido DESC";

                datos.setearConsulta(query);
                if (desde != null)
                {
                    datos.setearParametro("@desde", desde.Value);
                }
                if (hasta != null)
                {
                    datos.setearParametro("@hasta", hasta.Value);
                }
                datos.ejecutarLectura();
                while (datos.Lector != null && datos.Lector.Read())
                {
                    lista.Add(new VentasMesero
                    {
                        NombreMesero = $"{datos.Lector.GetString(0)} {datos.Lector.GetString(1)}",
                        CantidadPedidos = datos.Lector.GetInt32(2),
                        TotalVendido = datos.Lector.GetDecimal(3)
                    });
                }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }

    public class ProductoMasVendido
    {
        public string Nombre { get; set; }
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }

    public class VentasMesero
    {
        public string NombreMesero { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
