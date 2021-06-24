using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class ReporteTipoXUser
    {
        public List<CInmov> CausaIn { get; set; }
        public List<TGrua> TipoGru { get; set; }
        public List<TVehiculo> TipoVehi  { get; set; }
        public List<MCacncelacion> CausaCan { get; set; }
        public string UserName { get; set; }
    }
}
