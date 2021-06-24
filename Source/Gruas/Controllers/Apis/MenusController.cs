using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Bussiness;
using Services.Entitys.DTOs;

namespace Gruas.Controllers.Apis
{
    public class MenusController : ApiBaseController
    {
        FormsServices Services = new FormsServices();

        /// <summary>
        /// setRol
        /// Description: Registra un nuevo Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic setRol(AspNetRolesParams valor)
        {
            return Services.Registernewrol(valor);
        }

        /// <summary>
        /// FilterRolGet
        /// Description: Devuelve lista de Roles de acuerdo al filtro.
        /// </summary>
        /// <param valor=json de la Entidad AspNetRolesParams></param>
        /// <returns List = Lista de los Roles Disponibles de acuerdo al filtro.</returns>
        [HttpPost]
        public dynamic FilterRolGet(AspNetRolesParams valor)
        {
            return Services.FilterRolGet(valor);
        }

        /// <summary>
        /// registermenurol
        /// Description: Registra un menú asociado a un Rol.
        /// </summary>
        /// <param valor=json de la Entidad DetalleRolMenu></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic registermenurol(DetalleRolMenu valor)
        {

            return Services.registermenurol(valor);
        }

        /// <summary>
        /// GetMenus
        /// Description: Obtiene los menús a pintar de acuerdo al Usuario y Rol.
        /// </summary>
        /// <returns List= Lista de Menús></returns>
        [HttpGet]
        public dynamic GetMenus()
        {
            return Services.GetMenus();
        }

        /// <summary>
        /// Menusnew
        /// Description: Crea un nuevo Menú.
        /// </summary>
        /// <param menusparams=json de la Entidad AspNetMenusParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic Menusnew(AspNetMenusParams menusparams)
        {
            return Services.Menusnew(menusparams);
        }

        /// <summary>
        /// inactivarelement
        /// Description: Método usado para Inactivar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic inactivarelement(AspNetRolesParams rol)
        {
            return Services.inactivarelement(rol);
        }

        /// <summary>
        /// actelement
        /// Description: Método usado para Activar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic actelement(AspNetRolesParams rol)
        {
            return Services.actelement(rol);
        }

        /// <summary>
        /// actgen
        /// Description: Método usado para Actualizar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        [HttpPost]
        public dynamic actgen(AspNetRolesParams rol)
        {
            return Services.actgen(rol);
        }
    }
}