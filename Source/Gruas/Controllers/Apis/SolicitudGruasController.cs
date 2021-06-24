using System.Web.Mvc;
using Services.Bussiness;
using Services.Entitys.DTOs;
using Services.Entitys.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;
using System.Web;

namespace Gruas.Controllers.Apis
{
    public class SolicitudGruasController : ApiBaseController
    {        
        FormsServices Services = new FormsServices();

        /// <summary>
        /// saveinfgru
        /// Description: Método para Registrar una solicitud de Grúas.
        /// </summary>
        /// <param values=json de la Entidad SolicitudGruasParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic saveinfgru(SolicitudGruasParams values)
        {
            return Services.saveinfgru(values);
        }

        /// <summary>
        /// infcodes
        /// Description: Obtiene la Lista Códigos de Infracción.
        /// </summary>
        /// <returns List= Lista de Códigos de Infracción></returns>
        [HttpPost]
        public dynamic infcodes()
        {
            return Services.infcodes();
        }

        /// <summary>
        /// searchagent
        /// Description: Método para Buscar un Agente por Documento o Placa.
        /// </summary>
        /// <param usr=json de la Entidad AspNetUsersParams></param>
        /// <returns AspNetUsersParams = Objeto asociado a la Búsqueda.</returns>
        [HttpPost]
        public dynamic searchagent(AspNetUsersParams usr)
        {
            return Services.searchagent(usr);
        }

        /// <summary>
        /// buscarsolicitudesgr
        /// Description: Obtiene la Lista Solicitudes de Grúas.
        /// </summary>
        /// <returns List= Lista de Solicitudes de Grúas></returns>
        [HttpPost]
        public dynamic buscarsolicitudesgr()
        {
            return Services.buscarsolicitudesgr();
        }

        /// <summary>
        /// actparams
        /// Description: Método para Atualizar el Tope Máximo de Solicitudes para todos los Agentes.
        /// </summary>
        /// <param val=json de la Entidad NumSolAgentsParams></param>
        /// <returns string = Mensaje Exitoso o Errores de la Actualización.</returns>
        [HttpPost]
        public dynamic actparams(NumSolAgentsParams val)
        {
            return Services.actparams(val);
        }

        /// <summary>
        /// consultartopeac
        /// Description: Obtiene el parámetro Tope Actual para el Máximo de Solicitudes por Agente.
        /// </summary>
        /// <returns List= Lista de Tope Máximo parametrizado para todos los Agentes></returns>
        [HttpPost]
        public dynamic consultartopeac()
        {
            return Services.consultartopeac();
        }

        /// <summary>
        /// constope
        /// Description: Método para Verificar si el Agente ya alcanzó el Tope Máximo Parametrizado de Solicitudes para todos los Agentes.
        /// </summary>
        /// <param val=json de la Entidad AspNetUsersParams></param>
        /// <returns string = Mensaje de si se llegó al tope, o ya se va a alcanzar para el agente buscado</returns>
        [HttpPost]
        public dynamic constope(AspNetUsersParams val)
        {
            return Services.constope(val);
        }

        /// <summary>
        /// consultarnumgr
        /// Description: Método para Obtener el Número de Grúa de la Solicitud que se está creando.
        /// </summary>
        /// <returns NumGruasSol= Objeto con el Número de Grúa de la Solicitud en trámite></returns>
        [HttpPost]
        public dynamic consultarnumgr()
        {
            return Services.consultarnumgr();
        }

        /// <summary>
        /// actnumgr
        /// Description: Método para Actualizar el Número de Grúa de la Solicitud en Trámite.
        /// </summary>
        /// <param val=json de la Entidad NumGruasSolParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic actnumgr(NumGruasSolParams val)
        {
            return Services.actnumgr(val);
        }

        /// <summary>
        /// conscinmo
        /// Description: Método para Obtener la Lista de las Causas de Cancelación.
        /// </summary>
        /// <returns List= Lista con las Causas de Cancelación.></returns>
        [HttpPost]
        public dynamic conscinmo()
        {
            return Services.conscinmo();
        }

        /// <summary>
        /// T_S_Translados
        /// Description: Método para Obtener la Lista de los Tipos de Servicio de Traslado Configurados.
        /// </summary>
        /// <returns List= Lista con los Tipos de Traslado Parametrizados.></returns>
        [HttpPost]
        public dynamic T_S_Translados()
        {
            return Services.T_S_Translados();
        }

        /// <summary>
        /// C_Inmovilizaciones
        /// Description: Método para Obtener la Lista de las Causas de Inmovilización.
        /// </summary>
        /// <returns List= Lista con las causas de Inmovilización.></returns>
        [HttpPost]
        public dynamic C_Inmovilizaciones(C_Inmovilizaciones val)
        {
            return Services.C_Inmovilizaciones(val);
        }

        /// <summary>
        /// Localidades
        /// Description: Método para Obtener la Lista de las Localidades.
        /// </summary>
        /// <returns List= Lista con las Localidades.></returns>
        [HttpPost]
        public dynamic Localidades()
        {
            return Services.Localidades();
        }

