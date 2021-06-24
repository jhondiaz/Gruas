using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entitys.DTOs
{
    public class AspNetUsersParams
    {
        public string Id { get; set; }
        public string firstName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaAgente { get; set; }
        public string NombreJefe { get; set; }
        public string TelefonoJefe { get; set; }
        public string Entidad { get; set; }
        public int? DiasExpiracion { get; set; }

        public string Tipo { get; set; }
        public string iduser { get; set; }
    }
}
