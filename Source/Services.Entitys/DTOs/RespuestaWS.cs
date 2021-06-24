using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class RespuestaWS
    {
        public RespuestaWS() {
            mensajes = new List<mensajes>();
        }

        public List<mensajes> mensajes { get; set; }
        public respuesta respuesta { get; set; }
    }

    public class RespuestaWSCancelacion
    {
        public mensajes[] mensajes { get; set; }
        public respuestacancelacion respuesta { get; set; }
    }

    public class mensajes
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }
        public string severidad { get; set; }

    }

    public class respuesta
    {
        public string confirmacion { get; set; }
        public int nroOrden { get; set; }
        public string tipoOrden { get; set; }
        public string tipoTraslado { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public DateTime fechaOrdenServicio { get; set; }
        public int causaInmovilizacion { get; set; }
        public int tipoInfraccion { get; set; }
        public string entidad { get; set; }
        public string causalRechazo { get; set; }
    }

    public class respuestacancelacion
    {
        public string confirmacion { get; set; }
        public int? nroOrden { get; set; }
        public string tipoOrden { get; set; }
        public string tipoTraslado { get; set; }
        public DateTime? fechaSolicitud { get; set; }
        public DateTime? fechaOrdenServicio { get; set; }
        public int? causaInmovilizacion { get; set; }
        public int? tipoInfraccion { get; set; }
        public string entidad { get; set; }
        public string causalRechazo { get; set; }
    }
}
