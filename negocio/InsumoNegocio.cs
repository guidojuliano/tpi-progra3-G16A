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
    public class InsumoNegocio
    {
        public List<Insumo> ObtenerInsumos()
        {
            var lista = new List<Insumo>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Precio, Stock, Tipo, Activo FROM Insumos");
                datos.ejecutarLectura();

                var lector = datos.Lector;

                while (lector != null && lector.Read())
                {
                    var insumo = new Insumo
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Precio = lector.GetDecimal(2),
                        Stock = lector.GetInt32(3),
                        Tipo = (TipoInsumo)Enum.Parse(typeof(TipoInsumo), lector.GetString(4)),
                        Activo = lector.GetBoolean(5)
                    };
                    lista.Add(insumo);
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

        public Insumo ObtenerInsumoPorId(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Precio, Stock, Tipo, Activo FROM Insumos WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                var lector = datos.Lector;

                if (lector != null && lector.Read())
                {
                    return new Insumo
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Precio = lector.GetDecimal(2),
                        Stock = lector.GetInt32(3),
                        Tipo = (TipoInsumo)Enum.Parse(typeof(TipoInsumo), lector.GetString(4)),
                        Activo = lector.GetBoolean(5)
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

        public void AgregarInsumo(Insumo nuevo)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Insumos (Nombre, Precio, Stock, Tipo, Activo) 
                    VALUES (@Nombre, @Precio, @Stock, @Tipo, 1)");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Precio", nuevo.Precio);
                datos.setearParametro("@Stock", nuevo.Stock);
                datos.setearParametro("@Tipo", nuevo.Tipo.ToString());

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

        public void ModificarInsumo(Insumo insumo)
        {
            if (insumo.Stock < 0)
                throw new InvalidOperationException("El stock no puede ser negativo.");

            if (insumo.Stock == 0)
                insumo.Activo = false;
            

            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    UPDATE Insumos 
                    SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock, Tipo = @Tipo, Activo = @Activo 
                    WHERE Id = @Id");

                datos.setearParametro("@Nombre", insumo.Nombre);
                datos.setearParametro("@Precio", insumo.Precio);
                datos.setearParametro("@Stock", insumo.Stock);
                datos.setearParametro("@Tipo", insumo.Tipo.ToString());
                datos.setearParametro("@Activo", insumo.Activo ? 1 : 0);
                datos.setearParametro("@Id", insumo.Id);

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

        public void EliminarInsumo(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Insumos SET Activo = 0 WHERE Id = @Id");
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
