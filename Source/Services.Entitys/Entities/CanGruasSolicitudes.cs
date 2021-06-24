using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class CanGruasSolicitudes
    {
        public int Id { get; set; }
        public int IdSol { get; set; }
        public string TipoGrua { get; set; }
        public string Placa { get; set; }
        public string TipoIdenConductor { get; set; }
        public string NroIdenConductor { get; set; }
        public int Estado { get; set; }
    }
}
