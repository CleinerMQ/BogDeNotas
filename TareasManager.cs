using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDeNotas
{
    public class TareasManager
    {
        private const string filePath = "tareas.txt"; // Ruta donde se guardan las tareas

        // Método para guardar una nueva tarea en el archivo
        public void Guardar(Tarea tarea)
        {
            string tareaData = $"{tarea.Titulo}|{tarea.Contenido}|{tarea.Fecha}|{tarea.FechaLimite}|{tarea.Cumplido}|{tarea.Prioridad}";
            File.AppendAllText(filePath, tareaData + Environment.NewLine);
        }

        // Método para editar una tarea existente
        public void Editar(string titulo, Tarea nuevaTarea)
        {
            List<string> tareas = new List<string>(File.ReadAllLines(filePath));
            for (int i = 0; i < tareas.Count; i++)
            {
                var datosTarea = tareas[i].Split('|');
                if (datosTarea[0] == titulo)
                {
                    tareas[i] = $"{nuevaTarea.Titulo}|{nuevaTarea.Contenido}|{nuevaTarea.Fecha}|{nuevaTarea.FechaLimite}|{nuevaTarea.Cumplido}|{nuevaTarea.Prioridad}";
                    break;
                }
            }
            File.WriteAllLines(filePath, tareas);
        }

        // Método para borrar una tarea existente
        public void Borrar(string titulo)
        {
            List<string> tareas = new List<string>(File.ReadAllLines(filePath));
            tareas.RemoveAll(tarea => tarea.Split('|')[0] == titulo);
            File.WriteAllLines(filePath, tareas);
        }

        // Método para buscar una tarea por título
        public Tarea Buscar(string titulo)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var datosTarea = line.Split('|');
                if (datosTarea[0] == titulo)
                {
                    return new Tarea
                    {
                        Titulo = datosTarea[0],
                        Contenido = datosTarea[1],
                        Fecha = DateTime.Parse(datosTarea[2]),
                        FechaLimite = DateTime.Parse(datosTarea[3]),
                        Cumplido = bool.Parse(datosTarea[4]),
                        Prioridad = datosTarea[5]
                    };
                }
            }
            return null;
        }

        // Método para leer todas las tareas
        public List<Tarea> LeerTodas()
        {
            List<Tarea> tareas = new List<Tarea>();

            // Verificar si el archivo existe
            if (!File.Exists(filePath))
            {
                // Si no existe, devolver una lista vacía
                return tareas;
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var datosTarea = line.Split('|');
                tareas.Add(new Tarea
                {
                    Titulo = datosTarea[0],
                    Contenido = datosTarea[1],
                    Fecha = DateTime.Parse(datosTarea[2]),
                    FechaLimite = DateTime.Parse(datosTarea[3]),
                    Cumplido = bool.Parse(datosTarea[4]),
                    Prioridad = datosTarea[5]
                });
            }
            return tareas;
        }

    }
}
