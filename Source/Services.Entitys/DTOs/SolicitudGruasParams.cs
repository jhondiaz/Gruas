using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class SolicitudGruasParams
    {
        public SolicitudGruasParams() {
            Tipo_de_Grua = new List<GruasParams>();
        }

        public int ID_solicitud { get; set; }
        public string Entidad { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime Fecha_y_hora_solicitud_servicio { get; set; }
        public string Tipo_de_orden_de_servicio { get; set; }
        public string Tipo_de_servicio_de_traslado { get; set; }
        public string Causa_de_inmovilizacion { get; set; }
        public string Codigo_de_infraccion { get; set; }
        public string Direccion_georreferencial { get; set; }
        public string Localidad { get; set; }
        public string Barrio { get; set; }
        public string Tipo_Zona { get; set; }
        public string Tipo_de_vehiculo_a_inmovilizar { get; set; }
        public List<GruasParams> Tipo_de_Grua { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha_y_hora_solicitud_servicio_res { get; set; }
        public string Direccion_georreferencial_res { get; set; }
        public string Confirmacion_de_envio { get; set; }
        public string Numero_de_orden_del_servicio { get; set; }
        public string Causal_de_rechazo { get; set; }
        public DateTime Fecha_y_hora_de_orden_de_servicio { get; set; }
        public string Placa_grua_Numero { get; set; }
        public string Tipo_documento_identificacion_conductor_grua { get; set; }
        public string Numero_de_identificacion_conductor_grua { get; set; }
        public DateTime Fecha_hora_inicio_atencion_servicio { get; set; }
        public string Placa_vehiculo_trasladar { get; set; }
        public DateTime Fecha_hora_Finalizacion_servicio { get; set; }
        public string Parqueadero_destino { get; set; }
        public string Link_video_inmovilizacion { get; set; }
        public DateTime Fecha_hora_envio_informacion { get; set; }
        public DateTime Fecha_hora_novedad { get; set; }
        public string Tipo_novedad { get; set; }
        public string Placa_grua_Nueva { get; set; }
        public string Tipo_Doc_Conductor_Grua_Nueva { get; set; }
        public string Numero_identificacion_conductor_grua_Nueva { get; set; }
        public DateTime Fecha_hora_reasignacion { get; set; }
        public string Observaciones_Novedad { get; set; }
        public DateTime Fecha_hora_llegada_lugar_solicitud { get; set; }
        public DateTime Fecha_Cierre_Auto { get; set; }
        public string Causa_de_Cancelacion_de_Servicio { get; set; }
        public string Direccion { get; set; }
        public string ObsCancel { get; set; }
        public DateTime? ANSTime { get; set; }
        public bool ValANS { get; set; }
        public List<string> CantVti { get; set; }

    }

    public class GruasParams
    {
        public string Tipo_de_Grua { get; set; }
        public int Numero_de_gruas_solicitadas_por_tipo_de_grua { get; set; }
    }
}
