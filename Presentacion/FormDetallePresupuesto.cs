using SistemaPresupuesto.BusniessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormDetallePresupuesto : Form
    {
        private readonly DetallePresupuestoService _detalleService;
        private const string ConnectionStringName = "SistemaPresupuestoDB";

        public FormDetallePresupuesto()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            // ⚠️ NOTA: El DetallePresupuestoService inyecta otros servicios,
            // por lo que debe construirse con el connectionString
            _detalleService = new DetallePresupuestoService(connectionString);

            ConfigurarGrid();
        }

        private void ConfigurarGrid()
        {
            dgvDetalles.AutoGenerateColumns = true; // Usamos autogeneración para la vista simple
            dgvDetalles.ReadOnly = true;
            dgvDetalles.DataSource = new List<DetallePresupuestoView>(); // Inicializa vacío
        }

        private void btnBuscar_Click(object sender, EventArgs e) // Asume que tienes un botón 'Buscar'
        {
            if (!int.TryParse(txtIdPresupuesto.Text, out int idPresupuesto) || idPresupuesto <= 0)
            {
                MessageBox.Show("Ingrese un ID de Presupuesto válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarDetalles(idPresupuesto);
        }

        private void CargarDetalles(int idPresupuesto)
        {
            try
            {
                // 1. Llamada al servicio
                List<DetallePresupuestoView> detalles = _detalleService.ObtenerDetallesViewPorPresupuesto(idPresupuesto);

                // 2. Enlazar datos
                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = detalles;

                if (detalles.Count == 0)
                {
                    MessageBox.Show($"No se encontraron detalles para el Presupuesto N°{idPresupuesto}.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los detalles: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Asumiendo que txtIdPresupuesto y btnBuscar están definidos en el designer.
    }
}