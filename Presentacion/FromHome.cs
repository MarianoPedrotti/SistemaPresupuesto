using System.Drawing;
using System.Windows.Forms;

namespace SistemaPresupuesto.Presentacion
{
    public partial class FormHome : Form
    {
        private Panel panelMain;   // Panel central
        private Panel sidebar;     // Sidebar lateral

        public FormHome()
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(245, 245, 248);
            this.Font = new Font("Segoe UI", 10);

            // Ocultar menú viejo
            menuStrip1.Visible = false;

            CrearHeader();
            CrearSidebar();
            CrearPanelPrincipal();
        }

        // -------------------------------------------------------------
        //   HEADER (barra superior)
        // -------------------------------------------------------------
        private void CrearHeader()
        {
            Panel panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 50;
            panelHeader.BackColor = Color.FromArgb(30, 144, 255);
            this.Controls.Add(panelHeader);

            Label lblTitulo = new Label();
            lblTitulo.Text = "Sistema de Presupuesto";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 12);
            panelHeader.Controls.Add(lblTitulo);

            Button btnCerrar = new Button();
            btnCerrar.Text = "X";
            btnCerrar.ForeColor = Color.White;
            btnCerrar.BackColor = Color.FromArgb(220, 20, 60);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Size = new Size(40, 40);
            btnCerrar.Location = new Point(this.Width - 50, 5);
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Click += (s, e) => this.Close();
            panelHeader.Controls.Add(btnCerrar);
        }

        // -------------------------------------------------------------
        //   SIDEBAR (barra lateral)
        // -------------------------------------------------------------
        private void CrearSidebar()
        {
            sidebar = new Panel();
            sidebar.Width = 200;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(45, 45, 48);
            this.Controls.Add(sidebar);

            // Crear botones
            Button btnClientes = CrearBoton("Clientes");
            Button btnProductos = CrearBoton("Productos");
            Button btnPresupuesto = CrearBoton("Presupuestos");
            Button btnDetalle = CrearBoton("Detalle Presupuesto");

            // Asignar eventos
            btnClientes.Click += (s, e) => clientesToolStripMenuItem_Click(s, e);
            btnProductos.Click += (s, e) => productosToolStripMenuItem_Click(s, e);
            btnPresupuesto.Click += (s, e) => presupuestoToolStripMenuItem_Click(s, e);
            btnDetalle.Click += (s, e) => detallePresupuestosToolStripMenuItem_Click(s, e);

            // Agregar al sidebar (orden invertido para apilar)
            sidebar.Controls.Add(btnDetalle);
            sidebar.Controls.Add(btnPresupuesto);
            sidebar.Controls.Add(btnProductos);
            sidebar.Controls.Add(btnClientes);
        }

        // Creador genérico de botones
        private Button CrearBoton(string texto)
        {
            return new Button()
            {
                Text = texto,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Padding = new Padding(20, 0, 0, 0)
            };
        }

        // -------------------------------------------------------------
        //   PANEL CENTRAL DONDE SE CARGAN LOS FORMULARIOS HIJOS
        // -------------------------------------------------------------
        private void CrearPanelPrincipal()
        {
            panelMain = new Panel();
            panelMain.Dock = DockStyle.Fill;
            panelMain.BackColor = Color.White;
            this.Controls.Add(panelMain);
            panelMain.BringToFront();
        }

        // -------------------------------------------------------------
        //   ABRIR FORMULARIOS DENTRO DEL PANEL CENTRAL
        // -------------------------------------------------------------
        private void AbrirFormulario(Form form)
        {
            panelMain.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panelMain.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        // -------------------------------------------------------------
        //   EVENTOS DEL MENÚ ORIGINAL (SE SIGUEN USANDO)
        // -------------------------------------------------------------
        private void clientesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AbrirFormulario(new FormCliente());
        }

        private void productosToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AbrirFormulario(new FormProducto());
        }

        private void presupuestoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AbrirFormulario(new FormPresupuesto());
        }

        private void detallePresupuestosToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AbrirFormulario(new FormDetallePresupuesto());
        }
    }
}
