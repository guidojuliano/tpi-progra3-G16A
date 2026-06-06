using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public EstadoMesa Estado { get; set; }
        public Usuario Mesero { get; set; } //sin asignar
    }
}
