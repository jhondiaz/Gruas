using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class SolicitudTvs
    {
        public int Id { get; set; }
        public int IdSol { get; set; }
        public string TipoV { get; set; }
        public string Cantidad { get; set; }
    }
}
