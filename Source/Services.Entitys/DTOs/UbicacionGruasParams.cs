using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class UbicacionGruasParams
    {
        public int Id { get; set; }
        public string Ubicacion { get; set; }
        public string placagrua { get; set; }
        public string nroOrden { get; set; }        
        public int Tiempo { get; set; }

        public string strCoordenadas { get; set; }
    }
}
