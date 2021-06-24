using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class DetalleMenusLog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Idmenu { get; set; }
        public string Icon { get; set; }
        public List<AspNetMenusParams> SubMenu { get; set; }
    }
}
