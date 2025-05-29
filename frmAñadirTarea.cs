using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlogDeNotas
{
    public partial class frmAñadirTarea : Form
    {
        public event Action TareaGuardada;
        private TareasManager tareasManager;
        public Tarea TareaAEditar { get; set; }
        public string Titulo;


        public frmAñadirTarea(Tarea tarea = null)
        {
            InitializeComponent();
            tareasManager = new TareasManager();
            CargarPrioridades();

            if (tarea != null)
            {
                // Si se pasa una tarea, llenar los controles con su información
                TareaAEditar = tarea;
                txtTitulo.Text = tarea.Titulo;
                txtContenido.Text = tarea.Contenido;
                dtpFechaLimite.Value = tarea.FechaLimite;
                chkCumplido.Checked = tarea.Cumplido;
                cmbPrioridad.SelectedItem = tarea.Prioridad;
                Titulo = tarea.Titulo;
            }
        }
        private void CargarPrioridades()
        {
            cmbPrioridad.Items.AddRange(new string[] { "Alta", "Media", "Baja" });
            cmbPrioridad.SelectedIndex = 1; // Seleccionar "Media" por defecto
        }
        // Función para limpiar el formulario
        void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtContenido.Clear();
            dtpFechaLimite.Value = DateTime.Now; // Restablecer fecha límite a la actual
            chkCumplido.Checked = false;
            cmbPrioridad.SelectedIndex = -1; // Deseleccionar prioridad
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar que el título no esté vacío
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear o actualizar la tarea
            Tarea tarea = TareaAEditar ?? new Tarea();
            tarea.Titulo = txtTitulo.Text;
            tarea.Contenido = txtContenido.Text;
            tarea.FechaLimite = dtpFechaLimite.Value;
            tarea.Cumplido = chkCumplido.Checked;
            tarea.Prioridad = cmbPrioridad.SelectedItem?.ToString() ?? "Media"; // Manejar caso donde no se seleccione nada

            if (TareaAEditar == null)
            {
                // Si no estamos editando, guardar una nueva tarea
                tareasManager.Guardar(tarea);
                MessageBox.Show("Tarea guardada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Si estamos editando, actualizar la tarea
                tareasManager.Editar(Titulo, tarea);
                MessageBox.Show("Tarea editada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Disparar el evento para notificar al formulario principal que la tarea ha sido guardada o editada
            TareaGuardada?.Invoke();

            // Limpiar los campos del formulario
            LimpiarCampos();
            Close(); // Cerrar el formulario después de guardar/editar
        }
    }
}
