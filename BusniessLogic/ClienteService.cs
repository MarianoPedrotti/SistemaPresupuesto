using SistemaPresupuesto.DataAccess;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.BusniessLogic
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;
        public ClienteService(string connectionString)
        {
            _clienteRepository = new ClienteRepository(connectionString);
        }
        public List<Cliente> Clientes()
        {
            return _clienteRepository.getAll();
        }
        public Cliente GetCliente(int id)
        {
            return _clienteRepository.GetClienteById(id);
        }
        public void addCliente(Cliente cliente)
        {
            _clienteRepository.AddCliente(cliente);
        }
        public void deleteCliente(int id)
        {
            {
                _clienteRepository.DeleteCliente(id);
            }
        }
        public void actualiarCliente(Cliente cliente)
        {
            _clienteRepository.UpdateCliente(cliente);
        }
    }
}
