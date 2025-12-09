using SistemaPresupuesto.DataAccess;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPresupuesto.BusniessLogic
{
    public class PresupuestoService
    {
        private readonly PresupuestoRepository _presupuestoRepository;
        // Asumo que ya tienes ClienteService y ProductoService disponibles
        private readonly ClienteService _clienteService;
        private readonly ProductoService _productoService;

        public PresupuestoService(string connectionString)
        {
            _presupuestoRepository = new PresupuestoRepository(connectionString);
            _clienteService = new ClienteService(connectionString);
            _productoService = new ProductoService(connectionString);
        }
        public List<Presupuesto> ListarPresupuestos()
        {
            return _presupuestoRepository.ListarPresupuestos();
        }
        public Presupuesto ObtenerPresupuestoPorId(int id)
        {
            return _presupuestoRepository.ObtenerPresupuesto(id);
        }

        public void ActualizarPresupuesto(Presupuesto presupuesto)
        {
            // Opcional: Validaciones de negocio antes de actualizar
            if (presupuesto.Detalles == null || presupuesto.Detalles.Count == 0)
                throw new Exception("El presupuesto debe tener al menos un producto.");

            // Recalcular el total final del encabezado antes de actualizar
            presupuesto.Total = presupuesto.Detalles.Sum(d => d.Total);

            _presupuestoRepository.ActualizarPresupuesto(presupuesto);
        }

        public void EliminarPresupuesto(int id)
        {
            // Opcional: Chequeos de permisos o estado (ej: no eliminar si ya está confirmado)
            _presupuestoRepository.EliminarPresupuesto(id);
        }

        public void ConfirmarPresupuesto(int id)
        {
            // Opcional: Aquí se podría poner lógica para actualizar el stock de productos
            _presupuestoRepository.ConfirmarPresupuesto(id);
        }

        public int GuardarPresupuestoCompleto(Presupuesto presupuesto)
        {
            if (presupuesto.IdCliente <= 0)
                throw new Exception("Debe seleccionar un cliente.");

            if (presupuesto.Detalles == null || presupuesto.Detalles.Count == 0)
                throw new Exception("El presupuesto debe tener al menos un producto.");

            // Recalcular el total final del encabezado
            presupuesto.Total = presupuesto.Detalles.Sum(d => d.Total);

            return _presupuestoRepository.GuardarPresupuestoCompleto(presupuesto);
        }

        // Métodos auxiliares para el formulario
        public List<Cliente> ObtenerClientes() => _clienteService.Clientes();
        public List<Producto> ObtenerProductos() => _productoService.productos();
        public Producto ObtenerProducto(int id) => _productoService.ObtenerProductoPorId(id);
    }
}





