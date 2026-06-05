using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Pedido
    {
        public int Id { get; set; }
        public int MesaId { get; set; }
        public int MeseroId { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoPedido Estado { get; set; }
        public decimal Total { get; set; }
    }
}
