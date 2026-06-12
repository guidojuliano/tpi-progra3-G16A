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
            // Aquí puedes implementar la lógica para obtener los usuarios desde la base de datos
            // Por ejemplo, podrías usar ADO.NET para consultar la tabla de usuarios y devolver una lista de objetos Usuario
            // Este es un ejemplo simplificado que devuelve una lista vacía para fines de demostración    
            return new List<Usuario>();
        }





        public bool ValidarUsuario(string nombreUsuario, string contraseña)
        {
            // Aquí puedes implementar la lógica para validar el usuario contra la base de datos
            // Por ejemplo, podrías usar ADO.NET para consultar la tabla de usuarios y verificar las credenciales
            // Este es un ejemplo simplificado que siempre devuelve true para fines de demostración    
            return true;
        }
    
    
    
    
    }



}
