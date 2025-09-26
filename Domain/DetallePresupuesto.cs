using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.Domain
{
    public class DetallePresupuesto
    {
        public int Id { get; set; }
        public int IdPresupuesto { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total => Cantidad * PrecioUnitario;
        public Producto Producto { get; set; }
        public Presupuesto Presupuesto { get; set; }
        public DetallePresupuesto(int id, int presupuestoId, int productoId, int cantidad, decimal precioUnitario)
        {
            Id = id;
            IdPresupuesto = presupuestoId;
            IdProducto = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
        public DetallePresupuesto() { }
    }
}
