using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;



namespace BlogDeNotas
{
    public partial class frmTareas : Form
    {
        private TareasManager tareasManager;
        private List<Tarea> listaTareas;
        private System.Timers.Timer timerNotificacion;

        public frmTareas()
        {
            InitializeComponent();
            tareasManager = new TareasManager();
            CargarTareas();

            // Cargar los filtros de los ComboBox
            cmbPrioridad.Items.AddRange(new string[] { "Todas", "Alta", "Media", "Baja" });
            cmbPrioridad.SelectedIndex = 0;  // "Todas" seleccionada por defecto

            cmbEstado.Items.AddRange(new string[] { "Todos", "Cumplidos", "No cumplidos" });
            cmbEstado.SelectedIndex = 0;  // "Todos" seleccionada por defecto

            // Agregar eventos para cambiar el filtro
            cmbPrioridad.SelectedIndexChanged += (sender, e) => AplicarFiltros();
            cmbEstado.SelectedIndexChanged += (sender, e) => AplicarFiltros();
            // Inicializar NotifyIcon
            
            // Inicializar Timer
            timerNotificacion = new System.Timers.Timer(86400000 ); 
            timerNotificacion.Elapsed += VerificarTareasPendientes;
            timerNotificacion.Start();
        }
        private void VerificarTareasPendientes(object sender, ElapsedEventArgs e)
        {
            List<Tarea> tareasPendientes = tareasManager.LeerTodas().Where(t => !t.Cumplido).ToList();

            if (tareasPendientes.Any())
            {
                notifyIcon1.ShowBalloonTip(5000, "Tareas Pendientes", "Tienes tareas sin completar.", ToolTipIcon.Warning);
            }
        }

