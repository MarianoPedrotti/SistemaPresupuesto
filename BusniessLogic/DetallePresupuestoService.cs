using SistemaPresupuesto.Domain;
using SistemaPresupuesto.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SistemaPresupuesto.BusniessLogic
{
    public class DetallePresupuestoService
    {
        private readonly DetallePresupuestoRepository _detalleRepo;
        // Inyectamos el servicio de producto para obtener el nombre
        private readonly ProductoService _productoService;

        public DetallePresupuestoService(string connectionString)
        {
            _detalleRepo = new DetallePresupuestoRepository(connectionString);
            _productoService = new ProductoService(connectionString);
            // Asegúrate de que ProductoService esté implementado para obtener el nombre
        }

        // Método que usa el SP ListarPorPresupuesto para un presupuesto en particular
        public List<DetallePresupuestoView> ObtenerDetallesViewPorPresupuesto(int idPresupuesto)
        {
            var detalles = _detalleRepo.ObtenerDetallesPorIdPresupuesto(idPresupuesto);

            // Mapeo a la clase View para obtener el nombre del producto
            var detallesView = detalles.Select(d =>
            {
                // Obtenemos el nombre del producto
                string nombreProducto = _productoService.ObtenerProductoPorId(d.IdProducto)?.Nombre ?? "Producto Desconocido";

                return new DetallePresupuestoView
                {
                    IdDetalle = d.Id,
                    IdPresupuesto = d.IdPresupuesto,
                    NombreProducto = nombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    // Usamos la propiedad calculada de C# (Total)
                    Total = d.Total
                };
            }).ToList();

            return detallesView;
        }
    }

    // Clase auxiliar para la vista (Si aún no la creaste, hazlo en BusniessLogic o Domain)
    public class DetallePresupuestoView
    {
        public int IdDetalle { get; set; }
        public int IdPresupuesto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}