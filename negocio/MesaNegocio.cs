using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Configuration;

namespace negocio
{
    public class MesaNegocio
    {
        public List<Mesa> ObtenerMesas()
        {
            var lista = new List<Mesa>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT m.Id, m.Numero, m.Estado, m.MeseroId, u.Nombre, u.Apellido 
                    FROM Mesas m 
                    LEFT JOIN Usuarios u ON m.MeseroId = u.Id");
                datos.ejecutarLectura();

                var lector = datos.Lector;

                while (lector != null && lector.Read())
                {
                    var mesa = new Mesa
                    {
                        Id = lector.GetInt32(0),
                        Numero = lector.GetInt32(1),
                        Estado = lector.IsDBNull(2) ? EstadoMesa.Libre : (EstadoMesa)Enum.Parse(typeof(EstadoMesa), lector.GetString(2)),
                        Mesero = lector.IsDBNull(3) ? null : new Usuario
                        {
                            Id = lector.GetInt32(3),
                            Nombre = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                            Apellido = lector.IsDBNull(5) ? string.Empty : lector.GetString(5)
                        }
                    };
                    lista.Add(mesa);
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

        public Mesa ObtenerMesaPorId(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT m.Id, m.Numero, m.Estado, m.MeseroId, u.Nombre, u.Apellido 
                    FROM Mesas m 
                    LEFT JOIN Usuarios u ON m.MeseroId = u.Id 
                    WHERE m.Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                var lector = datos.Lector;

                if (lector != null && lector.Read())
                {
                    return new Mesa
                    {
                        Id = lector.GetInt32(0),
                        Numero = lector.GetInt32(1),
                        Estado = lector.IsDBNull(2) ? EstadoMesa.Libre : (EstadoMesa)Enum.Parse(typeof(EstadoMesa), lector.GetString(2)),
                        Mesero = lector.IsDBNull(3) ? null : new Usuario
                        {
                            Id = lector.GetInt32(3),
                            Nombre = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                            Apellido = lector.IsDBNull(5) ? string.Empty : lector.GetString(5)
                        }
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

        public void AgregarMesa(Mesa nueva)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Mesas (Numero, Ocupada, MeseroId, Estado) 
                    VALUES (@Numero, @Ocupada, @MeseroId, @Estado)");
                
                datos.setearParametro("@Numero", nueva.Numero);
                datos.setearParametro("@Ocupada", nueva.Estado == EstadoMesa.Ocupada ? 1 : 0);
                datos.setearParametro("@MeseroId", nueva.Mesero != null ? (object)nueva.Mesero.Id : DBNull.Value);
                datos.setearParametro("@Estado", nueva.Estado.ToString());

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

        public void ModificarMesa(Mesa mesa)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    UPDATE Mesas 
                    SET Numero = @Numero, Ocupada = @Ocupada, MeseroId = @MeseroId, Estado = @Estado 
                    WHERE Id = @Id");
                
                datos.setearParametro("@Numero", mesa.Numero);
                datos.setearParametro("@Ocupada", mesa.Estado == EstadoMesa.Ocupada ? 1 : 0);
                datos.setearParametro("@MeseroId", mesa.Mesero != null ? (object)mesa.Mesero.Id : DBNull.Value);
                datos.setearParametro("@Estado", mesa.Estado.ToString());
                datos.setearParametro("@Id", mesa.Id);

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

        public void EliminarMesa(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM Mesas WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                
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
    }
}

