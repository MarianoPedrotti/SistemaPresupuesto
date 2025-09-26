using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPresupuesto.DataAccess
{
    public class ClienteRepository
    {
        private readonly string _connectionString;
        public ClienteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //query para obtener todos los clientes
        public List<Cliente> getAll()
        {
            var clientes = new List<Cliente>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("sp_ListarClientes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IDCliente")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            };
                            clientes.Add(cliente);
                        }
                    }
                    //Sin usar stored procedure
                    //string query = "SELECT IdCliente, Nombre, Apellido, Email From Clientes";
                    //using (SqlDataReader reader = cmd.ExecuteReader()) 
                    //{
                    //    while (reader.Read()) 
                    //    {
                    //        var cliente = new Cliente
                    //        {
                    //            Id = reader.GetInt32(0),
                    //            Nombre = reader.GetString(1),
                    //            Apellido = reader.GetString(2),
                    //            Email = reader.GetString(3)
                    //        };
                    //        clientes.Add(cliente);
                    //    }
                    //}
                }
            }
            return clientes;
        }
        //Query para obtener un cliente por ID
        public Cliente GetClienteById(int id)
        {
            Cliente cliente = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerClienteById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDCliente", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IDCliente")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                            };
                        }
                    }
                }
            }
            return cliente;
        }
        //Query para agregar un nuevo cliente
        public void AddCliente(Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AgregarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //Query para actualizar un cliente
        public void UpdateCliente(Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDCliente", cliente.Id);
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //Query para eliminar un cliente
        public void DeleteCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDCliente", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }        
}
