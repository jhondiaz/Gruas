using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class RespuestaUbicacion2
    {
        public RespuestaUbicacion2()
        {
            mensajes = new List<mensajess2>();
        }

        public List<mensajess2> mensajes { get; set; }
        public List<respuestas2> respuesta { get; set; }

    }

    public class mensajess2
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public string severidad { get; set; }

    }

    public class respuestas2
    {
        public string placagrua { get; set; }
        public string ubicacion { get; set; }
        public string tiempo { get; set; }
    }
}