        /// <summary>
        /// SentidoViales
        /// Description: Método para Obtener la Lista de los Sentidos Viales.
        /// </summary>
        /// <returns List= Lista con los Sentidos Viales.></returns>
        [HttpPost]
        public dynamic SentidoViales()
        {
            return Services.SentidoViales();
        }

        /// <summary>
        /// TipoGruas
        /// Description: Método para Obtener la Lista de los Tipos de Gruas.
        /// </summary>
        /// <returns List= Lista con los Tipos de Gruas.></returns>
        [HttpPost]
        public dynamic TipoGruas()
        {
            return Services.TipoGruas();
        }

        /// <summary>
        /// TVehiculoInmovilizars
        /// Description: Método para Obtener la Lista de los Tipos de Vehículos.
        /// </summary>
        /// <returns List= Lista con los Tipos de Vehículos.></returns>
        [HttpPost]
        public dynamic TVehiculoInmovilizars()
        {
            return Services.TVehiculoInmovilizars();
        }

        /// <summary>
        /// cacelarservicio
        /// Description: Método para Cancelar Solicitud de Grúa.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic cacelarservicio(SolicitudGruasParams val)
        {
            return Services.cacelarservicio(val);
        }

        /// <summary>
        /// buscarsolicitudesgruser
        /// Description: Método para Buscar Solicitud de Grúa de un usuario específico.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns List = Lista con Solicitudes de Grúa asociadas al usuario.</returns>
        [HttpPost]
        public dynamic buscarsolicitudesgruser(SolicitudGruasParams val)
        {
            return Services.buscarsolicitudesgruser(val);
        }

        /// <summary>
        /// solicitudporid
        /// Description: Método para Buscar Solicitud de Grúa por Id.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns SolicitudGruasParams = Solicitud de Grúa por Id.</returns>
        [HttpPost]
        public dynamic solicitudporid(SolicitudGruasParams val)
        {
            return Services.solicitudporid(val);
        }

        /// <summary>
        /// consultarnumest
        /// Description: Método para Consultar la Parametrización de minutos de Para cierre automático de solicitudes de grúas.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos Parámetro 1></returns>
        [HttpPost]
        public dynamic consultarnumest()
        {
            return Services.consultarnumest();
        }

        /// <summary>
        /// anscons
        /// Description: Método para Consultar la Parametrización de minutos de atención ANS.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos Parámetro 2></returns>
        [HttpPost]
        public dynamic anscons()
        {
            return Services.anscons();
        }

        /// <summary>
        /// horainfcons
        /// Description: Método para Consultar la Parametrización de hora de envío de Correo Solicitudes expiración ANS.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos Parámetro 3></returns>
        [HttpPost]
        public dynamic horainfcons()
        {
            return Services.horainfcons();
        }

        /// <summary>
        /// actualizardiasexp
        /// Description: Método para Actualizar minutos de Para cierre automático de solicitudes de grúas.
        /// </summary>
        /// <param val=json de la Entidad ConfigCierreAutosParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        [HttpPost]
        public dynamic actualizardiasexp(ConfigCierreAutosParams val)
        {
            return Services.actualizardiasexp(val);
        }

        /// <summary>
        /// actans
        /// Description: Método para Actualizar minutos de atención ANS.
        /// </summary>
        /// <param val=json de la Entidad ConfigCierreAutosParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        [HttpPost]
        public dynamic actans(ConfigCierreAutosParams val)
        {
            return Services.actans(val);
        }

        /// <summary>
        /// ActualizarHora
        /// Description: Método para Actualizar hora de envío de Correo Solicitudes expiración ANS.
        /// </summary>
        /// <param val=json de la Entidad ConfigCierreAutosParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        [HttpPost]
        public dynamic ActualizarHora(ConfigCierreAutosParams val)
        {
            return Services.ActualizarHora(val);
        }

        /// <summary>
        /// searchreportroluser
        /// Description: Método para Consultar los Reportes de Total de Usuarios por Roles.
        /// /// </summary>
        /// <returns List= Conteo Usuarios por Roles.></returns>
        [HttpPost]
        public dynamic searchreportroluser()
        {
            return Services.searchreportroluser();
        }

        /// <summary>
        /// ReportUserRoles
        /// Description: Método para Consultar los Lista de Usuarios con Roles.
        /// </summary>
        /// <returns List= Lista con los Usuarios por Roles.></returns>
        [HttpPost]
        public dynamic ReportUserRoles()
        {
            return Services.ReportUserRoles();
        }

        /// <summary>
        /// listmails
        /// Description: Método para Consultar la Lista de Mails parametrizados para enviar de las ANS.
        /// </summary>
        /// <returns List= Lista con los Emails parametrizados.></returns>
        [HttpPost]
        public dynamic listmails()
        {
            return Services.listmails();
        }

        /// <summary>
        /// addmail
        /// Description: Método para Agregar un email a la lista de Emails parametrizados de ANS.
        /// </summary>
        /// <param val=json de la Entidad ListaCorreosANSParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        [HttpPost]
        public dynamic addmail(ListaCorreosANSParams val)
        {
            return Services.addmail(val);
        }

