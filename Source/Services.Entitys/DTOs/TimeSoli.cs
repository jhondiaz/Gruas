using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class TimeSoli
    {
        public TimeSpan dias { get; set; }
        public TimeSpan dias2 { get; set; }
        public int solicitud { get; set; }        
        public DateTime? fini { get; set; }
        public DateTime? ffin { get; set; }
        public DateTime? fat { get; set; }
        public string Estado { get; set; }
    }
}
