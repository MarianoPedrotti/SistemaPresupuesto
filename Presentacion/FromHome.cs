using System.Windows.Forms;
using SistemaPresupuesto.Presentacion; // Asume que FormCliente está aquí

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Método general para abrir un formulario como hijo del MDI.
        /// (Asume que FormHome es un contenedor MDI, si no lo es, se abrirá como ventana normal).
        /// </summary>
        private void AbrirFormulario(Form form)
        {
            // Opcional: Si quieres que FormHome sea un contenedor MDI (como un escritorio)
            // Descomenta la línea: this.IsMdiContainer = true; en el diseñador
            // if (this.IsMdiContainer)
            // {
            //     form.MdiParent = this;
            // }

            // Verifica si el formulario ya está abierto
            foreach (Form openForm in Application.OpenForms)
            {
                // Busca por el tipo del formulario, ej: FormCliente
                if (openForm.GetType() == form.GetType() && openForm.Name == form.Name)
                {
                    openForm.BringToFront(); // Lo trae al frente
                    return;
                }
            }

            form.Show(); // Abre el nuevo formulario
        }


        // --- Implementación de los Clics del Menú ---

        private void clientesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // Crea una instancia del Formulario de Clientes
            FormCliente formCliente = new FormCliente();

            // Llama al método para abrirlo
            AbrirFormulario(formCliente);
        }

        private void productosToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // Abre FormProducto
            FormProducto formProducto = new FormProducto();
            AbrirFormulario(formProducto);
        }

        private void presupuestoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // Abre FormPresupuesto
            FormPresupuesto formPresupuesto = new FormPresupuesto();
            AbrirFormulario(formPresupuesto);
        }

        private void detallePresupuestosToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // Abre FormDetallePresupuesto
            FormDetallePresupuesto formDetalle = new FormDetallePresupuesto();
            AbrirFormulario(formDetalle);
        }
    }
}