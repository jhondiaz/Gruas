using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class ValuesWS
    {
        public string entidad { get; set; }
        public DateTime? fechaSolicitud { get; set; }
        public string tipoOrden { get; set; }
        public int tipoTraslado { get; set; }
        public int causaInmovilizacion { get; set; }
        public int tipoInfraccion { get; set; }
        public string direccionGeo { get; set; }
        public int tipoZona { get; set; }
        public string direccionUrbana { get; set; }
        public string direccionRural { get; set; }

        public tipoVehiculos[] tipoVehiculo { get; set; }


        public string observaciones { get; set; }
        public string parqueaderoOrigen { get; set; }
        public string parqueaderoDestino { get; set; }
        public DateTime? fechaTraslado { get; set; }
        public DateTime? horaTraslado { get; set; }

        public int dependenciaOperativo { get; set; }
        public string nombreSolicitanteOperativo { get; set; }
        public int? nroOrden { get; set; }
        public string causalModificacion { get; set; }
        public string causalCancelacion { get; set; }

    }

    public class tipoVehiculos
    {
        public string tipoVehiculo { get; set; }
        public string cantidad { get; set; }
    }
}
