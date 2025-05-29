using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDeNotas
{
    public class Nota
    {
        // Título de la nota
        public string Titulo { get; set; }

        // Contenido de la nota
        public string Contenido { get; set; }

        // Fecha de creación de la nota
        public DateTime Fecha { get; set; }

        // Categoría de la nota (por ejemplo, Trabajo, Personal, etc.)
        public string Categoria { get; set; }
    }

}
