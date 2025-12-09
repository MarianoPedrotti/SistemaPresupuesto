namespace SistemaPresupuesto.Presentacion
{
    partial class FormDetallePresupuesto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblIdPresupuesto = new System.Windows.Forms.Label();
            this.txtIdPresupuesto = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(425, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Consulta de Líneas de Detalle por Presupuesto";
            // 
            // lblIdPresupuesto
            // 
            this.lblIdPresupuesto.AutoSize = true;
            this.lblIdPresupuesto.Location = new System.Drawing.Point(25, 80);
            this.lblIdPresupuesto.Name = "lblIdPresupuesto";
            this.lblIdPresupuesto.Size = new System.Drawing.Size(117, 13);
            this.lblIdPresupuesto.TabIndex = 1;
            this.lblIdPresupuesto.Text = "ID de Presupuesto:";
            // 
            // txtIdPresupuesto
            // 
            this.txtIdPresupuesto.Location = new System.Drawing.Point(148, 77);
            this.txtIdPresupuesto.Name = "txtIdPresupuesto";
            this.txtIdPresupuesto.Size = new System.Drawing.Size(100, 20);
            this.txtIdPresupuesto.TabIndex = 2;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(260, 75);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click); // ASIGNACIÓN DEL EVENTO
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Location = new System.Drawing.Point(25, 120);
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.Size = new System.Drawing.Size(750, 300);
            this.dgvDetalles.TabIndex = 4;
            // 
            // FormDetallePresupuesto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDetalles);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtIdPresupuesto);
            this.Controls.Add(this.lblIdPresupuesto);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormDetallePresupuesto";
            this.Text = "Detalle de Presupuesto";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblIdPresupuesto;
        private System.Windows.Forms.TextBox txtIdPresupuesto;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvDetalles; // Control clave para la tabla
    }
}