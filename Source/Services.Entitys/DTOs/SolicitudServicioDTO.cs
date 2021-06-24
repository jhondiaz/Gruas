using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class SolicitudServicioDTO
    {
        public string entidad { get; set; }
        public int nroOrden { get; set; }
        public string fechaSolicitud { get; set; }
        public string tipoOrden { get; set; }
        public int tipoTraslado { get; set; }
        public int causaInmovilizacion { get; set; }
        public int tipoInfraccion { get; set; }
        public string direccionGeo { get; set; }
        public int tipoZona { get; set; }
        public string direccionUrbana { get; set; }
        public string direccionRural { get; set; }
        public List<SolicitudCantidadVehiculoDTO> tipoVehiculo { get; set; }
        public List<SolicitudCantidadGruaDTO> gruasSolicitadas { get; set; }
        public string observaciones { get; set; }
        public string parqueaderoOrigen { get; set; }
        public string parqueaderoDestino { get; set; }
        public DateTime? fechaTraslado { get; set; }
        public DateTime? horaTraslado { get; set; }
        public List<SolicitudPlacaDTO> placasTraslado { get; set; }
        public int dependenciaOperativo { get; set; }
        public string nombreSolicitanteOperativo { get; set; }
        public string causalModificacion { get; set; }
        public string causalCancelacion { get; set; }
        public List<GruaAsignadaDTO> gruasAsignadas { get; set; }
        public string confirmacion { get; set; }        

        //public string Numero_de_orden_del_servicio { get; set; }
        //public DateTime? Fecha_y_hora_solicitud_servicio_res { get; set; }
        //public string Placa_grua_Numero { get; set; }
        //public string Tipo_documento_identificacion_conductor_grua { get; set; }
        //public string Numero_de_identificacion_conductor_grua { get; set; }
        //public List<GruasParameters> Tipo_de_Grua { get; set; }
    }

    public class SolicitudCantidadVehiculoDTO
    {
        public string tipoVehiculo { get; set; }
        public string cantidad { get; set; }
    }

    public class SolicitudCantidadGruaDTO
    {
        public string tipoGrua { get; set; }
        public int cantidad { get; set; }
    }

    public class SolicitudPlacaDTO
    {
        public string placa { get; set; }
        public int nroOrdenInicial { get; set; }
    }


    public class GruaAsignadaDTO
    {
        public string placa { get; set; }
        public int tipoGrua { get; set; }
        public string tipoIdenConductor { get; set; }
        public string nroIdenConductor { get; set; }
    }
}
