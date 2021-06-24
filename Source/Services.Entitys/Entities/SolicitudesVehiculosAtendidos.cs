using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class SolicitudesVehiculosAtendidos
    {
        public int Id { get; set; }
        public int IdSol { get; set; }
        public string Placa { get; set; }
        public DateTime? FechaIniAtencion { get; set; }
        public DateTime? FechaFinAtencion { get; set; }
        public string TipoVehiculo { get; set; }
        public string LinkVideo { get; set; }
        public int Parqueadero { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
