using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class ReportSolServices
    {
        public int Conteo { get; set; }
        public string Name { get; set; }
        public string Tipo { get; set; }
        public DateTime? fini { get; set; }
        public DateTime? ffin { get; set; }
        public string Estado { get; set; }
    }
}
