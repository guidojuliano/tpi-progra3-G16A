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
        public Comanda Comanda { get; set; }
        public Insumo Insumo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } = 0;
        public decimal Subtotal => Cantidad * PrecioUnitario; //es un calculo que se hace en tiempo de ejecucion (para mostrar en pantalla), no se guarda en la base de datos
    }
}
