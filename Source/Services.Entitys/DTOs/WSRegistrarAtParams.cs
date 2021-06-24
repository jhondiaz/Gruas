using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class WSRegistrarAtParams
    {
        public string Numero_de_orden_del_servicio { get; set; }
        public DateTime? Fecha_hora_inicio_atencion_servicio { get; set; }
        public string Placa_grua_Numero { get; set; }
        public string Placa_vehiculo_trasladar { get; set; }
        public string Parqueadero_destino { get; set; }
        public string Link_video_inmovilizacion { get; set; }
        public DateTime? Fecha_hora_envio_informacion { get; set; }

        public string nroOrden { get; set; }
        public string direccionGeo { get; set; }
        public string placaGrua { get; set; }
        public string vehiculos { get; set; }

    }
}
