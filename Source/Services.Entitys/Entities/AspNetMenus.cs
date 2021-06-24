using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class AspNetMenus
    {
        public string Id { get; set; }
        public string Menu { get; set; }
        public int OrderMenu { get; set; }
        public string Icon { get; set; }
        public string IdSubMenu { get; set; }
        public string SubMenu { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string AUrl { get; set; }
        public string ATemplateUrl { get; set; }
        public string AController { get; set; }
        public bool Mostrar { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
