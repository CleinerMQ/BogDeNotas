using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDeNotas
{
    public class NotasManager
    {
        private const string filePath = "notas.txt"; // Ruta donde se guardan las notas

        // Método para guardar una nueva nota en el archivo
        public void Guardar(Nota nota)
        {
            string notaData = $"{nota.Titulo}|{nota.Contenido}|{nota.Fecha}|{nota.Categoria}";
            File.AppendAllText(filePath, notaData + Environment.NewLine);
        }

        // Método para editar una nota existente
        public void Editar(string titulo, Nota nuevaNota)
        {
            List<string> notas = new List<string>(File.ReadAllLines(filePath));
            for (int i = 0; i < notas.Count; i++)
            {
                var datosNota = notas[i].Split('|');
                if (datosNota[0] == titulo)
                {
                    notas[i] = $"{nuevaNota.Titulo}|{nuevaNota.Contenido}|{nuevaNota.Fecha}|{nuevaNota.Categoria}";
                    break;
                }
            }
            File.WriteAllLines(filePath, notas);
        }

        // Método para borrar una nota existente
        public void Borrar(string titulo)
        {
            List<string> notas = new List<string>(File.ReadAllLines(filePath));
            notas.RemoveAll(nota => nota.Split('|')[0] == titulo);
            File.WriteAllLines(filePath, notas);
        }

        // Método para buscar una nota por título
        public Nota Buscar(string titulo)
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var datosNota = line.Split('|');
                if (datosNota[0] == titulo)
                {
                    return new Nota
                    {
                        Titulo = datosNota[0],
                        Contenido = datosNota[1],
                        Fecha = DateTime.Parse(datosNota[2]),
                        Categoria = datosNota[3]
                    };
                }
            }
            return null;
        }

        public List<Nota> LeerTodas()
        {
            List<Nota> notas = new List<Nota>();

            // Verifica si el archivo existe; si no, lo crea vacío para evitar la excepción.
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return notas; // Retorna una lista vacía ya que no hay notas aún.
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var datosNota = line.Split('|');
                if (datosNota.Length < 4) continue; // Evita líneas corruptas o vacías

                notas.Add(new Nota
                {
                    Titulo = datosNota[0],
                    Contenido = datosNota[1],
                    Fecha = DateTime.Parse(datosNota[2]),
                    Categoria = datosNota[3]
                });
            }

            return notas;
        }
        public Nota ObtenerPorTitulo(string titulo)
        {
            List<string> notas = new List<string>(File.ReadAllLines(filePath));

            foreach (var linea in notas)
            {
                var datosNota = linea.Split('|');

                if (datosNota.Length >= 4 && datosNota[0].Trim().Equals(titulo.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return new Nota
                    {
                        Titulo = datosNota[0],
                        Contenido = datosNota[1],
                        Fecha = DateTime.Parse(datosNota[2]),
                        Categoria = datosNota[3]
                    };
                }
            }

            return null; // Retorna null si no encuentra la nota
        }
        public List<string> ObtenerCategorias()
        {
            HashSet<string> categorias = new HashSet<string>();

            // Leer todas las líneas del archivo de notas
            List<string> notas = File.ReadAllLines(filePath).ToList();

            // Iterar a través de las notas y extraer la categoría
            foreach (var linea in notas)
            {
                var datos = linea.Split('|'); // Suponiendo que las notas están separadas por '|'

                // Comprobar que la nota tiene la cantidad correcta de datos
                if (datos.Length >= 4)
                {
                    categorias.Add(datos[3]); // Suponiendo que la categoría es el cuarto campo (índice 3)
                }
            }

            // Convertir el HashSet a una lista y devolverla
            return categorias.ToList();
        }

    }
}
