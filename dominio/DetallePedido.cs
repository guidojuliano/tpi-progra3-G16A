using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public EstadoDetalle Estado { get; set; }
        public int? ChefId { get; set; } //sin asignar
        public string NombreChef { get; set; } //para mostrar el nombre del chef asignado
    }
}
