using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Comanda
    {
        public int Id { get; set; }
        public Pedido Pedido { get; set; }
        public Usuario Chef { get; set; }
        public EstadoDetalle Estado { get; set; }
        public List<DetallePedido> Detalles { get; set; }
    }
}

