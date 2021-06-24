using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class RespuestaUbicacion
    {
        public RespuestaUbicacion() {
            mensajes = new List<mensajess>();
        }

        public List<mensajess> mensajes { get; set; }
        public respuestas respuesta { get; set; }
    }

    public class mensajess
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public string severidad { get; set; }

    }

    public class respuestas
    {
        public string placagrua { get; set; }
        public string ubicacion { get; set; }
        public string tiempo { get; set; }
    }
}
