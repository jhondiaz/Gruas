using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class PasswordHistorys
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public int Count { get; set; }
    }
}
