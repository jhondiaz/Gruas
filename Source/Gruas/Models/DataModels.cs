using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Gruas.Models
{
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string firstName { get; set; }        
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string PlacaAgente { get; set; }
        public string NombreJefe { get; set; }
        public string TelefonoJefe { get; set; }
        public string Entidad { get; set; }
        public bool Agente { get; set; }
        public int AccessFailedCount { get; set; }
        public int? DiasExpiracion { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }

        public virtual List<todoItem> todoItems { get; set; }
    }

    public class todoItem
    {
        [Key]
        public int id { get; set; }
        public string task { get; set; }
        public bool completed { get; set; }
    }

    public class MenuItems
    {
        [Key]
        public int Id { get; set; }
        public int IdChild { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string TemplateName { get; set; }
        public string TemplateUrl { get; set; }
        public string Controller { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActivo { get; set; }
    }


    public class RolesItems
    {
        [Key]
        public int Id { get; set; }
        public int IdMenuItem { get; set; }
        public string RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActivo { get; set; }
    }

    public class DBContext : IdentityDbContext<User>
    {
        public DBContext() : base("GruasContext")
        {

        }
        //Override default table names
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static DBContext Create()
        {
            return new DBContext();
        }

        public DbSet<todoItem> TodoTokens { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<RolesItems> RolesItems { get; set; }

    }

    //This function will ensure the database is created and seeded with any default data.
    public class DBInitializer : CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            context.MenuItems.Add(new MenuItems
            {

                IdChild = 0,
                CreateDate = DateTime.Now,
                IsActivo = true,
                Name = "USUARIOS",
            });

            var Id = context.SaveChanges();

            context.MenuItems.Add(new MenuItems
            {
                IdChild = Id,
                CreateDate = DateTime.Now,
                IsActivo = true,
                Name = "Registrar",
                Controller = "registerCtrl",
                TemplateName = "/register",
                TemplateUrl = "App/Register"
            });

            context.SaveChanges();

        }
    }
}

