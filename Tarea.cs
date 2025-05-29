using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDeNotas
{
    public class Tarea
    {
        // Título de la tarea
        public string Titulo { get; set; }

        // Contenido o descripción de la tarea
        public string Contenido { get; set; }

        // Fecha de creación o asignación de la tarea
        public DateTime Fecha { get; set; }

        // Fecha límite para completar la tarea
        public DateTime FechaLimite { get; set; }

        // Indica si la tarea fue completada (verdadero o falso)
        public bool Cumplido { get; set; }

        // Prioridad de la tarea (por ejemplo, Alta, Media, Baja)
        public string Prioridad { get; set; }
    }

}
