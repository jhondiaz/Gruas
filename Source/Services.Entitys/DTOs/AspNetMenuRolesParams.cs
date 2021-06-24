using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class AspNetMenuRolesParams
    {
        public string Id { get; set; }
        public string IdMenu { get; set; }
        public string RoleId { get; set; }
        public string IdUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
