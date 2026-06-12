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
    public class UsuarioNegocio
    {
        public List<Usuario> ObtenerUsuarios()
        {
            var lista = new List<Usuario>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Apellido, Email, Password, Rol, Activo FROM Usuarios WHERE Activo = 1");
                datos.ejecutarLectura();

                var lector = datos.Lector;

                while (lector != null && lector.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Apellido = lector.GetString(2),
                        Email = lector.GetString(3),
                        Password = lector.GetString(4),
                        Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(5)),
                        Activo = lector.GetBoolean(6)
                    };
                    lista.Add(usuario);
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

        public Usuario ObtenerUsuarioPorId(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Apellido, Email, Password, Rol, Activo FROM Usuarios WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                var lector = datos.Lector;

                if (lector != null && lector.Read())
                {
                    return new Usuario
                    {
                        Id = lector.GetInt32(0),
                        Nombre = lector.GetString(1),
                        Apellido = lector.GetString(2),
                        Email = lector.GetString(3),
                        Password = lector.GetString(4),
                        Rol = (Rol)Enum.Parse(typeof(Rol), lector.GetString(5)),
                        Activo = lector.GetBoolean(6)
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

        public void AgregarUsuario(Usuario nuevo)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Usuarios (Nombre, Apellido, Email, Password, Rol, Activo) 
                    VALUES (@Nombre, @Apellido, @Email, @Password, @Rol, 1)");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Password", nuevo.Password);
                datos.setearParametro("@Rol", nuevo.Rol.ToString());

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

        public void ModificarUsuario(Usuario usuario)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    UPDATE Usuarios 
                    SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Password = @Password, Rol = @Rol, Activo = @Activo 
                    WHERE Id = @Id");

                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@Password", usuario.Password);
                datos.setearParametro("@Rol", usuario.Rol.ToString());
                datos.setearParametro("@Activo", usuario.Activo ? 1 : 0);
                datos.setearParametro("@Id", usuario.Id);

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

        public void EliminarUsuario(int id)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Usuarios SET Activo = 0 WHERE Id = @Id");
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

        public bool ValidarUsuario(string email, string password)
        {
            var datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id FROM Usuarios WHERE Email = @Email AND Password = @Password AND Activo = 1");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);
                datos.ejecutarLectura();

                var lector = datos.Lector;

                if (lector != null && lector.Read())
                {
                    return true;
                }

                return false;
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
