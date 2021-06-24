using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Entitys.DTOs;
using Services.Bussiness;
using Microsoft.Owin.Security.OAuth;

namespace Gruas.Controllers.Apis
{
    public class RegistersController : ApiBaseController
    {
        FormsServices Services = new FormsServices();

        /// <summary>
        /// GetUsers
        /// Description: Obtiene la Lista de Usuarios de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Usuarios></returns>
        [HttpPost]
        public dynamic GetUsers()
        {
            return Services.GetUsers();
        }

        /// <summary>
        /// GetRoles
        /// Description: Obtiene la Lista de Roles de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Roles></returns>
        [HttpPost]
        public dynamic GetRoles()
        {
            return Services.GetRoles();
        }

        /// <summary>
        /// registerrol
        /// Description: Asigna un Rol a un Usuario.
        /// </summary>
        /// <param valor=json de la Entidad AspNetUserRolesParmas></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic registerrol(AspNetUserRolesParmas valor)
        {
            return Services.Registerrol(valor);
        }

        /// <summary>
        /// GetMenus
        /// Description: Obtiene la Lista de Menús de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Menus></returns>
        [HttpPost]
        public dynamic GetMenus()
        {
            return Services.GetMenus();
        }

        /// <summary>
        /// ObMenusRol
        /// Description: Obtiene los Menús Principales asociados a un Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuRolesParams></param>
        /// <returns List= Lista de Menús Principales></returns>
        [HttpPost]
        public dynamic ObMenusRol(AspNetMenuRolesParams valor)
        {
            return Services.ObMenusRol(valor);
        }

        /// <summary>
        /// Menusfilter
        /// Description: Obtiene los Menús Secundarios asociados a un Menú por Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuRolesParams></param>
        /// <returns List= Lista de Menús Secundarios asociados a un Menú por Rol></returns>
        [HttpPost]
        public dynamic Menusfilter(AspNetMenusParams valor)
        {
            return Services.Menusfilter(valor);
        }

        /// <summary>
        /// GetMenusall
        /// Description: Obtiene la Lista Todos los Menús.
        /// </summary>
        /// <returns List= Lista de Roles></returns>
        [HttpPost]
        public dynamic GetMenusall()
        {
            return Services.GetMenusall();
        }

        /// <summary>
        /// MenusUsers
        /// Description: Obtiene los Menús asociados a un Usuario específicamente.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuUsersParams></param>
        /// <returns List= Lista de Menús asociados al Usuario></returns>
        [HttpPost]
        public dynamic MenusUsers(AspNetMenuUsersParams valor)
        {
            return Services.MenusUsers(valor);
        }

        /// <summary>
        /// savechanges
        /// Description: Actualiza la asignación de Menús a Usuarios.
        /// </summary>
        /// <param valor=json de la Entidad DetalleProjectUser></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic savechanges(DetalleProjectUser valor)
        {

            return Services.savechanges(valor);
        }

        /// <summary>
        /// RegRequest
        /// Description: Registra y/o crea un Nuevo Usuario.
        /// </summary>
        /// <param valor=json de la Entidad RequestUsersParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic RegRequest(RequestUsersParams valor)
        {

            return Services.RegRequest(valor);
        }

        /// <summary>
        /// GetSolicitudes
        /// Description: Obtiene la Lista Solcitudes de Creación de nuevos Usuarios.
        /// </summary>
        /// <returns List= Lista de Solicitud de Creación de Usuarios></returns>
        [HttpPost]
        public dynamic GetSolicitudes()
        {
            return Services.GetSolicitudes();
        }

        /// <summary>
        /// RechazarReq
        /// Description: Método para rechazar una solicitud de creación de Nuevo Usuario.
        /// </summary>
        /// <param valor=json de la Entidad RequestUsersParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic RechazarReq(RequestUsersParams itm)
        {
            return Services.RechazarReq(itm);
        }
    }
}