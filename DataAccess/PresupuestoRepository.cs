using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace SistemaPresupuesto.DataAccess
{
    public class PresupuestoRepository
    {
        private readonly string _connectionString;

        public PresupuestoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // --- Mapeo de Datos Común ---
        private Presupuesto MapearPresupuesto(SqlDataReader reader)
        {
            return new Presupuesto
            {
                Id = Convert.ToInt32(reader["IdPresupuesto"]),
                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                Fecha = Convert.ToDateTime(reader["Fecha"]),
                Total = Convert.ToDecimal(reader["Total"]),
                // El campo Confirmado es nuevo en este contexto, debe estar en la clase de dominio Presupuesto
                Confirmado = Convert.ToBoolean(reader["Confirmado"])
            };
        }

        // --- 1. CRUD y Operaciones ---

        public List<Presupuesto> ListarPresupuestos()
        {
            List<Presupuesto> presupuestos = new List<Presupuesto>();
            string spName = "SP_Presupuesto_Listar";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presupuestos.Add(MapearPresupuesto(reader));
                        }
                    }
                }
            }
            return presupuestos;
        }

        public Presupuesto ObtenerPresupuesto(int id)
        {
            string spName = "SP_Presupuesto_Obtener";
            Presupuesto presupuesto = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            presupuesto = MapearPresupuesto(reader);
                        }
                    }
                }
            }
            return presupuesto;
        }

        // El método de Guardado ahora está dividido para manejar el encabezado y los detalles.
        // Se mantiene el SP_GuardarPresupuesto que usaste en el borrador anterior (asumo que se renombró a SP_Presupuesto_Agregar)

        public int GuardarPresupuestoCompleto(Presupuesto presupuesto)
        {
            // Este método DEBE usar una TRANSACCIÓN.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                int nuevoIdPresupuesto = 0;

                try
                {
           
                    // 1. Guardar el encabezado del Presupuesto (Usando tu SP_Presupuesto_Agregar)
                    using (SqlCommand cmd = new SqlCommand("SP_Presupuesto_Agregar", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdCliente", presupuesto.IdCliente);
                        cmd.Parameters.AddWithValue("@Fecha", presupuesto.Fecha);
                        cmd.Parameters.AddWithValue("@Total", presupuesto.Total);

                        // Parámetro de salida: @NuevoId
                        SqlParameter outputParam = new SqlParameter("@NuevoId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        if (outputParam.Value != DBNull.Value)
                        {
                            nuevoIdPresupuesto = Convert.ToInt32(outputParam.Value);
                        }
                    }

                    // 2. Guardar los Detalles (Asumo SP_GuardarDetallePresupuesto existe)
                    foreach (var detalle in presupuesto.Detalles)
                    {
                        using (SqlCommand cmdDetalle = new SqlCommand("SP_DetallePresupuesto_Insertar", conn, transaction))
                        {
                            cmdDetalle.CommandType = CommandType.StoredProcedure;

                            cmdDetalle.Parameters.AddWithValue("@IdPresupuesto", nuevoIdPresupuesto);
                            cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                            cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                            cmdDetalle.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return nuevoIdPresupuesto;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ActualizarPresupuesto(Presupuesto presupuesto)
        {
            string spName = "SP_Presupuesto_Actualizar";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", presupuesto.Id);
                    cmd.Parameters.AddWithValue("@IdCliente", presupuesto.IdCliente);
                    cmd.Parameters.AddWithValue("@Fecha", presupuesto.Fecha);
                    cmd.Parameters.AddWithValue("@Total", presupuesto.Total);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarPresupuesto(int id)
        {
            // Nota: En un sistema real, la eliminación de un presupuesto DEBE eliminar
            // primero sus detalles y luego el encabezado, posiblemente en una transacción.
            // Aquí solo implementamos el SP que eliminaste.
            string spName = "SP_Presupuesto_Eliminar";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ConfirmarPresupuesto(int id)
        {
            string spName = "SP_Presupuesto_Confirmar";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}