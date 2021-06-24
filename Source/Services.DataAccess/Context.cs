using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Services.Entitys.Entities;
using Services.Entitys.DTOs;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace Services.DataAccess
{
    public class Context : DbContext
    {
        public Context() : base("GruasContext") { }

        public DbSet<AspNetMenus> AspNetMenus { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<AspNetMenuRoles> AspNetMenuRoles { get; set; }
        public DbSet<AspNetMenuUsers> AspNetMenuUsers { get; set; }
        public DbSet<RequestUsers> RequestUsers { get; set; }
        public DbSet<PasswordHistorys> PasswordHistorys { get; set; }
        public DbSet<CodigosInfracciones> CodigosInfracciones { get; set; }
        public DbSet<SolicitudGruas> SolicitudGruas { get; set; }
        public DbSet<NumSolAgents> NumSolAgents { get; set; }
        public DbSet<NumGruasSol> NumGruasSol { get; set; }
        public DbSet<Causa_Cancelaciones> Causa_Cancelaciones { get; set; }
        public DbSet<T_S_Translados> T_S_Translados { get; set; }
        public DbSet<C_Inmovilizaciones> C_Inmovilizaciones { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
        public DbSet<SentidoViales> SentidoViales { get; set; }
        public DbSet<TipoGruas> TipoGruas { get; set; }
        public DbSet<TVehiculoInmovilizars> TVehiculoInmovilizars { get; set; }
        public DbSet<ConfigCierreAutos> ConfigCierreAutos { get; set; }
        public DbSet<TipoOrdServs> TipoOrdServs { get; set; }
        public DbSet<Estados> Estados { get; set; }
        public DbSet<ListaCorreosANS> ListaCorreosANS { get; set; }
        public DbSet<SolicitudTvs> SolicitudTvs { get; set; }
        public DbSet<CanGruasSolicitudes> CanGruasSolicitudes { get; set; }
        public DbSet<UbicacionGruas> UbicacionGruas { get; set; }
        public DbSet<SolicitudGruasHistories> SolicitudGruasHistories { get; set; }
        public DbSet<SolicitudesVehiculosAtendidos> SolicitudesVehiculosAtendidos { get; set; }
        public DbSet<SolicitudTVSHistories> SolicitudTVSHistories { get; set; }
        public DbSet<AutTokens> AutTokens { get; set; }
        public DbSet<TipoNovedades> TipoNovedades { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Context>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