        private void AplicarFiltros()
        {
            string prioridadSeleccionada = cmbPrioridad.SelectedItem.ToString();
            string estadoSeleccionado = cmbEstado.SelectedItem.ToString();

            // Filtrar tareas
            var tareasFiltradas = tareasManager.LeerTodas();

            // Filtrar por prioridad
            if (prioridadSeleccionada != "Todas")
            {
                tareasFiltradas = tareasFiltradas.Where(t => t.Prioridad.Equals(prioridadSeleccionada, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtrar por estado
            if (estadoSeleccionado == "Cumplidos")
            {
                tareasFiltradas = tareasFiltradas.Where(t => t.Cumplido).ToList();
            }
            else if (estadoSeleccionado == "No cumplidos")
            {
                tareasFiltradas = tareasFiltradas.Where(t => !t.Cumplido).ToList();
            }

            // Actualizar la lista de tareas con las tareas filtradas
            MostrarTareas(tareasFiltradas);
        }
        private void MostrarTareas(List<Tarea> tareas)
        {
            flowLayoutPanelTareas.Controls.Clear(); // Limpiar antes de cargar

            foreach (var tarea in tareas)
            {
                // Crear un panel para cada tarea
                Panel panelTarea = new Panel
                {
                    Width = flowLayoutPanelTareas.Width - 20,
                    Height = 90,
                    BackColor = Color.FromArgb(30, 30, 30), // Fondo oscuro
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    Margin = new Padding(5)
                };

                // CheckBox para marcar como completado
                CheckBox chkCompletado = new CheckBox
                {
                    Text = tarea.Titulo,
                    AutoSize = true,
                    Checked = tarea.Cumplido,
                    Font = tarea.Cumplido ? new Font("Segoe UI", 10, FontStyle.Strikeout) : new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.White, // Texto blanco para contraste
                    Location = new Point(10, 10)
                };
                chkCompletado.CheckedChanged += (sender, e) => MarcarTarea(tarea, chkCompletado.Checked);

                // Label para mostrar la fecha límite
                Label lblFechaLimite = new Label
                {
                    Text = $"📅 {tarea.FechaLimite.ToShortDateString()}",
                    AutoSize = true,
                    Location = new Point(30, 35),
                    ForeColor = Color.LightGray
                };

                // Label para mostrar la prioridad en color
                Label lblPrioridad = new Label
                {
                    Text = $"🔥 Prioridad: {tarea.Prioridad}",
                    AutoSize = true,
                    Location = new Point(30, 55),
                    ForeColor = GetPrioridadColor(tarea.Prioridad)
                };

                // Botón para editar
                Button btnEditar = new Button
                {
                    Text = "Ver",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.White,
                    BackColor = Color.Gray,
                    Location = new Point(panelTarea.Width - 160, 25),
                    Width = 60,
                    Height = 30
                };
                btnEditar.Click += (sender, e) => EditarTarea(tarea);

                // Botón para eliminar
                Button btnEliminar = new Button
                {
                    Text = "Eliminar",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.White,
                    BackColor = Color.Red,
                    Location = new Point(panelTarea.Width - 90, 25),
                    Width = 60,
                    Height = 30
                };
                btnEliminar.Click += (sender, e) => EliminarTarea(tarea);

                // Agregar controles al panel
                panelTarea.Controls.Add(chkCompletado);
                panelTarea.Controls.Add(lblFechaLimite);
                panelTarea.Controls.Add(lblPrioridad);
                panelTarea.Controls.Add(btnEditar);
                panelTarea.Controls.Add(btnEliminar);

                // Agregar el panel al FlowLayoutPanel
                flowLayoutPanelTareas.Controls.Add(panelTarea);
            }
        }

        

        private void CargarTareas()
        {
            listaTareas = tareasManager.LeerTodas();
            flowLayoutPanelTareas.Controls.Clear(); // Limpiar antes de cargar

            foreach (var tarea in listaTareas)
            {
                // Crear un panel para cada tarea
                Panel panelTarea = new Panel
                {
                    Width = flowLayoutPanelTareas.Width - 30,
                    Height = 90,
                    BackColor = Color.FromArgb(30, 30, 30), // Fondo oscuro
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    Margin = new Padding(5)
                };

                // CheckBox para marcar como completado
                CheckBox chkCompletado = new CheckBox
                {
                    Text = tarea.Titulo,
                    AutoSize = true,
                    Checked = tarea.Cumplido,
                    Font = tarea.Cumplido ? new Font("Segoe UI", 10, FontStyle.Strikeout) : new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.White, // Texto blanco para contraste
                    Location = new Point(10, 10)
                };
                chkCompletado.CheckedChanged += (sender, e) => MarcarTarea(tarea, chkCompletado.Checked);

                // Label para mostrar la fecha límite
                Label lblFechaLimite = new Label
                {
                    Text = $"📅 {tarea.FechaLimite.ToShortDateString()}",
                    AutoSize = true,
                    Location = new Point(30, 35),
                    ForeColor = Color.LightGray
                };

                // Label para mostrar la prioridad en color
                Label lblPrioridad = new Label
                {
                    Text = $"🔥 Prioridad: {tarea.Prioridad}",
                    AutoSize = true,
                    Location = new Point(30, 55),
                    ForeColor = GetPrioridadColor(tarea.Prioridad)
                };

                // Botón para editar
                Button btnEditar = new Button
                {
                    Text = "Ver",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.White,
                    BackColor = Color.Gray,
                    Location = new Point(panelTarea.Width - 160, 25),
                    Width = 60,
                    Height = 30
                };
                btnEditar.Click += (sender, e) => EditarTarea(tarea);

                // Botón para eliminar
                Button btnEliminar = new Button
                {
                    Text = "Eliminar",
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.White,
                    BackColor = Color.Red,
                    Location = new Point(panelTarea.Width - 90, 25),
                    Width = 60,
                    Height = 30
                };
                btnEliminar.Click += (sender, e) => EliminarTarea(tarea);

                // Agregar controles al panel
                panelTarea.Controls.Add(chkCompletado);
                panelTarea.Controls.Add(lblFechaLimite);
                panelTarea.Controls.Add(lblPrioridad);
                panelTarea.Controls.Add(btnEditar);
                panelTarea.Controls.Add(btnEliminar);

                // Agregar el panel al FlowLayoutPanel
                flowLayoutPanelTareas.Controls.Add(panelTarea);
            }
        }

        // Método para obtener el color según la prioridad
        private Color GetPrioridadColor(string prioridad)
        {
            switch (prioridad.ToLower())
            {
                case "alta":
                    return Color.Red; // Rojo para alta
                case "media":
                    return Color.Orange; // Naranja para media
                case "baja":
                    return Color.Green; // Verde para baja
                default:
                    return Color.Gray; // Default color
            }
        }

        // Método para editar la tarea
        private void EditarTarea(Tarea tarea)
        {
            // Abrir el formulario de añadir/editar y pasar la tarea a editar
            frmAñadirTarea formularioEditar = new frmAñadirTarea(tarea);
            formularioEditar.ShowDialog();

            // Recargar las tareas después de la edición
            CargarTareas();
        }

        // Método para eliminar la tarea
        private void EliminarTarea(Tarea tarea)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro de eliminar esta tarea?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                tareasManager.Borrar(tarea.Titulo);
                CargarTareas(); // Recargar tareas después de eliminar
                MessageBox.Show("Tarea eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MarcarTarea(Tarea tarea, bool cumplido)
        {
            tarea.Cumplido = cumplido;
            tareasManager.Editar(tarea.Titulo, tarea);
            CargarTareas(); // Recargar para aplicar el formato
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAñadirTarea tarea = new frmAñadirTarea();
            tarea.TareaGuardada += CargarTareas;
            tarea.ShowDialog();
        }
        // Método para mostrar una notificación emergente
        private void MostrarNotificacion(string titulo, string mensaje)
        {
            notifyIcon.BalloonTipTitle = titulo;
            notifyIcon.BalloonTipText = mensaje;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
            notifyIcon.ShowBalloonTip(5000); // Muestra por 5 segundos
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {

            AbrirFormularioTareas();

        }
        private void AbrirFormularioTareas()
        {
            // Buscar si el formulario principal ya está abierto
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1 principal)
                {
                    // Si está minimizado, restaurarlo y traerlo al frente
                    if (principal.WindowState == FormWindowState.Minimized)
                    {
                        principal.WindowState = FormWindowState.Normal;
                    }

                    principal.BringToFront(); // Traerlo al frente
                    principal.Activate(); // Activarlo

                    // Mostrar el formulario de tareas en el panel
                    principal.MostrarFormularioEnPanel(new frmTareas());
                    break;
                }
            }
        }

    }
}
