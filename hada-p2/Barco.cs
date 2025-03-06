using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hada
{
    public class Barco
    {
        public event EventHandler<TocadoArgs> eventoTocado;
        public event EventHandler<HundidoArgs> eventoHundido;

        public Dictionary<Coordenada, string> CoordenadasBarco { get; private set; }
        public string Nombre { get; private set; }
        public int NumDanyos { get; private set; }

        public Barco(string nombre, int longitud, char orientacion, Coordenada coordenadaInicio)
        {
            Nombre = nombre;
            NumDanyos = 0;
            CoordenadasBarco = new Dictionary<Coordenada, string>();

            int f = coordenadaInicio.Fila;
            int c = coordenadaInicio.Columna;

            if (orientacion == 'h') // Si la orientación es horizontal
            {
                for (int i = 0; i < longitud; i++)
                {
                    Coordenada c1 = new Coordenada(f, c + i);
                    CoordenadasBarco.Add(c1, nombre);
                }
            }
            else // Si la orientación es vertical
            {
                for (int i = 0; i < longitud; i++)
                {
                    Coordenada c1 = new Coordenada(f + i, c);
                    CoordenadasBarco.Add(c1, nombre);
                }
            }
        }

        public void Disparo(Coordenada c)
        {
            if (CoordenadasBarco.ContainsKey(c)) // Verificamos si la coordenada está en el diccionario
            {
                if (!CoordenadasBarco[c].Contains("_T")) // Verificamos si la coordenada no ha sido tocada
                {
                    NumDanyos++;
                    CoordenadasBarco[c] = Nombre + "_T"; // Marcamos la coordenada como tocada

                    if (eventoTocado != null) // Lanzamos el evento Tocado
                    {
                        eventoTocado(this, new TocadoArgs(Nombre, c));
                    }

                    if (hundido()) // Verificamos si el barco ha sido hundido
                    {
                        if (eventoHundido != null) // Lanzamos el evento Hundido
                        {
                            eventoHundido(this, new HundidoArgs(Nombre));
                        }
                    }
                }
            }
        }

        public bool hundido()
        {
            foreach (string etiqueta in CoordenadasBarco.Values) // Recorremos todas las etiquetas del diccionario
            {
                if (!etiqueta.Contains("_T")) // Verificamos si alguna coordenada no ha sido tocada
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string s = $"[{Nombre}] - DAÑOS: [{NumDanyos}] - HUNDIDO: [{hundido()}] - COORDENADAS: ";

            foreach (var coordenada in CoordenadasBarco) // Usando var para no tener que escribir KeyValuePair<Coordenada, string>
            {
                s += $"[({coordenada.Key.Fila},{coordenada.Key.Columna}) : {coordenada.Value}] ";
            }

            return s;
        }
    }
}