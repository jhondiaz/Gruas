using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class RequestUsersParams
    {
        public string Id { get; set; }
        public string firstName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaAgente { get; set; }
        public string NombreJefe { get; set; }
        public string TelefonoJefe { get; set; }
        public string Entidad { get; set; }
        public bool Agente { get; set; }
        public int Validado { get; set; }
        public DateTime FechaSolicitud { get; set; }
    }
}
