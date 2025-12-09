using SistemaPresupuesto.BusniessLogic;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration; // Necesario para leer la ConnectionString

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormCliente : Form
    {
        private readonly ClienteService _clienteService;
        private const string ConnectionStringName = "SistemaPresupuestoDB"; // Ajusta este nombre si es necesario

        public FormCliente()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            _clienteService = new ClienteService(connectionString);

            // Inicialización de la grilla y carga de datos
            ConfigurarDataGridView();
            CargarClientes();
        }

        private void ConfigurarDataGridView()
        {
            // Ocultar la columna Id que no es necesaria para el usuario
            dgvClientes.AutoGenerateColumns = true;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.ReadOnly = true;

            // Asignar el evento para seleccionar una fila
            dgvClientes.CellClick += dgvClientes_CellClick;
        }

        private void CargarClientes()
        {
            try
            {
                List<Cliente> clientes = _clienteService.Clientes();
                dgvClientes.DataSource = clientes;

                // Opcional: Ocultar la columna ID si se genera automáticamente
                if (dgvClientes.Columns.Contains("Id"))
                {
                    dgvClientes.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            // Asegurarse de que no haya una fila seleccionada
            dgvClientes.ClearSelection();
        }

        private Cliente CrearClienteDesdeFormulario()
        {
            // Si hay una fila seleccionada, intentamos obtener el ID para Actualizar/Eliminar
            int id = 0;
            if (dgvClientes.SelectedRows.Count > 0)
            {
                id = (int)dgvClientes.SelectedRows[0].Cells["Id"].Value;
            }

            return new Cliente
            {
                Id = id,
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                Email = txtEmail.Text
            };
        }

        // --- Eventos de Botones ---

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente nuevoCliente = CrearClienteDesdeFormulario();
                if (string.IsNullOrWhiteSpace(nuevoCliente.Nombre))
                {
                    MessageBox.Show("El nombre del cliente no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Si la grilla tiene una fila seleccionada, esto debe ser una actualización, no una adición
                // Por simplicidad, deshabilitamos el botón Agregar si hay una fila seleccionada, o forzamos a que sea Agregar si no hay Id.
                if (dgvClientes.SelectedRows.Count > 0 && (int)dgvClientes.SelectedRows[0].Cells["Id"].Value != 0)
                {
                    MessageBox.Show("Por favor, usa el botón 'Actualizar' o 'Limpiar' para agregar un nuevo cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _clienteService.addCliente(nuevoCliente);
                MessageBox.Show("Cliente agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente de la lista para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cliente clienteActualizado = CrearClienteDesdeFormulario();
                if (clienteActualizado.Id == 0)
                {
                    MessageBox.Show("No se pudo obtener el ID del cliente para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _clienteService.actualiarCliente(clienteActualizado);
                MessageBox.Show("Cliente actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente de la lista para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar el cliente seleccionado?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idAEliminar = (int)dgvClientes.SelectedRows[0].Cells["Id"].Value;

                    _clienteService.deleteCliente(idAEliminar);
                    MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarCampos();
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // --- Evento de Selección de Fila ---

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que se hizo clic en una fila de datos válida
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClientes.Rows[e.RowIndex];

                // Cargar los datos de la fila seleccionada a los campos de texto
                txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? string.Empty;
                txtTelefono.Text = row.Cells["Telefono"].Value?.ToString() ?? string.Empty;
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
            }
        }
    }
}