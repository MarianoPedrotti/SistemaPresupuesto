using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPresupuesto.DataAccess
{
    public class DetallePresupuestoRepository
    {
        private readonly string _connectionString;

        public DetallePresupuestoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método auxiliar para mapear el DataReader a la clase de Dominio
        private DetallePresupuesto MapearDetalle(SqlDataReader reader)
        {
            return new DetallePresupuesto
            {
                // ATENCIÓN: Usamos los nombres de columna exactos de tu base de datos
                Id = Convert.ToInt32(reader["IdPresupuestoDetalle"]),
                IdPresupuesto = Convert.ToInt32(reader["IdPresupuesto"]),
                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                // Nota: Tu clase de dominio calcula Total, pero si el SP lo devuelve, 
                // podríamos usarlo, o dejar que la propiedad calculada de C# lo haga.
                // Como tu SP devuelve SubTotal, lo mapearemos si es necesario.
                // Si la columna SubTotal es calculada en SQL, la leeremos aquí (aunque es redundante).
                // Total se calcula en el dominio: public decimal Total => Cantidad * PrecioUnitario;
                // Si la columna SubTotal NO es calculada en SQL, tendríamos un problema. 
                // Asumiremos que el SP devuelve solo los campos que acepta la clase de dominio.
            };
        }

        public List<DetallePresupuesto> ObtenerDetallesPorIdPresupuesto(int idPresupuesto)
        {
            List<DetallePresupuesto> detalles = new List<DetallePresupuesto>();
            string spName = "SP_DetallePresupuesto_ListarPorPresupuesto";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(MapearDetalle(reader));
                        }
                    }
                }
            }
            return detalles;
        }

        // *** Método para listar todos los detalles (Necesitarás un SP adicional para esto) ***
        /*
        public List<DetallePresupuesto> ListarTodos() 
        {
            // Necesitas crear el SP [dbo].[SP_DetallePresupuesto_ListarTodos] en SQL 
            // Si no lo haces, este método no funcionará.
        }
        */

        // Los métodos de Insertar, Actualizar y Eliminar se utilizarán dentro de PresupuestoRepository.
        // Pero el repositorio de detalles debe tenerlos si se desea usarlos directamente.
    }
}