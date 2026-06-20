using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class PedidoNegocio
    {
        public Pedido ObtenerPedidoAbiertoPorMesa(int idMesa)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT p.Id, p.MesaId, p.MeseroId, p.FechaHora, p.Estado, p.Total,
                           u.Nombre, u.Apellido, u.Email, u.Password, u.Rol, u.Activo
                    FROM Pedidos p
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE p.MesaId = @MesaId AND p.Estado = 'Abierto'");
                datos.setearParametro("@MesaId", idMesa);
                datos.ejecutarLectura();

                var lector = datos.Lector;
                if (lector != null && lector.Read())
                {
                    var mesaNegocio = new MesaNegocio();
                    var mesa = mesaNegocio.ObtenerMesaPorId(idMesa);

                    return new Pedido
                    {
                        Id = lector.GetInt32(0),
                        Mesa = mesa,
                        Mesero = new Usuario
                        {
                            Id = lector.GetInt32(2),
                            Nombre = lector.GetString(6),
                            Apellido = lector.GetString(7),
                            Email = lector.GetString(8),
                            Password = lector.GetString(9),
                            Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(10)),
                            Activo = lector.GetBoolean(11)
                        },
                        FechaHora = lector.GetDateTime(3),
                        Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(4)),
                        Total = lector.GetDecimal(5)
                    };
                }
                return null;
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

        public void AbrirPedido(int idMesa, int idMesero)
        {
            var pedidoExistente = ObtenerPedidoAbiertoPorMesa(idMesa);
            if (pedidoExistente != null)
                throw new InvalidOperationException("La mesa ya tiene un pedido abierto.");

            var datos = new AccesoDatos();
            try
            {
                // 1. Insertar el Pedido
                datos.setearConsulta(@"
                    INSERT INTO Pedidos (MesaId, MeseroId, FechaHora, Estado, Total)
                    VALUES (@MesaId, @MeseroId, @FechaHora, 'Abierto', 0.00)");
                datos.setearParametro("@MesaId", idMesa);
                datos.setearParametro("@MeseroId", idMesero);
                datos.setearParametro("@FechaHora", DateTime.Now);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 2. Actualizar el estado de la Mesa
                datos = new AccesoDatos();
                datos.setearConsulta(@"
                    UPDATE Mesas
                    SET Estado = 'Ocupada', Ocupada = 1, MeseroId = @MeseroId
                    WHERE Id = @MesaId");
                datos.setearParametro("@MesaId", idMesa);
                datos.setearParametro("@MeseroId", idMesero);
                datos.ejecutarAccion();
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

        public void RegistrarComanda(int idPedido, List<DetallePedido> detalles, string observaciones)
        {
            if (detalles == null || detalles.Count == 0)
                throw new ArgumentException("La comanda debe tener al menos un detalle.");

            var insumoNegocio = new InsumoNegocio();
            foreach (var det in detalles)
            {
                var insumo = insumoNegocio.ObtenerInsumoPorId(det.Insumo.Id);
                if (insumo == null)
                    throw new InvalidOperationException($"El insumo con ID {det.Insumo.Id} no existe.");
                if (insumo.Stock < det.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para {insumo.Nombre}. Disponible: {insumo.Stock}, Solicitado: {det.Cantidad}");
            }

            var datos = new AccesoDatos();
            int comandaId = 0;
            try
            {
                // 1. Insertar Comanda y obtener su ID
                datos.setearConsulta(@"
                    INSERT INTO Comandas (PedidoId, Estado, FechaHora, Observaciones)
                    OUTPUT INSERTED.Id
                    VALUES (@PedidoId, 'Pendiente', @FechaHora, @Observaciones)");
                datos.setearParametro("@PedidoId", idPedido);
                datos.setearParametro("@FechaHora", DateTime.Now);
                datos.setearParametro("@Observaciones", string.IsNullOrEmpty(observaciones) ? (object)DBNull.Value : observaciones);
                datos.ejecutarLectura();

                if (datos.Lector != null && datos.Lector.Read())
                {
                    comandaId = datos.Lector.GetInt32(0);
                }
                datos.cerrarConexion();

                if (comandaId == 0)
                    throw new Exception("No se pudo obtener el ID de la Comanda insertada.");

                // 2. Insertar cada Detalle y actualizar Stock
                foreach (var det in detalles)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta(@"
                        INSERT INTO DetallesPedidos (ComandaId, InsumoId, Cantidad, PrecioUnitario)
                        VALUES (@ComandaId, @InsumoId, @Cantidad, @PrecioUnitario)");
                    datos.setearParametro("@ComandaId", comandaId);
                    datos.setearParametro("@InsumoId", det.Insumo.Id);
                    datos.setearParametro("@Cantidad", det.Cantidad);
                    datos.setearParametro("@PrecioUnitario", det.PrecioUnitario);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();

                    datos = new AccesoDatos();
                    datos.setearConsulta(@"
                        UPDATE Insumos
                        SET Stock = Stock - @Cantidad
                        WHERE Id = @InsumoId");
                    datos.setearParametro("@Cantidad", det.Cantidad);
                    datos.setearParametro("@InsumoId", det.Insumo.Id);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();
                }
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

        public decimal CerrarYCobrarPedido(int idPedido)
        {
            var datos = new AccesoDatos();
            decimal total = 0;
            int mesaId = 0;

            try
            {
                // 1. Obtener MesaId y calcular total de detalles
                datos.setearConsulta(@"
                    SELECT p.MesaId, 
                           ISNULL((SELECT SUM(dp.Cantidad * dp.PrecioUnitario) 
                                   FROM DetallesPedidos dp 
                                   INNER JOIN Comandas c ON dp.ComandaId = c.Id 
                                   WHERE c.PedidoId = p.Id), 0.00) AS TotalCalculado
                    FROM Pedidos p
                    WHERE p.Id = @PedidoId");
                datos.setearParametro("@PedidoId", idPedido);
                datos.ejecutarLectura();

                if (datos.Lector != null && datos.Lector.Read())
                {
                    mesaId = datos.Lector.GetInt32(0);
                    total = datos.Lector.GetDecimal(1);
                }
                datos.cerrarConexion();

                if (mesaId == 0)
                    throw new InvalidOperationException("No se encontró el pedido a cerrar.");

                // 2. Actualizar el Pedido
                datos = new AccesoDatos();
                datos.setearConsulta(@"
                    UPDATE Pedidos
                    SET Estado = 'Cerrado', Total = @Total
                    WHERE Id = @PedidoId");
                datos.setearParametro("@Total", total);
                datos.setearParametro("@PedidoId", idPedido);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 3. Liberar la Mesa
                datos = new AccesoDatos();
                datos.setearConsulta(@"
                    UPDATE Mesas
                    SET Estado = 'Libre', Ocupada = 0, MeseroId = NULL
                    WHERE Id = @MesaId");
                datos.setearParametro("@MesaId", mesaId);
                datos.ejecutarAccion();

                return total;
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

        public void ActualizarEstadoComanda(int idComanda, EstadoDetalle nuevoEstado)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    UPDATE Comandas
                    SET Estado = @Estado
                    WHERE Id = @Id");
                datos.setearParametro("@Estado", nuevoEstado.ToString());
                datos.setearParametro("@Id", idComanda);
                datos.ejecutarAccion();
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

        public Pedido ObtenerPedidoPorId(int idPedido)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT p.Id, p.MesaId, p.MeseroId, p.FechaHora, p.Estado, p.Total,
                           u.Nombre, u.Apellido, u.Email, u.Password, u.Rol, u.Activo
                    FROM Pedidos p
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE p.Id = @PedidoId");
                datos.setearParametro("@PedidoId", idPedido);
                datos.ejecutarLectura();

                var lector = datos.Lector;
                if (lector != null && lector.Read())
                {
                    var mesaNegocio = new MesaNegocio();
                    var mesa = mesaNegocio.ObtenerMesaPorId(lector.GetInt32(1));

                    return new Pedido
                    {
                        Id = lector.GetInt32(0),
                        Mesa = mesa,
                        Mesero = new Usuario
                        {
                            Id = lector.GetInt32(2),
                            Nombre = lector.GetString(6),
                            Apellido = lector.GetString(7),
                            Email = lector.GetString(8),
                            Password = lector.GetString(9),
                            Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(10)),
                            Activo = lector.GetBoolean(11)
                        },
                        FechaHora = lector.GetDateTime(3),
                        Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(4)),
                        Total = lector.GetDecimal(5)
                    };
                }
                return null;
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

        public List<Comanda> ObtenerComandasPorEstado(EstadoDetalle estado)
        {
            var lista = new List<Comanda>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT c.Id, c.PedidoId, c.Estado, c.FechaHora, c.Observaciones,
                           p.MesaId, p.MeseroId, p.FechaHora AS PedidoFechaHora, p.Estado AS PedidoEstado, p.Total AS PedidoTotal,
                           u.Nombre, u.Apellido, u.Email, u.Rol, u.Activo
                    FROM Comandas c
                    INNER JOIN Pedidos p ON c.PedidoId = p.Id
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE c.Estado = @Estado");
                datos.setearParametro("@Estado", estado.ToString());
                datos.ejecutarLectura();

                var lector = datos.Lector;
                var mesaNegocio = new MesaNegocio();
                while (lector != null && lector.Read())
                {
                    var mesa = mesaNegocio.ObtenerMesaPorId(lector.GetInt32(5));
                    var comanda = new Comanda
                    {
                        Id = lector.GetInt32(0),
                        Estado = (EstadoDetalle)Enum.Parse(typeof(EstadoDetalle), lector.GetString(2)),
                        FechaHora = lector.GetDateTime(3),
                        Observaciones = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                        Pedido = new Pedido
                        {
                            Id = lector.GetInt32(1),
                            Mesa = mesa,
                            Mesero = new Usuario
                            {
                                Id = lector.GetInt32(6),
                                Nombre = lector.GetString(10),
                                Apellido = lector.GetString(11),
                                Email = lector.GetString(12),
                                Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(13)),
                                Activo = lector.GetBoolean(14)
                            },
                            FechaHora = lector.GetDateTime(7),
                            Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(8)),
                            Total = lector.GetDecimal(9)
                        }
                    };
                    lista.Add(comanda);
                }
                datos.cerrarConexion();

                foreach (var comanda in lista)
                {
                    comanda.Detalles = ObtenerDetallesPorComanda(comanda.Id);
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

        public List<Comanda> ObtenerComandasActivas()
        {
            var lista = new List<Comanda>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT c.Id, c.PedidoId, c.Estado, c.FechaHora, c.Observaciones,
                           p.MesaId, p.MeseroId, p.FechaHora AS PedidoFechaHora, p.Estado AS PedidoEstado, p.Total AS PedidoTotal,
                           u.Nombre, u.Apellido, u.Email, u.Rol, u.Activo
                    FROM Comandas c
                    INNER JOIN Pedidos p ON c.PedidoId = p.Id
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE c.Estado IN ('Pendiente', 'EnPreparacion')");
                datos.ejecutarLectura();

                var lector = datos.Lector;
                var mesaNegocio = new MesaNegocio();
                while (lector != null && lector.Read())
                {
                    var mesa = mesaNegocio.ObtenerMesaPorId(lector.GetInt32(5));
                    var comanda = new Comanda
                    {
                        Id = lector.GetInt32(0),
                        Estado = (EstadoDetalle)Enum.Parse(typeof(EstadoDetalle), lector.GetString(2)),
                        FechaHora = lector.GetDateTime(3),
                        Observaciones = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                        Pedido = new Pedido
                        {
                            Id = lector.GetInt32(1),
                            Mesa = mesa,
                            Mesero = new Usuario
                            {
                                Id = lector.GetInt32(6),
                                Nombre = lector.GetString(10),
                                Apellido = lector.GetString(11),
                                Email = lector.GetString(12),
                                Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(13)),
                                Activo = lector.GetBoolean(14)
                            },
                            FechaHora = lector.GetDateTime(7),
                            Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(8)),
                            Total = lector.GetDecimal(9)
                        }
                    };
                    lista.Add(comanda);
                }
                datos.cerrarConexion();

                foreach (var comanda in lista)
                {
                    comanda.Detalles = ObtenerDetallesPorComanda(comanda.Id);
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

        public List<Comanda> ObtenerComandasPorPedido(int idPedido)
        {
            var lista = new List<Comanda>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT c.Id, c.PedidoId, c.Estado, c.FechaHora, c.Observaciones,
                        p.MesaId, p.MeseroId, p.FechaHora AS PedidoFechaHora, p.Estado AS PedidoEstado, p.Total AS PedidoTotal,
                        u.Nombre, u.Apellido, u.Email, u.Rol, u.Activo
                    FROM Comandas c
                    INNER JOIN Pedidos p ON c.PedidoId = p.Id  
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE c.PedidoId = @PedidoId");
                datos.setearParametro("@PedidoId", idPedido);
                datos.ejecutarLectura();

                var lector = datos.Lector;
                var mesaNegocio = new MesaNegocio();
                while(lector != null && lector.Read())
                {
                    var mesa = mesaNegocio.ObtenerMesaPorId(lector.GetInt32(5));
                    var comanda = new Comanda
                    {
                        Id = lector.GetInt32(0),
                        Estado = (EstadoDetalle)Enum.Parse(typeof(EstadoDetalle), lector.GetString(2)),
                        FechaHora = lector.GetDateTime(3),
                        Observaciones = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                        Pedido = new Pedido
                        {
                            Id = lector.GetInt32(1),
                            Mesa = mesa,
                            Mesero = new Usuario
                            {
                                Id = lector.GetInt32(6),
                                Nombre = lector.GetString(10),
                                Apellido = lector.GetString(11),
                                Email = lector.GetString(12),
                                Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(13)),
                                Activo = lector.GetBoolean(14)
                            },
                            FechaHora = lector.GetDateTime(7),
                            Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(8)),
                            Total = lector.GetDecimal(9)
                        }
                    };
                    lista.Add(comanda);
                }
                datos.cerrarConexion();

                foreach (var comanda in lista)
                {
                    comanda.Detalles = ObtenerDetallesPorComanda(comanda.Id);
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

        public Comanda ObtenerComandaPorId(int idComanda)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT c.Id, c.PedidoId, c.Estado, c.FechaHora, c.Observaciones,
                           p.MesaId, p.MeseroId, p.FechaHora AS PedidoFechaHora, p.Estado AS PedidoEstado, p.Total AS PedidoTotal,
                           u.Nombre, u.Apellido, u.Email, u.Rol, u.Activo
                    FROM Comandas c
                    INNER JOIN Pedidos p ON c.PedidoId = p.Id
                    INNER JOIN Usuarios u ON p.MeseroId = u.Id
                    WHERE c.Id = @Id");
                datos.setearParametro("@Id", idComanda);
                datos.ejecutarLectura();

                var lector = datos.Lector;
                if (lector != null && lector.Read())
                {
                    var mesaNegocio = new MesaNegocio();
                    var mesa = mesaNegocio.ObtenerMesaPorId(lector.GetInt32(5));
                    var comanda = new Comanda
                    {
                        Id = lector.GetInt32(0),
                        Estado = (EstadoDetalle)Enum.Parse(typeof(EstadoDetalle), lector.GetString(2)),
                        FechaHora = lector.GetDateTime(3),
                        Observaciones = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                        Pedido = new Pedido
                        {
                            Id = lector.GetInt32(1),
                            Mesa = mesa,
                            Mesero = new Usuario
                            {
                                Id = lector.GetInt32(6),
                                Nombre = lector.GetString(10),
                                Apellido = lector.GetString(11),
                                Email = lector.GetString(12),
                                Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(13)),
                                Activo = lector.GetBoolean(14)
                            },
                            FechaHora = lector.GetDateTime(7),
                            Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), lector.GetString(8)),
                            Total = lector.GetDecimal(9)
                        }
                    };
                    datos.cerrarConexion();

                    comanda.Detalles = ObtenerDetallesPorComanda(comanda.Id);
                    return comanda;
                }
                return null;
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

        public List<DetallePedido> ObtenerDetallesPorComanda(int idComanda)
        {
            var lista = new List<DetallePedido>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    SELECT dp.Id, dp.ComandaId, dp.InsumoId, dp.Cantidad, dp.PrecioUnitario,
                           i.Nombre, i.Precio, i.Stock, i.Tipo, i.Activo
                    FROM DetallesPedidos dp
                    INNER JOIN Insumos i ON dp.InsumoId = i.Id
                    WHERE dp.ComandaId = @ComandaId");
                datos.setearParametro("@ComandaId", idComanda);
                datos.ejecutarLectura();

                var lector = datos.Lector;
                while (lector != null && lector.Read())
                {
                    var detalle = new DetallePedido
                    {
                        Id = lector.GetInt32(0),
                        Cantidad = lector.GetInt32(3),
                        PrecioUnitario = lector.GetDecimal(4),
                        Insumo = new Insumo
                        {
                            Id = lector.GetInt32(2),
                            Nombre = lector.GetString(5),
                            Precio = lector.GetDecimal(6),
                            Stock = lector.GetInt32(7),
                            Tipo = (TipoInsumo)Enum.Parse(typeof(TipoInsumo), lector.GetString(8)),
                            Activo = lector.GetBoolean(9)
                        }
                    };
                    lista.Add(detalle);
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
}
