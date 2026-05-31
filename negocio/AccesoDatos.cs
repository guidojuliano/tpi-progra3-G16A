using System;
using System.Data.SqlClient;
using System.Configuration;

namespace negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader? lector;
        public SqlDataReader? Lector => lector;

        public AccesoDatos()
        {
            //esto es para que no se rompa el proyecto al no tener la cadena de conexión en Web.config

            var cs = ConfigurationManager.ConnectionStrings["RestoBarDb"]?.ConnectionString;
            if (string.IsNullOrEmpty(cs))
                throw new InvalidOperationException("Falta la cadena de conexión 'RestoBarDb' en Web.config");

            conexion = new SqlConnection(cs);
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
            comando.Parameters.Clear();
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }
    }
}
