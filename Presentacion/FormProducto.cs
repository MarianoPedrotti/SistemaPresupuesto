using SistemaPresupuesto.BusniessLogic;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormProducto : Form
    {
        // Se asume que el nombre de la ConnectionString es "SistemaPresupuestoDB" (Ajustar si es necesario)
        private const string ConnectionStringName = "SistemaPresupuestoDB";
        private readonly ProductoService _productoService;

        public FormProducto()
        {
            InitializeComponent();

            // 1. Inicialización del Servicio (Manejo de la ConnectionString)
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                _productoService = new ProductoService(connectionString);

                ConfigurarDataGridView();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de inicialización (ConnectionString): Revise el App.config y el nombre '{ConnectionStringName}'. Detalle: {ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Cierra el formulario si no puede inicializar el servicio
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvProductos.AutoGenerateColumns = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.ReadOnly = true;
            dgvProductos.CellClick += dgvProductos_CellClick;
        }

        private void CargarProductos()
        {
            try
            {
                // ** USA TU MÉTODO: productos() **
                List<Producto> productos = _productoService.productos();
                dgvProductos.DataSource = productos;

                if (dgvProductos.Columns.Contains("Id"))
                {
                    dgvProductos.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos desde la base de datos: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
            dgvProductos.ClearSelection();
        }
        // Dentro de la clase FormProducto, junto a los otros métodos de eventos:

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
        private Producto CrearProductoDesdeFormulario()
        {
            int id = 0;
            // Solo obtiene el ID si una fila está seleccionada para actualización/eliminación
            if (dgvProductos.SelectedRows.Count > 0)
            {
                // Asegura que la columna "Id" existe y el valor no es nulo
                if (dgvProductos.SelectedRows[0].Cells["Id"].Value != null)
                {
                    id = (int)dgvProductos.SelectedRows[0].Cells["Id"].Value;
                }
            }

            decimal precio = 0;
            // Usa NumberStyles.Any para ser flexible con el formato local (coma o punto)
            decimal.TryParse(txtPrecio.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out precio);

            int stock = 0;
            int.TryParse(txtStock.Text, out stock);


            return new Producto
            {
                Id = id,
                Nombre = txtNombre.Text,
                Precio = precio,
                Stock = stock
            };
        }

        // --- Eventos de Botones ---

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                Producto nuevoProducto = CrearProductoDesdeFormulario();

                if (string.IsNullOrWhiteSpace(nuevoProducto.Nombre) || nuevoProducto.Precio <= 0)
                {
                    MessageBox.Show("Debe ingresar un nombre y un precio válido (> 0).", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Evitar agregar si hay una fila seleccionada con un ID válido (indica actualización)
                if (nuevoProducto.Id != 0)
                {
                    MessageBox.Show("Use el botón 'Actualizar' para modificar el producto seleccionado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ** USA TU MÉTODO: agregarProducto() **
                _productoService.agregarProducto(nuevoProducto);
                MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarProducto_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Producto productoActualizado = CrearProductoDesdeFormulario();
                if (productoActualizado.Id == 0)
                {
                    MessageBox.Show("Error: ID de producto no encontrado para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ** USA TU MÉTODO: actualizarProducto() **
                _productoService.actualizarProducto(productoActualizado);
                MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar el producto seleccionado?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int idAEliminar = (int)dgvProductos.SelectedRows[0].Cells["Id"].Value;

                    // ** USA TU MÉTODO: eliminarProducto() **
                    _productoService.eliminarProducto(idAEliminar);
                    MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimpiarCampos();
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // La funcionalidad de búsqueda se deja pendiente de implementación en el servicio
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            MessageBox.Show("La funcionalidad de Buscar debe ser implementada en el ProductoService y ProductoRepository.", "Funcionalidad Pendiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Evento de Selección de Fila ---

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];

                txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? string.Empty;

                // Formatea el decimal a dos decimales según la cultura actual
                if (row.Cells["Precio"].Value != null && decimal.TryParse(row.Cells["Precio"].Value.ToString(), out decimal precio))
                {
                    txtPrecio.Text = precio.ToString("N2", CultureInfo.CurrentCulture);
                }
                else
                {
                    txtPrecio.Text = string.Empty;
                }

                txtStock.Text = row.Cells["Stock"].Value?.ToString() ?? string.Empty;
            }
        }
    }
}