using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class NumSolAgents
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Conteo { get; set; }
        public int Tope { get; set; }
        public DateTime Fecha { get; set; }
    }
}
