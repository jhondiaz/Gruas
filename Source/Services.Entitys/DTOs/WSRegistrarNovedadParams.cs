using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class WSRegistrarNovedadParams
    {
        public string Numero_de_orden_del_servicio { get; set; }
        public DateTime? Fecha_hora_novedad { get; set; }
        public string Tipo_novedad { get; set; }
        public string Placa_vehiculo_trasladar { get; set; }
        public string Placa_grua_Numero { get; set; }
        public string Tipo_documento_identificacion_conductor_grua { get; set; }
        public string Numero_identificacion_conductor_grua_Nueva { get; set; }
        public string Placa_grua_Nueva { get; set; }
        public string Tipo_Doc_Conductor_Grua_Nueva { get; set; }
        public DateTime? Fecha_hora_reasignacion { get; set; }
        public string Parqueadero_destino { get; set; }
        public string Observaciones_Novedad { get; set; }
        public DateTime? Fecha_hora_llegada_lugar_solicitud { get; set; }
    }
}
