using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class AutTokens
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime TimeExpire { get; set; }
    }
}
