using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class Estados
    {
        public int Id { get; set; }
        public int ID_solicitud { get; set; }
        public string Nombre { get; set; }
        public string Observaciones { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
