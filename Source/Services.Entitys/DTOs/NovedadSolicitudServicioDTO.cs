using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class NovedadSolicitudServicioDTO
    {
        public int nroOrden { get; set; }
        public DateTime? fechaReasignacion { get; set; }
        public string tipoNovedad { get; set; }
        public string[] placaVehiculo { get; set; }
        public List<GruaAsignadaDTO> gruaAnterior { get; set; }
        public List<GruaAsignadaDTO> gruaNueva { get; set; }
        public string parqueaderoDestino { get; set; }
        public DateTime? fechaLlegada { get; set; }
        public string observaciones { get; set; }
    }
}
