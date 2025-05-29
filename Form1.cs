using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlogDeNotas
{
    public partial class Form1 : Form
    {
        frmTareas frmtareas = new frmTareas();
        frmNotas frmnotas = new frmNotas();

        public Form1()
        {
            InitializeComponent();
            MostrarFormularioEnPanel(frmnotas);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Tick += Timer1_Tick; // Asigna el evento al Timer
            timer1.Start(); // Inicia el Timer
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            txtHora.Text = DateTime.Now.ToString("hh:mm:ss tt"); // Formato de 12 horas con AM/PM
        }
        public void MostrarFormularioEnPanel(Form formulario)
        {
            // Limpiar el panel antes de agregar un nuevo formulario
            pnlPrincipal.Controls.Clear();

            // Configurar el formulario para que se comporte como un control dentro del panel
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            // Agregar el formulario al panel y mostrarlo
            pnlPrincipal.Controls.Add(formulario);
            formulario.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MostrarFormularioEnPanel(frmnotas);
        }

        private void btnTareas_Click(object sender, EventArgs e)
        {
           
            MostrarFormularioEnPanel(frmtareas);
        }
    }
}
