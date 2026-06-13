using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
	public enum Rol
	{
		Gerente,
		Mesero,
		Cocina
	}

	public enum EstadoMesa
    {
        Libre,
        Ocupada,
        NoDisponible
    }

	public enum EstadoPedido
	{
		Abierto,
		Cerrado
	}

	public enum EstadoDetalle
	{
		Pendiente,
		EnPreparacion,
		Listo,
		Entregado
	}

    public enum TipoInsumo
    {
        Plato,
        Bebida
    }
}
