using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.Entities
{
    public class UbicacionGruas
    {
        public int Id { get; set; }
        public string Ubicacion { get; set; }
        public string Placagrua { get; set; }
        public string OrdenServicio { get; set; }
        public int Tiempo { get; set; }
    }
}
