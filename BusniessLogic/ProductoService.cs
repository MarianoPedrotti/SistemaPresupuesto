using SistemaPresupuesto.DataAccess;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.BusniessLogic
{
    public class ProductoService
    {
        private readonly ProductoRepository _productoRepository;
        public ProductoService(string connectionString)
        {
            _productoRepository = new ProductoRepository(connectionString);
        }
        public List<Producto> productos()
        {
            return _productoRepository.getAll();
        }
        public Producto ObtenerProductoPorId(int id)
        {
            return _productoRepository.GetById(id);
        }

        public void agregarProducto(Producto producto)
        {
            _productoRepository.AddProduct(producto);
        }
        public void actualizarProducto(Producto producto)
        {
            _productoRepository.UpdateProduct(producto);
        }
        public void eliminarProducto(int id)
        {
            _productoRepository.DeleteProduct(id);
        }
    }
}
