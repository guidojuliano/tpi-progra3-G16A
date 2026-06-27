using System;
using System.Web;
using dominio;

namespace tpi_progra3_G16A
{
    public static class Seguridad
    {
        public static bool SesionActiva(object userSession)
        {
            return userSession != null && userSession is Usuario;
        }

        public static bool EsGerente(object userSession)
        {
            return SesionActiva(userSession) && ((Usuario)userSession).Rol == Rol.Gerente;
        }

        public static bool EsMesero(object userSession)
        {
            return SesionActiva(userSession) && ((Usuario)userSession).Rol == Rol.Mesero;
        }

        public static bool EsCocina(object userSession)
        {
            return SesionActiva(userSession) && ((Usuario)userSession).Rol == Rol.Cocina;
        }
    }
}
