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

    public partial class frmAñadirNota : Form
    {
        public event Action NotaGuardada;
        private Nota notaActual = null; // Almacena la nota que se está editando
        private String Titulo;

        public frmAñadirNota()
        {
            InitializeComponent();
            CargarCategorias();
        }

        // Constructor para editar una nota existente
        public frmAñadirNota(Nota nota) : this()
        {
            Titulo = nota.Titulo;
            notaActual = nota;
            txtTitulo.Text = nota.Titulo;
            txtContenido.Text = nota.Contenido;
            cmbCategoria.SelectedItem = nota.Categoria;

            btnGuardar.Text = "Actualizar"; // Cambiar el texto del botón
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtContenido.Text) || cmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NotasManager manager = new NotasManager();

            if (notaActual == null) // Si no hay nota en edición, crear una nueva
            {
                Nota nuevaNota = new Nota
                {
                    Titulo = txtTitulo.Text,
                    Contenido = txtContenido.Text,
                    Fecha = DateTime.Now,
                    Categoria = cmbCategoria.SelectedItem.ToString()
                };
                manager.Guardar(nuevaNota);
                Limpiar();
                MessageBox.Show("Nota guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // Si hay nota en edición, actualizarla
            {
                notaActual.Titulo = txtTitulo.Text;
                notaActual.Contenido = txtContenido.Text;
                notaActual.Categoria = cmbCategoria.SelectedItem.ToString();

                manager.Editar(Titulo, notaActual);
                MessageBox.Show("Nota actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            NotaGuardada?.Invoke();  // Notificar actualización
            this.Close();  // Cerrar formulario
        }
        private void Limpiar()
        {
            // Limpiar los campos después de añadir
            txtTitulo.Clear();
            txtContenido.Clear();
            cmbCategoria.SelectedIndex = -1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void CargarCategorias()
        {
            // Agregar categorías al ComboBox
            cmbCategoria.Items.Add("Personal");
            cmbCategoria.Items.Add("Estudios");
            cmbCategoria.Items.Add("Trabajo");
            cmbCategoria.Items.Add("Objetivos");

            // Opcional: Seleccionar el primer ítem por defecto
            cmbCategoria.SelectedIndex = 0;
        }
    }

}
