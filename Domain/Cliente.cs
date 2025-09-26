using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public Cliente(int id, string nombre, string telefono, string email)
        {
            Id = id;
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
        }
        public Cliente() { }
    }
}
