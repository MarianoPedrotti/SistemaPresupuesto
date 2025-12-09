using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.Domain
{
    public class Presupuesto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public Cliente Cliente { get; set; }
        public List<DetallePresupuesto> Detalles { get; set; }
        public bool Confirmado { get; set; }
        public Presupuesto(int id, int idCliente, DateTime fecha, decimal total, List<DetallePresupuesto> detalles, bool confirmado)
        {
            Id = id;
            IdCliente = idCliente;
            Fecha = fecha;
            Total = total;
            Detalles = detalles;
            Confirmado = confirmado;
        }
        public Presupuesto() { }
    }
}
