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
                datos.setearConsulta("SELECT Id, Numero, Ocupada, MeseroId, Estado FROM Mesas");
                datos.ejecutarLectura();

                var lector = datos.Lector;

                while (lector != null && lector.Read())
                {
                    var mesa = new Mesa
                    {
                        Id = lector.GetInt32(0),
                        Numero = lector.GetInt32(1),
                        Ocupada = lector.GetBoolean(2),
                        MeseroId = lector.IsDBNull(3) ? null : (int?)lector.GetInt32(3),
                        Estado = lector.IsDBNull(4) ? string.Empty : lector.GetString(4)
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
    }
}
}
