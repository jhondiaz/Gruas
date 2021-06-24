using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class RespuestaSolicitudServicioSDMDTO
    {
        public int nroOrden { get; set; }
        public string tipoOrden { get; set; }
        public string tipoTraslado { get; set; }
        public int causaInmovilizacion { get; set; }
        public string parqueaderoTransitorio { get; set; }
    }
}
