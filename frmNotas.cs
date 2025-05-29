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
    public partial class frmNotas : Form
    {
        public frmNotas()
        {
            InitializeComponent();
            CargarFiltro();
        }
        private void EditarNota(string titulo)
        {
            NotasManager manager = new NotasManager();
            Nota nota = manager.ObtenerPorTitulo(titulo);

            if (nota != null)
            {
                frmAñadirNota form = new frmAñadirNota(nota);
                form.NotaGuardada += CargarNotas; // Recargar después de actualizar
                form.ShowDialog();
            }
        }

        private void EliminarNota(string titulo)
        {
            var confirmacion = MessageBox.Show("¿Seguro que deseas eliminar esta nota?", "Confirmar eliminación", MessageBoxButtons.YesNo);
            if (confirmacion == DialogResult.Yes)
            {
                NotasManager manager = new NotasManager();
                manager.Borrar(titulo);
                CargarFiltro();
                CargarNotas(); // Recargar notas después de eliminar
            }
        }
        private void CargarNotas()
        {
            flowLayoutPanelNotas.Controls.Clear(); // Limpiar antes de cargar

            NotasManager manager = new NotasManager();
            List<Nota> notas = manager.LeerTodas();

            // Obtener categoría seleccionada
            string categoriaSeleccionada = cmbFiltroCategoria.SelectedItem?.ToString();

            // Filtrar notas si no es "Todo"
            if (categoriaSeleccionada != "Todo")
            {
                notas = notas.Where(n => n.Categoria == categoriaSeleccionada).ToList();
            }

            foreach (var nota in notas)
            {
                // Crear tarjeta como lo hacías antes
                Panel tarjeta = new Panel
                {
                    Width = (flowLayoutPanelNotas.Width / 2) - 15,
                    Height = 180,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    Margin = new Padding(5),
                    BackColor = Color.FromArgb(30, 30, 30)
                };

                Label lblTitulo = new Label
                {
                    Text = nota.Titulo,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Width = tarjeta.Width - 10,
                    Height = 25,
                    Location = new Point(5, 5)
                };

                Label lblVistaPrevia = new Label
                {
                    Text = nota.Contenido.Length > 50 ? nota.Contenido.Substring(0, 50) + "..." : nota.Contenido,
                    ForeColor = Color.LightGray,
                    AutoSize = false,
                    Width = tarjeta.Width - 10,
                    Height = 40,
                    Location = new Point(5, 35)
                };

                Label lblCategoria = new Label
                {
                    Text = $"Categoría: {nota.Categoria}",
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    Width = tarjeta.Width - 10,
                    Height = 20,
                    Location = new Point(5, 75)
                };

                Label lblFecha = new Label
                {
                    Text = $"Fecha: {nota.Fecha:dd/MM/yyyy}",
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    Width = tarjeta.Width - 10,
                    Height = 20,
                    Location = new Point(5, 95)
                };

                Button btnEditar = new Button
                {
                    Text = "Ver",
                    Width = (tarjeta.Width / 2) - 10,
                    Height = 30,
                    BackColor = Color.Orange,
                    ForeColor = Color.Black,
                    Location = new Point(5, 120)
                };
                btnEditar.Click += (sender, e) => EditarNota(nota.Titulo);

                Button btnEliminar = new Button
                {
                    Text = "Eliminar",
                    Width = (tarjeta.Width / 2) - 10,
                    Height = 30,
                    BackColor = Color.Red,
                    ForeColor = Color.White,
                    Location = new Point((tarjeta.Width / 2) + 5, 120)
                };
                btnEliminar.Click += (sender, e) => EliminarNota(nota.Titulo);

                tarjeta.Controls.Add(lblTitulo);
                tarjeta.Controls.Add(lblVistaPrevia);
                tarjeta.Controls.Add(lblCategoria);
                tarjeta.Controls.Add(lblFecha);
                tarjeta.Controls.Add(btnEditar);
                tarjeta.Controls.Add(btnEliminar);

                flowLayoutPanelNotas.Controls.Add(tarjeta);
            }

            flowLayoutPanelNotas.BackColor = Color.Black;
        }




        private void frmNotas_Load(object sender, EventArgs e)
        {
            CargarNotas();
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            frmAñadirNota notas = new frmAñadirNota();
            notas.NotaGuardada += CargarNotas;
            notas.NotaGuardada += CargarFiltro;
            notas.ShowDialog();
           
        }

        private void cmbFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarNotas();

        }
        private void CargarFiltro()
        {
            // Limpiar las opciones previas del ComboBox
            cmbFiltroCategoria.Items.Clear();
            cmbFiltroCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroCategoria.Items.Add("Todo"); // Opción para mostrar todas las notas
            NotasManager manager = new NotasManager();
            List<string> categorias = manager.ObtenerCategorias(); // Método que devuelve una lista de categorías
            cmbFiltroCategoria.Items.AddRange(categorias.ToArray());
            cmbFiltroCategoria.SelectedIndex = 0; // Seleccionar "Todo" por defecto

        }
    }
}
