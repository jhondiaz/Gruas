using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class AspNetMenuUsersParams
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string IdMenu { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
