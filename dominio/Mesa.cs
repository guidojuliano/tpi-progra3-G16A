using System;

namespace dominio
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public bool Ocupada { get; set; }
        public int? MeseroId { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
