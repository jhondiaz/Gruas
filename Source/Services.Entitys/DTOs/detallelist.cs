using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Entitys.Entities;

namespace Services.Entitys.DTOs
{
    public class detallelist
    {
        public List<SolicitudGruas> v1 { get; set; }
        public List<SolicitudTvs> v2 { get; set; }
    }
}
