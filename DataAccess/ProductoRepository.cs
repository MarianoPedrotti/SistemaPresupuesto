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
    public class ProductoRepository
    {
        private readonly string _connectionString;
        public ProductoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Query para obtener todos los productos
        public List<Producto> getAll()
        {
            var productos = new List<Producto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("sp_getAllProductos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Producto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                            };
                            productos.Add(producto);
                        }
                    }
                    //Sin usar stored procedure
                    //string query = "SELECT IdProducto, Nombre, Precio, Stock From Productos";
                    //using (SqlDataReader reader = cmd.ExecuteReader()) 
                    //{
                    //    while (reader.Read()) 
                    //    {
                    //        var producto = new Producto
                    //        {
                    //            Id = reader.GetInt32(0),
                    //            Nombre = reader.GetString(1),
                    //            Precio = reader.GetDecimal(2),
                    //            Stock = reader.GetInt32(3)
                    //        };
                    //        productos.Add(producto);
                    //    }
                    //}
                }
            }
            return productos;
        }

        //Query para obtener un producto por su Id
        public Producto GetById(int id)
        {
            Producto producto = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("sp_GetProductoById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                            };
                        }
                    }
                }
            }

            return producto;
            //sin usar stored procedure
            //using (SqlConnection conn = new SqlConnection(_connectionString)) 
            //{
            //    conn.Open();
            //    string query = "SELECT IdProducto, Nombre, Precio, Stock FROM Productos WHERE IdProducto = @Id";
            //    using (SqlCommand cmd = new SqlCommand(query, conn)) 
            //    {
            //        cmd.Parameters.AddWithValue("@Id", id);
            //        using (SqlDataReader reader = cmd.ExecuteReader()) 
            //        {
            //            if (reader.Read()) 
            //            {
            //                producto = new Producto
            //                {
            //                    Id = reader.GetInt32(0),
            //                    Nombre = reader.GetString(1),
            //                    Precio = reader.GetDecimal(2),
            //                    Stock = reader.GetInt32(3)
            //                };
            //            }
            //        }
            //    }
            //}
            //return producto;
        }

        //Query para insertar un nuevo producto 
        public void AddProduct(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("SP_Insertproducto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            //sin usar stored procedure
            //    using (SqlConnection conn = new SqlConnection(_connectionString)) 
            //    {
            //        conn.Open();
            //        string query = "INSERT INTO Productos (Nombre, Precio, Stock) VALUES (@Nombre, @Precio, @Stock)";
            //        using (SqlCommand cmd = new SqlCommand(query, conn)) 
            //        {
            //            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            //            cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            //            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            //            cmd.ExecuteNonQuery();
            //        }
            //    }
        }
        //Query para actualizar un producto existente
        public void UpdateProduct(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("SP_UpdateProducto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            //sin usar stored procedure
            //using (SqlConnection conn = new SqlConnection(_connectionString)) 
            //{
            //    conn.Open();
            //    string query = "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, Stock = @Stock WHERE IdProducto = @Id";
            //    using (SqlCommand cmd = new SqlCommand(query, conn)) 
            //    {
            //        cmd.Parameters.AddWithValue("@IdProducto", producto.Id);
            //        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            //        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
            //        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }

        //Query para eliminar un producto por su Id
        public void DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //Usando stored procedure
                using (SqlCommand cmd = new SqlCommand("SP_DeleteProducto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            //sin usar stored procedure
            //using (SqlConnection conn = new SqlConnection(_connectionString)) 
            //{
            //    conn.Open();
            //    string query = "DELETE FROM Productos WHERE IdProducto = @Id";
            //    using (SqlCommand cmd = new SqlCommand(query, conn)) 
            //    {
            //        cmd.Parameters.AddWithValue("@Id", id);
            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }

    }
}
