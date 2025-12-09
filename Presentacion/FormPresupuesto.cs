using SistemaPresupuesto.BusniessLogic;
using SistemaPresupuesto.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormPresupuesto : Form
    {
        private readonly PresupuestoService _presupuestoService;
        private Presupuesto _presupuestoActual;
        private const string ConnectionStringName = "SistemaPresupuestoDB";

        public FormPresupuesto()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            _presupuestoService = new PresupuestoService(connectionString);

            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Inicializar el nuevo presupuesto al inicio
            NuevoPresupuesto();

            // Cargar Comboboxes
            CargarClientes();
            CargarProductos();

            // Configurar DataGridView
            ConfigurarDetalleGrid();
        }

        private void NuevoPresupuesto()
        {
            // Inicializa una nueva instancia de presupuesto
            _presupuestoActual = new Presupuesto
            {
                Fecha = dtpFecha.Value, // dtpFecha es el DateTimePicker
                Detalles = new List<DetallePresupuesto>()
            };

            // Resetear controles
            txtId.Text = "Nuevo";
            txtTotal.Text = "0,00";
            cbCliente.SelectedIndex = -1;
            LimpiarCamposDetalle();
            ActualizarGridDetalle();

            // Habilitar/Deshabilitar botones según el estado (Nuevo)
            btnGuardar.Enabled = true;
            btnConfirmar.Enabled = true;
            btnEliminarPresupuesto.Enabled = false;
            btnNuevo.Enabled = false;
        }

        private void LimpiarCamposDetalle()
        {
            cbProducto.SelectedIndex = -1;
            nudCantidad.Value = 1; // nudCantidad es NumericUpDown
            txtPrecioUnitario.Text = string.Empty;
        }

        private void CargarClientes()
        {
            try
            {
                List<Cliente> clientes = _presupuestoService.ObtenerClientes();
                cbCliente.DataSource = clientes;
                cbCliente.DisplayMember = "Nombre";
                cbCliente.ValueMember = "Id";
                cbCliente.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                List<Producto> productos = _presupuestoService.ObtenerProductos();
                cbProducto.DataSource = productos;
                cbProducto.DisplayMember = "Nombre";
                cbProducto.ValueMember = "Id";
                cbProducto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDetalleGrid()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.Columns.Clear();

            // 1. Producto Nombre
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductoNombre",
                HeaderText = "Producto",
                DataPropertyName = "ProductoNombre" // <--- Clave
            });

            // 2. Cantidad
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad" // <--- Clave
            });

            // 3. Precio Unitario
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecioUnitario",
                HeaderText = "Precio Unitario",
                DataPropertyName = "PrecioUnitarioDisplay" // <--- CLAVE: Usaremos un nombre específico en la lista anónima para el formato
            });

            // 4. Subtotal (Propiedad Total en el dominio)
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Subtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "SubtotalDisplay" // <--- CLAVE: Usaremos un nombre específico para el formato
            });

            // 5. Columna Oculta para ID
            dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdProductoCol", // Renombrado para evitar confusión
                Visible = false,
                DataPropertyName = "IdProducto" // <--- Clave: Mapea a la propiedad real del dominio
            });

            // 6. Columna de botón para Eliminar Detalle
            var colEliminar = new DataGridViewButtonColumn();
            colEliminar.HeaderText = "Eliminar";
            colEliminar.Text = "X";
            colEliminar.UseColumnTextForButtonValue = true;
            colEliminar.Name = "EliminarDetalle";
            dgvDetalles.Columns.Add(colEliminar);

            dgvDetalles.CellContentClick += dgvDetalles_CellContentClick;
        }

        private void ActualizarGridDetalle()
        {
            // Usamos una lista anónima con las propiedades exactas que se mapearon en ConfigurarDetalleGrid
            var detallesParaGrid = _presupuestoActual.Detalles.Select(d => new
            {
                // Propiedad mapeada en la columna "ProductoNombre"
                ProductoNombre = _presupuestoService.ObtenerProducto(d.IdProducto)?.Nombre,

                // Propiedad mapeada en la columna "Cantidad"
                d.Cantidad,

                // Propiedad con formato para "PrecioUnitario"
                PrecioUnitarioDisplay = d.PrecioUnitario.ToString("N2"),

                // Propiedad con formato para "Subtotal" (usando d.Total)
                SubtotalDisplay = d.Total.ToString("N2"),

                // Propiedad oculta para "IdProductoCol"
                d.IdProducto // Mapea a la columna oculta IdProducto

            }).ToList();

            dgvDetalles.DataSource = null; // Reinicia el enlace
            dgvDetalles.DataSource = detallesParaGrid; // Enlaza la nueva lista

            // Recalcular el total
            _presupuestoActual.Total = _presupuestoActual.Detalles.Sum(d => d.Total);
            txtTotal.Text = _presupuestoActual.Total.ToString("N2");
        }

        // --- Eventos ---

        private void cbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProducto.SelectedValue != null && cbProducto.SelectedValue is int productoId && productoId > 0)
            {
                Producto producto = _presupuestoService.ObtenerProducto(productoId);
                if (producto != null)
                {
                    // Muestra el precio unitario del producto seleccionado
                    txtPrecioUnitario.Text = producto.Precio.ToString("N2");
                }
            }
            else
            {
                txtPrecioUnitario.Text = string.Empty;
            }
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e) // Botón 'Agregar' en la sección de detalle
        {
            if (cbProducto.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Ingrese un precio unitario válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cantidad = (int)nudCantidad.Value;
            int productoId = (int)cbProducto.SelectedValue;

            // Crea el detalle
            DetallePresupuesto nuevoDetalle = new DetallePresupuesto
            {
                IdProducto = productoId,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
            };

            // Agrega el detalle a la lista del presupuesto actual
            _presupuestoActual.Detalles.Add(nuevoDetalle);

            LimpiarCamposDetalle();
            ActualizarGridDetalle();
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Manejar el clic en el botón 'Eliminar' de la columna de detalles
            if (dgvDetalles.Columns[e.ColumnIndex].Name == "EliminarDetalle" && e.RowIndex >= 0)
            {
                // AHORA USAMOS EL NOMBRE DE LA PROPIEDAD DE LA LISTA ANÓNIMA: "IdProducto"
                if (dgvDetalles.Rows[e.RowIndex].Cells["IdProducto"].Value != null)
                {
                    int productoIdAEliminar = (int)dgvDetalles.Rows[e.RowIndex].Cells["IdProducto"].Value;

                    // Eliminar de la lista de detalles del presupuesto actual
                    _presupuestoActual.Detalles.RemoveAll(d => d.IdProducto == productoIdAEliminar);

                    ActualizarGridDetalle();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e) // Botón 'Guardar'
        {
            try
            {
                if (cbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (_presupuestoActual.Detalles.Count == 0)
                {
                    MessageBox.Show("Debe agregar productos al presupuesto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _presupuestoActual.IdCliente = (int)cbCliente.SelectedValue;
                _presupuestoActual.Confirmado = false; // Guardado, pero no confirmado/finalizado

                int idGuardado = _presupuestoService.GuardarPresupuestoCompleto(_presupuestoActual);
                _presupuestoActual.Id = idGuardado;
                txtId.Text = idGuardado.ToString();
                MessageBox.Show($"Presupuesto guardado con ID: {idGuardado}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Habilitar botones después de guardar
                btnEliminarPresupuesto.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar presupuesto: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e) // Botón 'Nuevo'
        {
            NuevoPresupuesto();
        }

        private void btnEliminarPresupuesto_Click(object sender, EventArgs e) // Botón 'Eliminar' (principal)
        {
            MessageBox.Show("La funcionalidad de eliminación de presupuesto debe ser implementada en el PresupuestoService.", "Funcionalidad Pendiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Aquí iría la lógica para eliminar el presupuesto guardado.
        }

        private void btnConfirmar_Click(object sender, EventArgs e) // Botón 'Confirmar'
        {
            // 1. Validar que el presupuesto ya esté guardado
            if (_presupuestoActual.Id <= 0 || txtId.Text == "Nuevo")
            {
                MessageBox.Show("Debe guardar el presupuesto antes de poder confirmarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Confirmación al usuario
            DialogResult result = MessageBox.Show(
                "¿Está seguro que desea CONFIRMAR este presupuesto? Esta acción podría afectar el stock.",
                "Confirmar Presupuesto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // 3. Llamar al servicio para cambiar el estado en la BD
                    _presupuestoService.ConfirmarPresupuesto(_presupuestoActual.Id);
                    _presupuestoActual.Confirmado = true;

                    MessageBox.Show($"Presupuesto N°{_presupuestoActual.Id} ha sido confirmado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Actualizar el estado visual del formulario (deshabilitar edición)
                    // Deshabilitar botones de edición una vez confirmado
                    btnGuardar.Enabled = false;
                    btnConfirmar.Enabled = false;
                    // Opcional: Deshabilitar la adición de detalles
                    btnAgregarDetalle.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al confirmar el presupuesto: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}