        /// <summary>
        /// elimemail
        /// Description: Método para Eliminar un email a la lista de Emails parametrizados de ANS.
        /// </summary>
        /// <param val=json de la Entidad ListaCorreosANSParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        [HttpPost]
        public dynamic elimemail(ListaCorreosANSParams val)
        {
            return Services.deletemail(val);
        }

        /// <summary>
        /// reportsolagent
        /// Description: Método para Consultar Solicitudes por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación por Agente.
        /// </summary>
        /// <param val=json de la Entidad reportsolagent></param>
        /// <returns List= Reporte por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación por Agente.></returns>
        [HttpPost]
        public dynamic reportsolagent(reportsolagent val)
        {
            val.finicio = DateTime.Parse(val.finicio.ToString()).AddHours(-5);
            val.ffin = DateTime.Parse(val.ffin.ToString()).AddHours(-5);
            return Services.reportsolagent(val);
        }

        /// <summary>
        /// reportsol
        /// Description: Método para Consultar Solicitudes por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación de todos los Agentes.
        /// </summary>
        /// <param val=json de la Entidad reportsolagent></param>
        /// <returns List= Reporte por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación de todos los Agentes.></returns>
        [HttpPost]
        public dynamic reportsol(reportsolagent val)
        {
            val.finicio = DateTime.Parse(val.finicio.ToString()).AddHours(-5);
            val.ffin = DateTime.Parse(val.ffin.ToString()).AddHours(-5);
            return Services.reportsol(val);
        }

        /// <summary>
        /// numsolaagente
        /// Description: Método para Consultar cantidad de Solicitudes por Usuario.
        /// </summary>
        /// <param val=json de la Entidad ReportSolServices></param>
        /// <returns List= Reporte de Cantidad de Solicitudes por Agente.></returns>
        [HttpPost]
        public dynamic numsolaagente(ReportSolServices val)
        {
            return Services.numsolaagente(val);
        }

        /// <summary>
        /// timesol
        /// Description: Método para Consultar tiempo de atención de Solicitudes.
        /// </summary>
        /// <param val=json de la Entidad TimeSoli></param>
        /// <returns List= Reporte Tiempo de Atención de Solicitudes.></returns>
        [HttpPost]
        public dynamic timesol(TimeSoli val)
        {
            return Services.timesol(val);
        }

        /// <summary>
        /// constvxid
        /// Description: Método para Consultar Tipos de Vehículos por Id de Solicitud.
        /// </summary>
        /// <param val=json de la Entidad SolicitudTvs></param>
        /// <returns List= Lista de Tipos de Vehículos por Id de Solicitud.></returns>
        [HttpPost]
        public dynamic constvxid(SolicitudTvs Id)
        {
            return Services.constvxid(Id);
        }

        /// <summary>
        /// searchgrubyid
        /// Description: Método para Consultar Grúas por ID.
        /// </summary>
        /// <param val=json de la Entidad SolicitudTvs></param>
        /// <returns List= Lista de Grúas por ID.></returns>
        [HttpPost]
        public dynamic searchgrubyid(CanGruasSolicitudes Id)
        {
            return Services.searchgrubyid(Id);
        }


        [HttpPost]
        public dynamic SearchGrua(UbicacionGruasParams val)
        {
            return Services.SearchGrua(val);
        }

        [HttpPost]
        public dynamic SearchGruaByCoordenadas(UbicacionGruasParams val)
        {
            return Services.GetListaUbicacionGruas(val.strCoordenadas);
        }


        //Inicio WS SDM
        [HttpPost]
        public RespuestaWS registrarSolicitud(SolicitudServicioDTO JSON)
        {
            var Authorization = HttpContext.Current.Request.Headers["Authorization"];

            return Services.registrarSolicitud(JSON, Authorization);
        }


        [HttpPost]
        public RespuestaWS registrarAtencion(AtencionSolicitudServicioDTO JSON)
        {
            var Authorization = HttpContext.Current.Request.Headers["Authorization"];
            return Services.registrarAtencion(JSON, Authorization);
        }


        [HttpPost]
        public RespuestaWS registrarNovedadServicio(NovedadSolicitudServicioDTO JSON)
        {
            var Authorization = HttpContext.Current.Request.Headers["Authorization"];
            return Services.registrarNovedadServicio(JSON, Authorization);
        }

        
        [AllowAnonymous]
        [HttpPost]
        public TokenDTO login(LoginAutenticationWS user)
        {
            if (user.usuario == null || user.password == null)
            {
                return null;
            }

            LoginAutenticationWS rU = new LoginAutenticationWS();
            rU.usuario = "UserWebServices";
            rU.password = "AutWs$2018$";
            bool credU = rU.usuario.Equals(user.usuario);
            bool credP = rU.password.Equals(user.password);

            if (!credU || !credP) return null;
            else return Services.login(user.usuario);
        }

    }
}