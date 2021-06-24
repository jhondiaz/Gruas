using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class AtencionSolicitudServicioDTO
    {
        public int nroOrden { get; set; }
        public string direccionGeo { get; set; }
        public string placaGrua { get; set; }
        public List<VehiculoAtendidoDTO> vehiculos { get; set; }
    }

    public class VehiculoAtendidoDTO
    {
        public string placa { get; set; }
        public DateTime? fechaIniAtencion { get; set; }
        public DateTime? fechaFinAtencion { get; set; }
        public int tipoVehiculo { get; set; }
        public string linkVideo { get; set; }
        public int parqueadero { get; set; }
    }
}
