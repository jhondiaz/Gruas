using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Entitys.DTOs;
using Services.Entitys.Entities;
using Services.DataAccess;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net.Mail;
using System.Net.Mime;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Net;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace Services.Bussiness
{
    public class FormsServices
    {
        Context Context = new Context();

        static TimeZoneInfo horazone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        DateTime horacol = TimeZoneInfo.ConvertTime(DateTime.Now, horazone);

        private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        #region Registers Users

        /// <summary>
        /// GetUsers
        /// Description: Obtiene la Lista de Usuarios de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Usuarios></returns>
        public dynamic GetUsers()
        {
            try
            {

                using (var dataContext = new Context())
                {
                    var valob = (from mAspNetUsers in dataContext.AspNetUsers
                                 from mAspNetRoles in dataContext.AspNetRoles
                                 from mAspNetUserRoles in dataContext.AspNetUserRoles
                                 where mAspNetUserRoles.UserId == mAspNetUsers.Id
                                 && mAspNetUserRoles.RoleId == mAspNetRoles.Id
                                 select new
                                 {
                                     Id = mAspNetUsers.Id,
                                     Email = mAspNetUsers.Email,
                                     EmailConfirmed = mAspNetUsers.EmailConfirmed,
                                     PhoneNumber = mAspNetUsers.PhoneNumber,
                                     PhoneNumberConfirmed = mAspNetUsers,
                                     TwoFactorEnabled = mAspNetUsers.TwoFactorEnabled,
                                     LockoutEnabled = mAspNetUsers.LockoutEnabled,
                                     UserName = mAspNetUsers.UserName,
                                     firstName = mAspNetUsers.firstName,
                                     Rol = mAspNetRoles.Name,
                                     TipoDocumento = mAspNetUsers.TipoDocumento,
                                     NumeroDocumento = mAspNetUsers.NumeroDocumento,
                                     PlacaAgente = mAspNetUsers.PlacaAgente,
                                     NombreJefe = mAspNetUsers.NombreJefe,
                                     TelefonoJefe = mAspNetUsers.TelefonoJefe,
                                     Entidad = mAspNetUsers.Entidad,
                                     DiasExpiracion = mAspNetUsers.DiasExpiracion

                                 }).ToList();
                    return valob;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// GetRoles
        /// Description: Obtiene la Lista de Roles de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Roles></returns>
        public dynamic GetRoles()
        {
            try
            {
                var item = Context.AspNetRoles.Select(t => new AspNetRolesParams
                {
                    Id = t.Id,
                    Name = t.Name,
                    Descripcion = t.Descripcion,
                    Activo = t.Activo

                }).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// inactivarelement
        /// Description: Método usado para Inactivar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic inactivarelement(AspNetRolesParams roles)
        {
            try
            {
                AspNetRoles obj = new AspNetRoles();
                AspNetMenuRoles obj1 = new AspNetMenuRoles();
                var mensaje = 0;

                AspNetRoles ob = Context.AspNetRoles.Where(tr => tr.Id == roles.Id).FirstOrDefault();

                ob.Activo = false;
                Context.SaveChanges();

                //return mensaje + " ASOCIACIONES A MENÚ PRINCIPAL ELIMINADAS, ROL INACTIVADO CORRECTAMENTE.";
                return "ROL INACTIVADO CORRECTAMENTE.";
            }
            catch (Exception ex)
            {
                return "Error Eliminando Rol " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// actelement
        /// Description: Método usado para Activar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic actelement(AspNetRolesParams roles)
        {
            try
            {
                AspNetRoles obj = new AspNetRoles();

                AspNetRoles ob = Context.AspNetRoles.Where(tr => tr.Id == roles.Id).FirstOrDefault();

                ob.Activo = true;
                Context.SaveChanges();

                return "ROL ACTIVADO CORRECTAMENTE.";
            }
            catch (Exception ex)
            {
                return "Error actualizando Rol " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// actgen
        /// Description: Método usado para Actualizar un Rol.
        /// </summary>
        /// <param rol=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic actgen(AspNetRolesParams roles)
        {
            try
            {
                AspNetRoles obj = new AspNetRoles();
                AspNetRoles ob = Context.AspNetRoles.Where(tr => tr.Id == roles.Id).FirstOrDefault();

                ob.Name = roles.Name;
                ob.Descripcion = roles.Descripcion;
                Context.SaveChanges();

                return "ROL ACTUALIZADO CORRECTAMENTE.";
            }
            catch (Exception ex)
            {
                return "Error actualizando Rol " + ex;
                throw ex;
            }
        }


        /// <summary>
        /// GetMenus
        /// Description: Obtiene la Lista de Menús de la Aplicación Grúas.
        /// </summary>
        /// <returns List= Lista de Menus></returns>
        public dynamic GetMenus()
        {
            try
            {
                var item = Context.AspNetMenus.Select(t => new AspNetMenusParams
                {
                    Id = t.Id,
                    Menu = t.Menu
                }).Where(t => t.Menu != null).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic obteneridrol(string valor)
        {
            return Context.AspNetRoles.Where(t => t.Name == valor).FirstOrDefault();
        }

        /// <summary>
        /// ObMenusRol
        /// Description: Obtiene los Menús Principales asociados a un Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuRolesParams></param>
        /// <returns List= Lista de Menús Principales></returns>
        public dynamic ObMenusRol(AspNetMenuRolesParams valor)
        {

            try
            {
                var name = obteneridrol(valor.RoleId);

                if (name.Activo == false)
                {
                    return 0;
                }
                else
                {
                    string idrol = name.Id;

                    var valob = (from mAspNetMenuRoles in Context.AspNetMenuRoles
                                 from mAspNetRoles in Context.AspNetRoles
                                 from mAspNetMenus in Context.AspNetMenus
                                 where mAspNetMenuRoles.RoleId == mAspNetRoles.Id
                                 && mAspNetMenuRoles.IdMenu == mAspNetMenus.Id
                                 && mAspNetMenuRoles.RoleId == idrol
                                 orderby mAspNetMenus.OrderMenu
                                 select new
                                 {
                                     Id = mAspNetMenuRoles.Id,
                                     Name = mAspNetMenus.Menu,
                                     Idmenu = mAspNetMenus.Id,
                                     Icon = mAspNetMenus.Icon
                                 }).ToList();

                    List<DetalleMenusLog> consulta = new List<DetalleMenusLog>();


                    for (int i = 0; i < valob.Count; i++)
                    {
                        DetalleMenusLog valoreslog = new DetalleMenusLog();
                        List<AspNetMenusParams> paramapmenu = new List<AspNetMenusParams>();

                        valoreslog.Id = valob[i].Id;
                        valoreslog.Name = valob[i].Name;
                        valoreslog.Idmenu = valob[i].Idmenu;
                        valoreslog.Icon = valob[i].Icon;
                        var sucon = SubMenuConsulta(valob[i].Idmenu, valor.IdUser);

                        for (int j = 0; j < sucon.Count; j++)
                        {
                            var ass = JsonConvert.SerializeObject(sucon[j]);
                            var total = JsonConvert.DeserializeObject<AspNetMenusParams>(ass);
                            paramapmenu.Add(total);
                        }
                        valoreslog.SubMenu = paramapmenu;
                        consulta.Add(valoreslog);
                    }

                    return consulta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic SubMenuFilter(string valor, string idmen)
        {

            try
            {
                var valob = (from mAspNetMenuUsers in Context.AspNetMenuUsers
                             from mAspNetUsers in Context.AspNetUsers
                             from mAspNetMenus in Context.AspNetMenus
                             where mAspNetMenuUsers.IdMenu == mAspNetMenus.Id
                             && mAspNetMenuUsers.IdUser == valor
                             && mAspNetMenus.IdSubMenu == idmen
                             select new
                             {
                                 Id = mAspNetMenuUsers.Id,
                                 IdMenu = mAspNetMenuUsers.IdMenu,
                                 IdUser = mAspNetMenuUsers.IdUser

                             }).Distinct().ToList();

                return valob;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic SubMenuConsulta(string valor, string iduser)
        {

            try
            {
                var valob = (from mAspNetMenuUsers in Context.AspNetMenuUsers
                             from mAspNetUsers in Context.AspNetUsers
                             from mAspNetMenus in Context.AspNetMenus
                             where mAspNetMenuUsers.IdMenu == mAspNetMenus.Id
                             && mAspNetMenus.IdSubMenu == valor
                             && mAspNetMenuUsers.IdUser == iduser
                             orderby mAspNetMenus.OrderMenu descending
                             select new
                             {
                                 Id = mAspNetMenus.Id,
                                 Menu = mAspNetMenus.Menu,
                                 OrderMenu = mAspNetMenus.OrderMenu,
                                 Icon = mAspNetMenus.Icon,
                                 IdSubMenu = mAspNetMenus.IdSubMenu,
                                 SubMenu = mAspNetMenus.SubMenu,
                                 ControllerName = mAspNetMenus.ControllerName,
                                 ActionName = mAspNetMenus.ActionName,
                                 AUrl = mAspNetMenus.AUrl,
                                 ATemplateUrl = mAspNetMenus.ATemplateUrl,
                                 AController = mAspNetMenus.AController,
                                 CreateDate = mAspNetMenus.CreateDate,
                                 Mostrar = mAspNetMenus.Mostrar
                             }).Distinct().ToList();

                return valob.OrderBy(t => t.OrderMenu).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetMenusall
        /// Description: Obtiene la Lista Todos los Menús.
        /// </summary>
        /// <returns List= Lista de Roles></returns>
        public dynamic GetMenusall()
        {

            try
            {
                var item = Context.AspNetMenus.Select(t => new AspNetMenusParams
                {
                    Id = t.Id,
                    Menu = t.Menu,
                    IdSubMenu = t.IdSubMenu,
                    SubMenu = t.SubMenu,
                    OrderMenu = t.OrderMenu,
                    Icon = t.Icon
                }).OrderBy(t => new { t.IdSubMenu, t.OrderMenu }).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// MenusUsers
        /// Description: Obtiene los Menús asociados a un Usuario específicamente.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuUsersParams></param>
        /// <returns List= Lista de Menús asociados al Usuario></returns>
        public dynamic MenusUsers(AspNetMenuUsersParams valor)
        {

            try
            {
                var item = Context.AspNetMenuUsers.Select(t => new AspNetMenuUsersParams
                {
                    Id = t.Id,
                    IdMenu = t.IdMenu,
                    IdUser = t.IdUser,
                    CreateDate = t.CreateDate
                }).Where(t => t.IdUser == valor.IdUser).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Menusfilter
        /// Description: Obtiene los Menús Secundarios asociados a un Menú por Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetMenuRolesParams></param>
        /// <returns List= Lista de Menús Secundarios asociados a un Menú por Rol></returns>
        public dynamic Menusfilter(AspNetMenusParams valor)
        {

            try
            {
                var item = Context.AspNetMenus.Select(t => new AspNetMenusParams
                {
                    Id = t.Id,
                    Menu = t.Menu,
                    IdSubMenu = t.IdSubMenu,
                    SubMenu = t.SubMenu,
                    OrderMenu = t.OrderMenu,
                    Icon = t.Icon
                }).Where(t => t.IdSubMenu == valor.Id).OrderBy(t => t.OrderMenu).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic GetMenusFilter(string IdSend)
        {
            try
            {
                var item = Context.AspNetMenuRoles.Select(t => new AspNetMenuRolesParams
                {
                    Id = t.Id,
                    RoleId = t.RoleId,
                    IdMenu = t.IdMenu
                }).Where(t => t.RoleId == IdSend).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// registerrol
        /// Description: Asigna un Rol a un Usuario.
        /// </summary>
        /// <param valor=json de la Entidad AspNetUserRolesParmas></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic Registerrol(AspNetUserRolesParmas valor)
        {
            try
            {
                AspNetUserRolesParmas ParamUserRol = valor;
                AspNetUserRoles obj = new AspNetUserRoles();

                //SetParameters
                obj.UserId = ParamUserRol.UserId;
                obj.RoleId = ParamUserRol.RoleId;

                Context.AspNetUserRoles.Add(obj);
                Context.SaveChanges();

                return "Rol Asignado Correctamente.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// setRol
        /// Description: Registra un nuevo Rol.
        /// </summary>
        /// <param valor=json de la Entidad AspNetRolesParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic Registernewrol(AspNetRolesParams valor)
        {
            try
            {

                var lisrol = GetRoles();

                foreach (var item in lisrol)
                {
                    if (item.Name == valor.Name)
                    {
                        return "EL ROL YA SE ENCUENTRA CREADO.";
                    }
                }

                AspNetRoles obj = new AspNetRoles();

                //SetParameters
                obj.Id = Guid.NewGuid().ToString();
                obj.Name = valor.Name;
                obj.Descripcion = valor.Descripcion;
                obj.Activo = true;

                Context.AspNetRoles.Add(obj);
                Context.SaveChanges();

                return "ROL GUARDADO CORRECTAMENTE.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FilterRolGet
        /// Description: Devuelve lista de Roles de acuerdo al filtro.
        /// </summary>
        /// <param valor=json de la Entidad AspNetRolesParams></param>
        /// <returns List = Lista de los Roles Disponibles de acuerdo al filtro.</returns>
        public dynamic FilterRolGet(AspNetRolesParams valor)
        {
            try
            {
                var item = Context.AspNetMenuRoles.Select(t => new AspNetMenuRolesParams
                {
                    Id = t.Id,
                    IdMenu = t.IdMenu,
                    RoleId = t.RoleId

                }).Where(t => t.RoleId == valor.Id).ToList();

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// registermenurol
        /// Description: Registra un menú asociado a un Rol.
        /// </summary>
        /// <param valor=json de la Entidad DetalleRolMenu></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>  
        public dynamic registermenurol(DetalleRolMenu valor)
        {

            try
            {

                List<string> list1 = new List<string>();
                List<string> list2 = new List<string>();
                List<string> list3 = new List<string>();
                List<string> list4 = new List<string>();

                var listmenus = GetMenusFilter(valor.IdRol);
                var listmenus2 = valor.IdsMenus.Split(',');

                foreach (var item in listmenus)
                {
                    list1.Add(Convert.ToString(item.IdMenu));
                }
                foreach (var itm in listmenus2)
                {
                    list2.Add(itm);
                }
                foreach (var itr in list2)
                {
                    if (!list1.Contains(itr))
                    {
                        list3.Add(itr);
                    }
                }
                foreach (var itdr in list1)
                {
                    if (!list2.Contains(itdr))
                    {
                        list4.Add(itdr);
                    }
                }
                if (list3.Contains(""))
                {
                    list3.Remove("");
                }

                if (list4.Contains(""))
                {
                    list4.Remove("");
                }

                if (list4.Count != 0)
                {
                    foreach (var itl in list4)
                    {
                        var test = Context.AspNetMenuRoles.Select(t => t).ToList();
                        var list = Context.AspNetMenuRoles.Where(m => m.RoleId == valor.IdRol && m.IdMenu == itl);
                        foreach (AspNetMenuRoles bar in list)
                            Context.AspNetMenuRoles.Remove(bar);
                        Context.SaveChanges();
                    }

                    return "ROL ACTUALIZADO CORRECTAMENTE";
                }
                else if (list3.Count != 0)
                {

                    foreach (var items in list3)
                    {
                        AspNetMenuRoles obj = new AspNetMenuRoles();

                        //SetParameters
                        obj.Id = Guid.NewGuid().ToString();
                        obj.IdMenu = items;
                        obj.RoleId = valor.IdRol;
                        obj.CreateDate = horacol;

                        Context.AspNetMenuRoles.Add(obj);
                        Context.SaveChanges();
                    }

                    return "ROL GUARDADO CORRECTAMENTE.";
                }

                return "NO SE REALIZO NINGUN CAMBIO.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Menusnew
        /// Description: Crea un nuevo Menú.
        /// </summary>
        /// <param menusparams=json de la Entidad AspNetMenusParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic Menusnew(AspNetMenusParams menusparams)
        {


            try
            {
                AspNetMenus obj = new AspNetMenus();

                obj.Id = Guid.NewGuid().ToString();
                obj.Menu = menusparams.Menu;
                obj.OrderMenu = menusparams.OrderMenu;
                obj.Icon = menusparams.Icon;
                obj.IdSubMenu = menusparams.IdSubMenu;
                obj.SubMenu = menusparams.SubMenu;
                obj.AUrl = menusparams.AUrl;
                obj.ATemplateUrl = menusparams.ATemplateUrl;
                obj.AController = menusparams.AController;
                obj.Mostrar = true;
                obj.CreateDate = horacol;

                Context.AspNetMenus.Add(obj);
                Context.SaveChanges();

                return "MENÚ CREADO CORRECTAMENTE.";
            }

            catch (Exception ex)
            {
                return "ERROR -- " + ex;
                throw;
            }
        }

        /// <summary>
        /// savechanges
        /// Description: Actualiza la asignación de Menús a Usuarios.
        /// </summary>
        /// <param valor=json de la Entidad DetalleProjectUser></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic savechanges(DetalleProjectUser valor)
        {

            try
            {

                List<string> list1 = new List<string>();
                List<string> list2 = new List<string>();
                List<string> list3 = new List<string>();
                List<string> list4 = new List<string>();

                var listmenus = SubMenuFilter(valor.IdUser, valor.IdMenu);
                var listmenus2 = valor.IdProject.Split(',');

                foreach (var item in listmenus)
                {
                    list1.Add(Convert.ToString(item.IdMenu));
                }
                foreach (var itm in listmenus2)
                {
                    list2.Add(itm);
                }
                foreach (var itr in list2)
                {
                    if (!list1.Contains(itr))
                    {
                        list3.Add(itr);
                    }
                }
                foreach (var itdr in list1)
                {
                    if (!list2.Contains(itdr))
                    {
                        list4.Add(itdr);
                    }
                }
                if (list3.Contains(""))
                {
                    list3.Remove("");
                }

                if (list4.Contains(""))
                {
                    list4.Remove("");
                }

                if (list4.Count != 0)
                {
                    foreach (var itl in list4)
                    {
                        var list = Context.AspNetMenuUsers.Where(m => m.IdMenu == itl && m.IdUser == valor.IdUser);
                        foreach (AspNetMenuUsers bar in list)
                            Context.AspNetMenuUsers.Remove(bar);
                        Context.SaveChanges();
                    }

                    return "PERMISOS ACTUALIZADOS CORRECTAMENTE";
                }
                else if (list3.Count != 0)
                {

                    foreach (var items in list3)
                    {
                        AspNetMenuUsers obj = new AspNetMenuUsers();


                        obj.Id = Guid.NewGuid().ToString();
                        obj.IdUser = Guid.Parse(valor.IdUser).ToString();
                        obj.IdMenu = Guid.Parse(items).ToString();
                        obj.CreateDate = horacol;

                        Context.AspNetMenuUsers.Add(obj);
                        Context.SaveChanges();
                    }

                    return "MENU ASIGNADO CORRECTAMENTE.";
                }

                return "NO SE REALIZO NINGUN CAMBIO.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RegRequest
        /// Description: Registra y/o crea un Nuevo Usuario.
        /// </summary>
        /// <param valor=json de la Entidad RequestUsersParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic RegRequest(RequestUsersParams valor)
        {
            try
            {

                RequestUsers obj = new RequestUsers();


                obj.Id = Guid.NewGuid().ToString();
                obj.firstName = valor.firstName;
                obj.UserName = valor.Email;
                obj.Email = valor.Email;
                obj.PhoneNumber = valor.PhoneNumber;
                obj.TipoDocumento = valor.TipoDocumento;
                obj.NumeroDocumento = valor.NumeroDocumento;
                obj.PlacaAgente = valor.PlacaAgente;
                obj.NombreJefe = valor.NombreJefe;
                obj.TelefonoJefe = valor.TelefonoJefe;
                obj.Entidad = valor.Entidad;
                obj.Agente = valor.Agente;
                obj.Validado = 0;
                obj.FechaSolicitud = horacol;

                Context.RequestUsers.Add(obj);
                Context.SaveChanges();

                var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                inline.ContentId = Guid.NewGuid().ToString();

                SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                server.EnableSsl = true;
                MailMessage mnsj = new MailMessage();
                mnsj.Subject = "Creación de Usuario";
                mnsj.To.Add(new MailAddress(valor.Email));
                mnsj.From = new MailAddress(valor.Email, "Plataforma Gruas");
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                body += "<BODY><div style='width: 677px;height: 64px;'>";
                body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                body += "</div>";
                body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                body += "<h1 style='color: cadetblue'>Solicitud de Creación</h1><h3 style='color:#878788'>Buen Día " + valor.firstName + "</h3>";
                body += "<h4 style='color:#878788'>La solicitud de creación de usuario para el acceso a la plataforma de grúas fue registrada de forma satisfactoria, se notificara una vez creados los datos de acceso.</h4>";
                body += "</DIV></BODY></HTML>";

                ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                mnsj.AlternateViews.Add(alternate);
                alternate.LinkedResources.Add(inline);
                mnsj.Body = body;
                server.Send(mnsj);


                return "Solicitud Registrada Satisfactoriamente.";
            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
                throw;
            }
        }

        /// <summary>
        /// GetSolicitudes
        /// Description: Obtiene la Lista Solcitudes de Creación de nuevos Usuarios.
        /// </summary>
        /// <returns List= Lista de Solicitud de Creación de Usuarios></returns>
        public dynamic GetSolicitudes()
        {
            try
            {
                var query = Context.RequestUsers.Where(t => t.Validado == 0).ToList();

                return query;
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// RechazarReq
        /// Description: Método para rechazar una solicitud de creación de Nuevo Usuario.
        /// </summary>
        /// <param valor=json de la Entidad RequestUsersParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic RechazarReq(RequestUsersParams model)
        {
            try
            {
                Context context = new Context();
                RequestUsers ob = context.RequestUsers.Where(t => t.Id == model.Id).FirstOrDefault();

                ob.Validado = 2;
                context.SaveChanges();

                var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                inline.ContentId = Guid.NewGuid().ToString();

                SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                server.EnableSsl = true;
                MailMessage mnsj = new MailMessage();
                mnsj.Subject = "Creación de Usuario";
                mnsj.To.Add(new MailAddress(ob.Email));
                mnsj.From = new MailAddress(ob.Email, "Plataforma Gruas");
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                body += "<BODY><div style='width: 677px;height: 64px;'>";
                body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                body += "</div>";
                body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                body += "<h1 style='color: cadetblue'>Solicitud de Creación</h1><h3 style='color:#878788'>Buen Día " + ob.firstName + "</h3>";
                body += "<h4 style='color:#878788'>La solicitud de creación de usuario para el acceso a la plataforma de grúas fue rechazada, Para mas información contacte el administrador.</h4>";
                body += "</DIV></BODY></HTML>";

                ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                mnsj.AlternateViews.Add(alternate);
                alternate.LinkedResources.Add(inline);
                mnsj.Body = body;
                server.Send(mnsj);


                return "Solicitud Rechazada Satisfactoriamente.";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// infcodes
        /// Description: Obtiene la Lista Códigos de Infracción.
        /// </summary>
        /// <returns List= Lista de Códigos de Infracción></returns>
        public dynamic infcodes()
        {
            try
            {
                var query = Context.CodigosInfracciones.ToList();
                return query;
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// searchagent
        /// Description: Método para Buscar un Agente por Documento o Placa.
        /// </summary>
        /// <param values=json de la Entidad AspNetUsersParams></param>
        /// <returns AspNetUsersParams = Objeto asociado a la Búsqueda.</returns>
        public dynamic searchagent(AspNetUsersParams usr)
        {
            try
            {
                if (usr.Tipo == "Placa")
                {
                    var query = Context.AspNetUsers.Where(t => t.PlacaAgente == usr.PlacaAgente && t.Agente == true).FirstOrDefault();
                    return query;
                }
                else
                {
                    var query = Context.AspNetUsers.Where(t => t.NumeroDocumento == usr.NumeroDocumento && t.Agente == true).FirstOrDefault();
                    return query;
                }
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// saveinfgru
        /// Description: Método para Registrar una solicitud de Grúas.
        /// </summary>
        /// <param values=json de la Entidad SolicitudGruasParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic saveinfgru(SolicitudGruasParams values)
        {
            try
            {
                if (values.Direccion == "" || values.Direccion == null)
                {
                    return "Verifique la dirección, Error guardando la solicitud.";
                }

                var consulta = Context.CodigosInfracciones.Where(t => t.Codigo == values.Codigo_de_infraccion).FirstOrDefault();

                if (consulta == null)
                {
                    return "El código de infracción no existe, favor validar.";
                }
                else
                {

                    var query = Context.NumSolAgents.Where(t => t.UserId == values.Id_Usuario).FirstOrDefault();

                    var f1 = "";
                    var f2 = "";
                    var conteo = 0;
                    var tope = 0;
                    var IDS = 0;

                    if (query != null)
                    {
                        f1 = query.Fecha.ToString().Split(' ')[0];
                        f2 = horacol.ToString().Split(' ')[0];

                        conteo = query.Conteo;
                        tope = query.Tope;
                    }

                    if ((conteo != 0 && tope != 0) && (conteo == tope || conteo > tope) && f1 == f2)
                    {
                        return "Ha alcanzado el numero maximo de solicitudes por día, no es posible registrar la solicitud.";
                    }
                    else
                    {
                        var expsol = Context.ConfigCierreAutos.Where(t => t.Id == "1").FirstOrDefault();

                        SolicitudGruas obj = new SolicitudGruas();
                        SolicitudTvs objtvs = new SolicitudTvs();

                        var lcs = Context.Localidades.FirstOrDefault(t => t.Nombre.Contains(values.Localidad));

                        ValuesWS ValoresWs = new ValuesWS();


                        ValoresWs.entidad = "2";
                        ValoresWs.fechaSolicitud = values.Fecha_hora_envio_informacion;
                        ValoresWs.tipoOrden = "1";
                        ValoresWs.tipoTraslado = int.Parse(values.Tipo_de_servicio_de_traslado);
                        ValoresWs.causaInmovilizacion = int.Parse(values.Causa_de_inmovilizacion);
                        ValoresWs.tipoInfraccion = 1;
                        ValoresWs.direccionGeo = values.Direccion_georreferencial;
                        ValoresWs.tipoZona = int.Parse(values.Tipo_Zona);
                        ValoresWs.direccionUrbana = values.Direccion;
                        ValoresWs.tipoVehiculo = new tipoVehiculos[values.CantVti.Count];
                        ValoresWs.horaTraslado = null;
                        ValoresWs.fechaTraslado = null;
                        ValoresWs.nroOrden = null;
                        ValoresWs.fechaSolicitud = horacol.Date;

                        for (int i = 0; i < values.CantVti.Count; i++)
                        {
                            if (values.CantVti[i] != null)
                            {
                                ValoresWs.tipoVehiculo[i] = new tipoVehiculos();
                                ValoresWs.tipoVehiculo[i].tipoVehiculo = values.CantVti[i].Split(':')[0];
                                ValoresWs.tipoVehiculo[i].cantidad = values.CantVti[i].Split(':')[1];
                            }

                        }

                        if (values.CantVti.Count == 0)
                        {
                            ValoresWs.tipoVehiculo = null;
                        }

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["WsSolicitudServicio"]);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            var json = JsonConvert.SerializeObject(ValoresWs);

                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var NumOrdenRespuesta = 0;

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            var Respuesta = JsonConvert.DeserializeObject<RespuestaWS>(result);


                            List<string> MensajesError = new List<string>();

                            if (Respuesta.respuesta == null)
                            {
                                MensajesError.Add("0");
                                foreach (var item in Respuesta.mensajes)
                                {
                                    MensajesError.Add(item.mensaje);
                                }

                                return MensajesError.ToList();
                            }
                            else
                            {
                                var EstRes = "";
                                var select = Context.ConfigCierreAutos.FirstOrDefault();

                                if (Respuesta.respuesta.confirmacion == "2")
                                {
                                    EstRes = "RECHAZADA";
                                }
                                else if (Respuesta.respuesta.confirmacion == "1")
                                {
                                    EstRes = "APROBADA";
                                    var anstime = Context.ConfigCierreAutos.Where(t => t.Id == "2").FirstOrDefault();
                                    obj.Fecha_Cierre_Auto = horacol.AddMinutes(Convert.ToDouble(select.Horas));
                                    obj.ANSTime = horacol.AddMinutes(Convert.ToDouble(anstime.Horas));
                                }

                                //SetParameters
                                obj.ID_solicitud = -1;
                                obj.Entidad = values.Entidad;
                                obj.Tipo_de_orden_de_servicio = "1";
                                obj.Id_Usuario = values.Id_Usuario;
                                obj.Numero_de_orden_del_servicio = Convert.ToString(Respuesta.respuesta.nroOrden);
                                obj.Tipo_de_servicio_de_traslado = values.Tipo_de_servicio_de_traslado;
                                obj.Causa_de_inmovilizacion = values.Causa_de_inmovilizacion;
                                obj.Codigo_de_infraccion = values.Codigo_de_infraccion;
                                obj.Direccion = values.Direccion;
                                obj.Barrio = values.Barrio;
                                obj.Direccion_georreferencial = values.Direccion_georreferencial;
                                obj.Localidad = lcs.Id;
                                obj.Tipo_Zona = values.Tipo_Zona;
                                obj.Tipo_de_vehiculo_a_inmovilizar = null;
                                obj.Tipo_de_Grua = null;
                                obj.Estado = EstRes;
                                obj.Fecha_y_hora_solicitud_servicio = horacol;
                                obj.Fecha_Cierre_Auto = horacol.AddMinutes(Convert.ToDouble(expsol.Horas));
                                obj.ValANS = false;
                                obj.Causal_de_rechazo = Respuesta.respuesta.causalRechazo;
                                obj.Fecha_y_hora_de_orden_de_servicio = Respuesta.respuesta.fechaOrdenServicio;

                                NumOrdenRespuesta = Respuesta.respuesta.nroOrden;

                                Context.SolicitudGruas.Add(obj);
                                Context.SaveChanges();

                                IDS = obj.ID_solicitud;

                                foreach (var item in values.CantVti)
                                {
                                    if (item != null)
                                    {
                                        objtvs.IdSol = IDS;
                                        objtvs.TipoV = item.Split(':')[0];
                                        objtvs.Cantidad = item.Split(':')[1];

                                        Context.SolicitudTvs.Add(objtvs);
                                        Context.SaveChanges();
                                    }

                                }

                                Estados est = new Estados();
                                est.ID_solicitud = IDS;
                                if (Respuesta.respuesta.confirmacion == "1")
                                {
                                    est.Nombre = "APROBADA";
                                    est.Observaciones = null;
                                }
                                else if (Respuesta.respuesta.confirmacion == "2")
                                {
                                    est.Nombre = "RECHAZADA";
                                    est.Observaciones = Respuesta.respuesta.causalRechazo;
                                }

                                est.Fecha = horacol;
                                Context.Estados.Add(est);
                                Context.SaveChanges();


                                NumSolAgents objec = Context.NumSolAgents.Where(tr => tr.UserId == values.Id_Usuario).FirstOrDefault();

                                if (f1 != f2)
                                {
                                    objec.Conteo = 1;
                                    objec.Fecha = horacol;
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    var valcont = 0;
                                    if (objec != null)
                                    {
                                        valcont = Convert.ToInt32(query.Conteo) + Convert.ToInt32(1);
                                        objec.Conteo = valcont;
                                        objec.Fecha = horacol;
                                        Context.SaveChanges();
                                    }
                                    else
                                    {
                                        valcont = 1;

                                        NumSolAgents objects = new NumSolAgents();
                                        objects.UserId = values.Id_Usuario;
                                        objects.Conteo = valcont;
                                        objects.Fecha = horacol;
                                        Context.NumSolAgents.Add(objects);
                                        Context.SaveChanges();
                                    }


                                }
                            }
                        }

                        return "Solicitud Enviada Satisfactoriamente, El Numero de la Solicitud es: " + IDS + " ,El número de orden es: " + NumOrdenRespuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Se produjo un error, intente nuevamente.";
                throw ex;
            }
        }

        /// <summary>
        /// buscarsolicitudesgr
        /// Description: Obtiene la Lista Solicitudes de Grúas.
        /// </summary>
        /// <returns List= Lista de Solicitudes de Grúas></returns>
        public dynamic buscarsolicitudesgr()
        {
            try
            {
                var query = (from mSolicitudGruas in Context.SolicitudGruas
                             from maspnetusers in Context.AspNetUsers
                             where mSolicitudGruas.Id_Usuario == maspnetusers.Id
                             select new
                             {
                                 ID_solicitud = mSolicitudGruas.ID_solicitud,
                                 Fecha_y_hora_solicitud_servicio = mSolicitudGruas.Fecha_y_hora_solicitud_servicio,
                                 Numero_de_orden_del_servicio = mSolicitudGruas.Numero_de_orden_del_servicio,
                                 Fecha_y_hora_de_orden_de_servicio = mSolicitudGruas.Fecha_y_hora_de_orden_de_servicio,
                                 Usuario = maspnetusers.firstName,
                                 Placa = maspnetusers.PlacaAgente,
                                 Estado = mSolicitudGruas.Estado
                             }).ToList();
                return query.OrderByDescending(t => t.ID_solicitud).ToList();

                //var query = Context.SolicitudGruas.ToList();

                //return query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// actparams
        /// Description: Método para Atualizar el Tope Máximo de Solicitudes para todos los Agentes.
        /// </summary>
        /// <param values=json de la Entidad NumSolAgentsParams></param>
        /// <returns string = Mensaje Exitoso o Errores de la Actualización.</returns>
        public dynamic actparams(NumSolAgentsParams val)
        {
            try
            {
                var list1 = Context.AspNetUsers.Where(t => t.Agente == true).ToList();

                foreach (var itr in list1)
                {
                    var cons = Context.NumSolAgents.Where(t => t.UserId == itr.Id).FirstOrDefault();

                    if (cons == null)
                    {
                        NumSolAgents obj = new NumSolAgents();

                        obj.UserId = itr.Id;
                        obj.Fecha = horacol;
                        obj.Tope = val.Tope;
                        obj.Conteo = 0;

                        Context.NumSolAgents.Add(obj);
                        Context.SaveChanges();
                    }
                    else
                    {
                        NumSolAgents obj = new NumSolAgents();
                        NumSolAgents ob = Context.NumSolAgents.Where(tr => tr.UserId == itr.Id).FirstOrDefault();

                        ob.Tope = val.Tope;
                        Context.SaveChanges();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// buscarsolicitudesgr
        /// Description: Obtiene el parámetro Tope Actual para el Máximo de Solicitudes por Agente.
        /// </summary>
        /// <returns List= Lista de Tope Máximo parametrizado para todos los Agentes></returns>
        public dynamic consultartopeac()
        {
            try
            {
                var query = Context.NumSolAgents.FirstOrDefault();

                if (query != null)
                {
                    return query.Tope;
                }
                else
                {
                    return 0;
                }

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// constope
        /// Description: Método para Verificar si el Agente ya alcanzó el Tope Máximo Parametrizado de Solicitudes para todos los Agentes.
        /// </summary>
        /// <param val=json de la Entidad AspNetUsersParams></param>
        /// <returns string = Mensaje de si se llegó al tope, o ya se va a alcanzar para el agente buscado</returns>
        public dynamic constope(AspNetUsersParams val)
        {
            try
            {
                var query = Context.NumSolAgents.Where(t => t.UserId == val.Id).FirstOrDefault();
                if (query != null)
                {
                    var contval = Convert.ToInt32(query.Conteo) + Convert.ToUInt32(1);

                    var f1 = query.Fecha.ToString().Split(' ')[0];
                    var f2 = horacol.ToString().Split(' ')[0];

                    if (query.Conteo == query.Tope && f1 == f2)
                    {
                        return "Ha alcanzado el numero maximo de solicitudes por día.";
                    }
                    else
                    {
                        if (f1 == f2 && contval == query.Tope)
                        {
                            return "Esta a una solicitud de alcanzar el tope maximo diario de solicitudes.";
                        }
                    }
                }

                return "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// consultartopeac
        /// Description: Método para Obtener el Número de Grúa de la Solicitud que se está creando.
        /// </summary>
        /// <returns NumGruasSol= Objeto con el Número de Grúa de la Solicitud en trámite></returns>
        public dynamic consultarnumgr()
        {
            try
            {
                var query = Context.NumGruasSol.FirstOrDefault();

                if (query != null)
                {
                    return query;
                }
                else
                {
                    return 0;
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// actnumgr
        /// Description: Método para Actualizar el Número de Grúa de la Solicitud en Trámite.
        /// </summary>
        /// <param val=json de la Entidad NumGruasSolParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic actnumgr(NumGruasSolParams val)
        {
            try
            {
                var query = Context.NumGruasSol.FirstOrDefault();

                if (query == null)
                {
                    NumGruasSol obj = new NumGruasSol();

                    obj.Conteo = val.Conteo;

                    Context.NumGruasSol.Add(obj);
                    Context.SaveChanges();
                }
                else
                {
                    NumGruasSol ob = Context.NumGruasSol.FirstOrDefault();

                    ob.Conteo = val.Conteo;
                    Context.SaveChanges();
                }

                return "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// conscinmo
        /// Description: Método para Obtener la Lista de las Causas de Cancelación.
        /// </summary>
        /// <returns List= Lista con las Causas de Cancelación.></returns>
        public dynamic conscinmo()
        {
            try
            {
                var query = Context.Causa_Cancelaciones.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// T_S_Translados
        /// Description: Método para Obtener la Lista de los Tipos de Servicio de Traslado Configurados.
        /// </summary>
        /// <returns List= Lista con los Tipos de Traslado Parametrizados.></returns>
        public dynamic T_S_Translados()
        {
            try
            {
                var query = Context.T_S_Translados.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// C_Inmovilizaciones
        /// Description: Método para Obtener la Lista de las Causas de Inmovilización.
        /// </summary>
        /// <returns List= Lista con las causas de Inmovilización.></returns>
        public dynamic C_Inmovilizaciones(C_Inmovilizaciones val)
        {
            try
            {
                var query = Context.C_Inmovilizaciones.Where(t => t.T_Translado == val.T_Translado).ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Localidades
        /// Description: Método para Obtener la Lista de las Localidades.
        /// </summary>
        /// <returns List= Lista con las Localidades.></returns>
        public dynamic Localidades()
        {
            try
            {
                var query = Context.Localidades.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// SentidoViales
        /// Description: Método para Obtener la Lista de los Sentidos Viales.
        /// </summary>
        /// <returns List= Lista con los Sentidos Viales.></returns>
        public dynamic SentidoViales()
        {
            try
            {
                var query = Context.SentidoViales.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// TipoGruas
        /// Description: Método para Obtener la Lista de los Tipos de Gruas.
        /// </summary>
        /// <returns List= Lista con los Tipos de Gruas.></returns>
        public dynamic TipoGruas()
        {
            try
            {
                var query = Context.TipoGruas.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// TVehiculoInmovilizars
        /// Description: Método para Obtener la Lista de los Tipos de Vehículos.
        /// </summary>
        /// <returns List= Lista con los Tipos de Vehículos.></returns>
        public dynamic TVehiculoInmovilizars()
        {
            try
            {
                var query = Context.TVehiculoInmovilizars.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// cacelarservicio
        /// Description: Método para Cancelar Solicitud en Grúa.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns string = Mensaje Exitoso o Errores.</returns>
        public dynamic cacelarservicio(SolicitudGruasParams val)
        {
            try
            {
                SolicitudGruas ob = Context.SolicitudGruas.Where(t => t.ID_solicitud == val.ID_solicitud).FirstOrDefault();

                if (ob.Numero_de_orden_del_servicio == null)
                {
                    return "el numero de la orden de servicio no existe, no es posible hacer la cancelación.";
                }

                ValuesWS ValoresWs = new ValuesWS();

                ValoresWs.entidad = "2";
                ValoresWs.tipoOrden = "3";
                ValoresWs.nroOrden = int.Parse(ob.Numero_de_orden_del_servicio);
                ValoresWs.causalCancelacion = val.Causa_de_Cancelacion_de_Servicio;
                ValoresWs.observaciones = val.ObsCancel;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["WsCancelarServicio"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(ValoresWs);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var Respuesta = JsonConvert.DeserializeObject<RespuestaWSCancelacion>(result);


                    List<string> MensajesError = new List<string>();

                    if (Respuesta.respuesta == null)
                    {
                        MensajesError.Add("0");
                        foreach (var item in Respuesta.mensajes)
                        {
                            MensajesError.Add(item.mensaje);
                        }

                        return MensajesError.ToList();
                    }
                    else
                    {
                        ob.Causa_de_Cancelacion_de_Servicio = val.Causa_de_Cancelacion_de_Servicio;
                        ob.Estado = "CANCELADA";
                        Context.SaveChanges();


                        Estados est = new Estados();
                        est.ID_solicitud = val.ID_solicitud;
                        est.Nombre = "CANCELADA";
                        est.Observaciones = val.ObsCancel;
                        est.Fecha = horacol;
                        Context.Estados.Add(est);
                        Context.SaveChanges();
                        return "Solicitud Cancelada Satisfactoriamente.";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// buscarsolicitudesgruser
        /// Description: Método para Buscar Solicitud de Grúa de un usuario específico.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns List = Lista con Solicitudes de Grúa asociadas al usuario.</returns>
        public dynamic buscarsolicitudesgruser(SolicitudGruasParams val)
        {
            try
            {
                var query = (from mSolicitudGruas in Context.SolicitudGruas
                             from maspnetusers in Context.AspNetUsers
                             from mTipoOrdServs in Context.TipoOrdServs
                             from mT_S_Translados in Context.T_S_Translados
                             from mC_Inmovilizaciones in Context.C_Inmovilizaciones
                             from mLocalidades in Context.Localidades
                             where
                             mSolicitudGruas.Tipo_de_servicio_de_traslado == mT_S_Translados.Id
                             && mSolicitudGruas.Causa_de_inmovilizacion == mC_Inmovilizaciones.Id
                             && mSolicitudGruas.Localidad == mLocalidades.Id
                             && mSolicitudGruas.Tipo_de_orden_de_servicio == mTipoOrdServs.Id
                             && mSolicitudGruas.Id_Usuario == maspnetusers.Id
                             && mSolicitudGruas.Id_Usuario == val.Id_Usuario

                             let est = (from mEstados in Context.Estados
                                        where mEstados.ID_solicitud == val.ID_solicitud
                                        select new
                                        {
                                            mEstados.Id,
                                            mEstados.Fecha
                                        }).FirstOrDefault()

                             let tgr = (from mtipogr in Context.TipoGruas
                                        where mSolicitudGruas.Tipo_de_Grua == mtipogr.Id
                                        select new
                                        {
                                            mtipogr.Id,
                                            mtipogr.Nombre
                                        }).FirstOrDefault()


                             select new
                             {
                                 ID_solicitud = mSolicitudGruas.ID_solicitud,
                                 Entidad = mSolicitudGruas.Entidad,
                                 Usuario = maspnetusers.firstName,
                                 Placa = maspnetusers.PlacaAgente,
                                 IdUser = mSolicitudGruas.Id_Usuario,
                                 Fecha_y_hora_solicitud_servicio = mSolicitudGruas.Fecha_y_hora_solicitud_servicio,
                                 Tipo_de_orden_de_servicio = mTipoOrdServs.Nombre,
                                 Tipo_de_servicio_de_traslado = mT_S_Translados.Nombre,
                                 Causa_de_inmovilizacion = mC_Inmovilizaciones.Nombre,
                                 Codigo_de_infraccion = mSolicitudGruas.Codigo_de_infraccion,
                                 Direccion_georreferencial = mSolicitudGruas.Direccion_georreferencial,
                                 Localidad = mLocalidades.Nombre,
                                 Direccion = mSolicitudGruas.Direccion,
                                 Barrio = mSolicitudGruas.Barrio,
                                 Tipo_de_vehiculo_a_inmovilizar = Context.SolicitudTvs.Where(t => t.IdSol == val.ID_solicitud).ToList(),
                                 Tipo_de_Grua = (tgr.Id == null) ? null : tgr.Nombre,
                                 Numero_de_gruas_solicitadas_por_tipo_de_grua = mSolicitudGruas.Numero_de_gruas_solicitadas_por_tipo_de_grua,
                                 Estado = mSolicitudGruas.Estado,
                                 Fecha_y_hora_solicitud_servicio_res = mSolicitudGruas.Fecha_y_hora_solicitud_servicio_res,
                                 Direccion_georreferencial_res = mSolicitudGruas.Direccion_georreferencial_res,
                                 Confirmacion_de_envio = mSolicitudGruas.Confirmacion_de_envio,
                                 Numero_de_orden_del_servicio = mSolicitudGruas.Numero_de_orden_del_servicio,
                                 Causal_de_rechazo = mSolicitudGruas.Causal_de_rechazo,
                                 Fecha_y_hora_de_orden_de_servicio = mSolicitudGruas.Fecha_y_hora_de_orden_de_servicio,
                                 Placa_grua_Numero = mSolicitudGruas.Placa_grua_Numero,
                                 Tipo_documento_identificacion_conductor_grua = mSolicitudGruas.Tipo_documento_identificacion_conductor_grua,
                                 Numero_de_identificacion_conductor_grua = mSolicitudGruas.Numero_de_identificacion_conductor_grua,
                                 Fecha_hora_inicio_atencion_servicio = mSolicitudGruas.Fecha_hora_inicio_atencion_servicio,
                                 Placa_vehiculo_trasladar = mSolicitudGruas.Placa_vehiculo_trasladar,
                                 Fecha_hora_Finalizacion_servicio = mSolicitudGruas.Fecha_hora_Finalizacion_servicio,
                                 Parqueadero_destino = mSolicitudGruas.Parqueadero_destino,
                                 Link_video_inmovilizacion = mSolicitudGruas.Link_video_inmovilizacion,
                                 Fecha_hora_envio_informacion = mSolicitudGruas.Fecha_hora_envio_informacion,
                                 Fecha_hora_novedad = mSolicitudGruas.Fecha_hora_novedad,
                                 Tipo_novedad = mSolicitudGruas.Tipo_novedad,
                                 Placa_grua_Nueva = mSolicitudGruas.Placa_grua_Nueva,
                                 Tipo_Doc_Conductor_Grua_Nueva = mSolicitudGruas.Tipo_Doc_Conductor_Grua_Nueva,
                                 Numero_identificacion_conductor_grua_Nueva = mSolicitudGruas.Numero_identificacion_conductor_grua_Nueva,
                                 Fecha_hora_reasignacion = mSolicitudGruas.Fecha_hora_reasignacion,
                                 Observaciones_Novedad = mSolicitudGruas.Observaciones_Novedad,
                                 Fecha_hora_llegada_lugar_solicitud = mSolicitudGruas.Fecha_hora_llegada_lugar_solicitud,
                                 Fecha_Cierre_Auto = mSolicitudGruas.Fecha_Cierre_Auto,
                                 Causa_de_Cancelacion_de_Servicio = mSolicitudGruas.Causa_de_Cancelacion_de_Servicio,
                                 //Causa_de_Cancelacion_de_Servicio = mSolicitudGruas.Causa_de_Cancelacion_de_Servicio,
                                 Estadofh = (est.Id == null) ? null : est.Fecha
                             }).ToList();
                return query.OrderByDescending(t => t.ID_solicitud).ToList();


                //var query = Context.SolicitudGruas.Where(t => t.Id_Usuario == val.Id_Usuario).ToList();

                //return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// solicitudporid
        /// Description: Método para Buscar Solicitud de Grúa por Id.
        /// </summary>
        /// <param val=json de la Entidad SolicitudGruasParams></param>
        /// <returns List = Solicitud de Grúa por Id.</returns>
        public dynamic solicitudporid(SolicitudGruasParams val)
        {
            try
            {
                //var query = Context.SolicitudGruas.Where(t => t.ID_solicitud == val.ID_solicitud).ToList();

                //return query;                

                var query = (from mSolicitudGruas in Context.SolicitudGruas
                             from maspnetusers in Context.AspNetUsers
                             from mTipoOrdServs in Context.TipoOrdServs
                             from mT_S_Translados in Context.T_S_Translados
                             from mC_Inmovilizaciones in Context.C_Inmovilizaciones
                             from mLocalidades in Context.Localidades
                                 //from mTipoGruas in Context.TipoGruas
                                 //from mTVehiculoInmovilizars in Context.TVehiculoInmovilizars
                             from mEstados in Context.Estados

                                 //from mCausa_Cancelaciones in Context.Causa_Cancelaciones
                             where mSolicitudGruas.Tipo_de_servicio_de_traslado == mT_S_Translados.Id
                             && mSolicitudGruas.Causa_de_inmovilizacion == mC_Inmovilizaciones.Id
                             && mSolicitudGruas.Localidad == mLocalidades.Id
                             //&& mSolicitudGruas.Tipo_de_Grua == mTipoGruas.Id
                             //&& mSolicitudGruas.Tipo_de_vehiculo_a_inmovilizar == mTVehiculoInmovilizars.Id
                             //&& mSolicitudGruas.Causa_de_Cancelacion_de_Servicio == mCausa_Cancelaciones.Id
                             && mSolicitudGruas.ID_solicitud == val.ID_solicitud
                             && mSolicitudGruas.Tipo_de_orden_de_servicio == mTipoOrdServs.Id
                             && mSolicitudGruas.Id_Usuario == maspnetusers.Id
                             //let mEstados = Context.Estados.FirstOrDefault(t => t.ID_solicitud == val.ID_solicitud && t.Nombre == val.Estado)
                             && mEstados.Nombre == mSolicitudGruas.Estado
                             && mEstados.ID_solicitud == mSolicitudGruas.ID_solicitud
                             select new
                             {
                                 ID_solicitud = mSolicitudGruas.ID_solicitud,
                                 Entidad = mSolicitudGruas.Entidad,
                                 Usuario = maspnetusers.firstName,
                                 Placa = maspnetusers.PlacaAgente,
                                 IdUser = mSolicitudGruas.Id_Usuario,
                                 Fecha_y_hora_solicitud_servicio = mSolicitudGruas.Fecha_y_hora_solicitud_servicio,
                                 Tipo_de_orden_de_servicio = mTipoOrdServs.Nombre,
                                 Tipo_de_servicio_de_traslado = mT_S_Translados.Nombre,
                                 Causa_de_inmovilizacion = mC_Inmovilizaciones.Nombre,
                                 Codigo_de_infraccion = mSolicitudGruas.Codigo_de_infraccion,
                                 Direccion_georreferencial = mSolicitudGruas.Direccion_georreferencial,
                                 Localidad = mLocalidades.Nombre,
                                 Direccion = mSolicitudGruas.Direccion,
                                 Barrio = mSolicitudGruas.Barrio,
                                 //Tipo_de_Grua = mTipoGruas.Nombre,
                                 Numero_de_gruas_solicitadas_por_tipo_de_grua = mSolicitudGruas.Numero_de_gruas_solicitadas_por_tipo_de_grua,
                                 Estado = mSolicitudGruas.Estado,
                                 Fecha_y_hora_solicitud_servicio_res = mSolicitudGruas.Fecha_y_hora_solicitud_servicio_res,
                                 Direccion_georreferencial_res = mSolicitudGruas.Direccion_georreferencial_res,
                                 Confirmacion_de_envio = mSolicitudGruas.Confirmacion_de_envio,
                                 Numero_de_orden_del_servicio = mSolicitudGruas.Numero_de_orden_del_servicio,
                                 Causal_de_rechazo = mSolicitudGruas.Causal_de_rechazo,
                                 Fecha_y_hora_de_orden_de_servicio = mSolicitudGruas.Fecha_y_hora_de_orden_de_servicio,
                                 Placa_grua_Numero = mSolicitudGruas.Placa_grua_Numero,
                                 Tipo_documento_identificacion_conductor_grua = mSolicitudGruas.Tipo_documento_identificacion_conductor_grua,
                                 Numero_de_identificacion_conductor_grua = mSolicitudGruas.Numero_de_identificacion_conductor_grua,
                                 Fecha_hora_inicio_atencion_servicio = mSolicitudGruas.Fecha_hora_inicio_atencion_servicio,
                                 Placa_vehiculo_trasladar = mSolicitudGruas.Placa_vehiculo_trasladar,
                                 Fecha_hora_Finalizacion_servicio = mSolicitudGruas.Fecha_hora_Finalizacion_servicio,
                                 Parqueadero_destino = mSolicitudGruas.Parqueadero_destino,
                                 Link_video_inmovilizacion = mSolicitudGruas.Link_video_inmovilizacion,
                                 Fecha_hora_envio_informacion = mSolicitudGruas.Fecha_hora_envio_informacion,
                                 Fecha_hora_novedad = mSolicitudGruas.Fecha_hora_novedad,
                                 Tipo_novedad = mSolicitudGruas.Tipo_novedad,
                                 Placa_grua_Nueva = mSolicitudGruas.Placa_grua_Nueva,
                                 Tipo_Doc_Conductor_Grua_Nueva = mSolicitudGruas.Tipo_Doc_Conductor_Grua_Nueva,
                                 Numero_identificacion_conductor_grua_Nueva = mSolicitudGruas.Numero_identificacion_conductor_grua_Nueva,
                                 Fecha_hora_reasignacion = mSolicitudGruas.Fecha_hora_reasignacion,
                                 Observaciones_Novedad = mSolicitudGruas.Observaciones_Novedad,
                                 Fecha_hora_llegada_lugar_solicitud = mSolicitudGruas.Fecha_hora_llegada_lugar_solicitud,
                                 Fecha_Cierre_Auto = mSolicitudGruas.Fecha_Cierre_Auto,
                                 Causa_de_Cancelacion_de_Servicio = mSolicitudGruas.Causa_de_Cancelacion_de_Servicio,
                                 //Causa_de_Cancelacion_de_Servicio = mSolicitudGruas.Causa_de_Cancelacion_de_Servicio,
                                 Estadofh = (mEstados.Id == null) ? null : mEstados.Fecha,
                                 EstadoTbl = (mEstados.Id == null) ? null : mEstados.Nombre
                             }).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// constvxid
        /// Description: Método para Consultar Tipos de Vehículos por Id de Solicitud.
        /// </summary>
        /// <param val=json de la Entidad SolicitudTvs></param>
        /// <returns List= Lista de Tipos de Vehículos por Id de Solicitud.></returns>
        public dynamic constvxid(SolicitudTvs Id)
        {

            return (from mSolicitudTvs in Context.SolicitudTvs
                    from mTVehiculoInmovilizars in Context.TVehiculoInmovilizars
                    where mSolicitudTvs.TipoV == mTVehiculoInmovilizars.Id
                    && mSolicitudTvs.IdSol == Id.Id
                    select new
                    {
                        Nombre = mTVehiculoInmovilizars.Nombre,
                        Cantidad = mSolicitudTvs.Cantidad
                    }).ToList();
        }


        /// <summary>
        /// searchgrubyid
        /// Description: Método para Consultar Grúas por ID.
        /// </summary>
        /// <param val=json de la Entidad SolicitudTvs></param>
        /// <returns List= Lista de Grúas por ID.></returns>
        public dynamic searchgrubyid(CanGruasSolicitudes Id)
        {

            return (from mCanGruasSolicitudes in Context.CanGruasSolicitudes
                    from mTipoGruas in Context.TipoGruas
                    where mCanGruasSolicitudes.TipoGrua == mTipoGruas.Id
                    && mCanGruasSolicitudes.IdSol == Id.Id
                    && mCanGruasSolicitudes.Estado == 1
                    select new
                    {
                        Nombre = mTipoGruas.Nombre,
                        Placa = mCanGruasSolicitudes.Placa,
                        TipoIdenConductor = mCanGruasSolicitudes.TipoIdenConductor,
                        NroIdenConductor = mCanGruasSolicitudes.NroIdenConductor,
                        Estado = mCanGruasSolicitudes.Estado
                    }).OrderByDescending(t => t.Estado).ToList();
        }

        /// <summary>
        /// consultarnumest
        /// Description: Método para Consultar la Parametrización de minutos de atención ANS.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos></returns>
        public dynamic consultarnumest()
        {
            try
            {
                var query = Context.ConfigCierreAutos.Where(t => t.Id == "1").FirstOrDefault();


                if (query == null)
                {
                    ConfigCierreAutos est = new ConfigCierreAutos();
                    est.Id = "1";
                    est.Horas = "0";
                    Context.ConfigCierreAutos.Add(est);
                    Context.SaveChanges();
                }


                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// consultarnumest
        /// Description: Método para Consultar la Parametrización de minutos de atención ANS.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos Parámetro 2></returns>
        public dynamic anscons()
        {
            try
            {
                var query = Context.ConfigCierreAutos.Where(t => t.Id == "2").FirstOrDefault();

                if (query == null)
                {
                    ConfigCierreAutos est = new ConfigCierreAutos();
                    est.Id = "2";
                    est.Horas = "0";
                    Context.ConfigCierreAutos.Add(est);
                    Context.SaveChanges();
                }

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// horainfcons
        /// Description: Método para Consultar la Parametrización de hora de envío de Correo Solicitudes expiración ANS.
        /// </summary>
        /// <returns ConfigCierreAutos= Objeto con la Parametrización de ConfigCierreAutos Parámetro 3></returns>
        public dynamic horainfcons()
        {
            try
            {
                var query = Context.ConfigCierreAutos.Where(t => t.Id == "3").FirstOrDefault();

                if (query == null)
                {
                    ConfigCierreAutos est = new ConfigCierreAutos();
                    est.Id = "3";
                    est.Horas = "0";
                    Context.ConfigCierreAutos.Add(est);
                    Context.SaveChanges();
                }

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// actualizardiasexp
        /// Description: Método para Actualizar minutos de Para cierre automático de solicitudes de grúas.
        /// </summary>
        /// <returns string= Mensajes Exitosos y Erroes.></returns>
        public dynamic actualizardiasexp(ConfigCierreAutosParams val)
        {
            try
            {
                ConfigCierreAutos ob = Context.ConfigCierreAutos.Where(t => t.Id == val.Id).FirstOrDefault();

                ob.Horas = val.Horas;
                Context.SaveChanges();

                return "Actualización Satisfactoria";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// actans
        /// Description: Método para Actualizar minutos de atención ANS.
        /// </summary>
        /// <returns string= Mensajes Exitosos y Erroes.></returns>
        public dynamic actans(ConfigCierreAutosParams val)
        {
            try
            {
                ConfigCierreAutos ob = Context.ConfigCierreAutos.Where(t => t.Id == val.Id).FirstOrDefault();

                ob.Horas = val.Horas;
                Context.SaveChanges();

                return "Actualización Satisfactoria";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// ActualizarHora
        /// Description: Método para Actualizar hora de envío de Correo Solicitudes expiración ANS.
        /// </summary>
        /// <returns string= Mensajes Exitosos y Erroes.></returns>
        public dynamic ActualizarHora(ConfigCierreAutosParams val)
        {
            try
            {

                DateTime MyDateTime = DateTime.Parse(val.Horas, new CultureInfo("en-US"));

                ConfigCierreAutos ob = Context.ConfigCierreAutos.Where(t => t.Id == val.Id).FirstOrDefault();

                ob.Horas = MyDateTime.TimeOfDay.ToString();
                Context.SaveChanges();

                return "Actualización Satisfactoria ---" + val.Horas;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// searchreportroluser
        /// Description: Método para Consultar los Reportes de Total de Usuarios por Roles.
        /// </summary>
        /// <returns List= Lista con los Usuarios por Roles.></returns>
        public dynamic searchreportroluser()
        {
            try
            {
                var a = (from museroles in Context.AspNetUserRoles
                         from muser in Context.AspNetUsers
                         from mroles in Context.AspNetRoles
                         where museroles.UserId == muser.Id
                         && museroles.RoleId == mroles.Id
                         select new
                         {
                             IdRol = mroles.Id,
                             Rol = mroles.Name,
                             User = muser.firstName,
                             Doc = muser.NumeroDocumento,
                             Estado = mroles.Activo
                         }).ToList();

                var b = Context.AspNetRoles.ToList();

                List<ReporteUserRol> listar = new List<ReporteUserRol>();
                var count = 0;

                foreach (var item in b)
                {
                    ReporteUserRol lista = new ReporteUserRol();

                    foreach (var itm in a)
                    {
                        if (item.Id == itm.IdRol)
                        {
                            count = count + 1;
                        }
                    }

                    lista.Conteo = count;
                    lista.Activo = item.Activo;
                    lista.Name = item.Name;

                    listar.Add(lista);
                    count = 0;
                }

                return listar;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// ReportUserRoles
        /// Description: Método para Consultar los Lista de Usuarios con Roles.
        /// </summary>
        /// <returns List= Lista con los Usuarios por Roles.></returns>
        public dynamic ReportUserRoles()
        {
            try
            {
                var a = (from museroles in Context.AspNetUserRoles
                         from muser in Context.AspNetUsers
                         from mroles in Context.AspNetRoles
                         where museroles.UserId == muser.Id
                         && museroles.RoleId == mroles.Id
                         select new
                         {
                             Rol = mroles.Name,
                             User = muser.firstName,
                             Doc = muser.NumeroDocumento,
                             Estado = muser.LockoutEnabled
                         }).ToList();
                return a;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// listmails
        /// Description: Método para Consultar la Lista de Mails parametrizados para enviar de las ANS.
        /// </summary>
        /// <returns List= Lista con los Emails parametrizados.></returns>
        public dynamic listmails()
        {
            try
            {
                var query = Context.ListaCorreosANS.ToList();

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// addmail
        /// Description: Método para Agregar un email a la lista de Emails parametrizados de ANS.
        /// </summary>
        /// <param val=json de la Entidad ListaCorreosANSParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        public dynamic addmail(ListaCorreosANSParams mails)
        {
            try
            {
                ListaCorreosANS obj = new ListaCorreosANS();

                obj.Correo = mails.Correo;

                Context.ListaCorreosANS.Add(obj);
                Context.SaveChanges();

                return "Correo Agregado Correctamente.";
            }

            catch (Exception ex)
            {
                return "ERROR -- " + ex;
                throw;
            }
        }

        /// <summary>
        /// elimemail
        /// Description: Método para Eliminar un email a la lista de Emails parametrizados de ANS.
        /// </summary>
        /// <param val=json de la Entidad ListaCorreosANSParams></param>
        /// <returns string= Mensajes Exitosos y Errores.></returns>
        public dynamic deletemail(ListaCorreosANSParams mails)
        {
            try
            {
                ListaCorreosANS obj = new ListaCorreosANS();

                //SetParameters
                obj.Id = mails.Id;

                Context.ListaCorreosANS.Attach(obj);
                Context.ListaCorreosANS.Remove(obj);
                Context.SaveChanges();

                return "Correo Eliminado Correctamente.";
            }

            catch (Exception ex)
            {
                return "ERROR -- " + ex;
                throw;
            }
        }

        /// <summary>
        /// reportsolagent
        /// Description: Método para Consultar Solicitudes por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación por Agente.
        /// </summary>
        /// <param val=json de la Entidad reportsolagent></param>
        /// <returns List= Reporte por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación por Agente.></returns>
        public dynamic reportsolagent(reportsolagent val)
        {
            try
            {
                ReporteTipoXUser reporte = new ReporteTipoXUser();

                var userp = Context.AspNetUsers.Where(t => t.PlacaAgente == val.plagente).FirstOrDefault();

                if (userp == null)
                {
                    return "La Placa No se Encuentra Asociada a Ningún Agente.";
                }
                else
                {

                    reporte.UserName = userp.firstName;


                    if (val.CInmov == "1")
                    {
                        var cin = Context.C_Inmovilizaciones.ToList();

                        List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                        if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id && t.Estado == val.estadosol).ToList();
                        }
                        else
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id).ToList();
                        }

                        List<CInmov> listar = new List<CInmov>();


                        foreach (var item in cin)
                        {
                            var count = 0;
                            CInmov lista = new CInmov();
                            foreach (var itm in tsoli)
                            {
                                if (item.Id == itm.Causa_de_inmovilizacion)
                                {
                                    count++;
                                }
                            }

                            lista.Name = item.Nombre;
                            lista.Conteo = count;
                            lista.Tipo = "Causa de la Inmovilización";

                            listar.Add(lista);
                        }

                        reporte.CausaIn = listar;
                    }


                    if (val.TGrua == "1")
                    {
                        var cin = Context.TipoGruas.ToList();

                        List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                        if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id && t.Estado == val.estadosol).ToList();
                        }
                        else
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id).ToList();
                        }

                        List<TGrua> listar = new List<TGrua>();
                        List<TGrua> listar2 = new List<TGrua>();



                        foreach (var itm in tsoli)
                        {
                            var consu = (from mCanGruasSolicitudes in Context.CanGruasSolicitudes
                                         from mTipoGruas in Context.TipoGruas
                                         where mCanGruasSolicitudes.TipoGrua == mTipoGruas.Id
                                         && mCanGruasSolicitudes.IdSol == itm.ID_solicitud
                                         select new { Name = mTipoGruas.Nombre }).ToList();

                            if (consu.Count != 0)
                            {
                                foreach (var item in consu)
                                {
                                    TGrua lista = new TGrua();

                                    lista.Name = item.Name;
                                    listar.Add(lista);
                                }
                            }
                        }

                        var grupoGrua = listar.GroupBy(t => t.Name).ToList();

                        for (int i = 0; i < grupoGrua.Count; i++)
                        {
                            TGrua lista2 = new TGrua();

                            lista2.Name = grupoGrua[i].Key;
                            lista2.Conteo = grupoGrua[i].Count();
                            lista2.Tipo = "Tipo de Grúa";

                            listar2.Add(lista2);
                        }

                        reporte.TipoGru = listar2;
                    }


                    if (val.TVehiculo == "1")
                    {
                        var cin = Context.TVehiculoInmovilizars.ToList();

                        List<SolicitudGruas> tsoli = new List<SolicitudGruas>();


                        if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id && t.Estado == val.estadosol).ToList();
                        }
                        else
                        {

                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id).ToList();
                        }

                        List<TVehiculo> listar = new List<TVehiculo>();
                        List<TVehiculo> listar2 = new List<TVehiculo>();

                        foreach (var itm in tsoli)
                        {
                            var consu = (from mSolicitudTvs in Context.SolicitudTvs
                                         from mTVehiculoInmovilizars in Context.TVehiculoInmovilizars
                                         where mSolicitudTvs.TipoV == mTVehiculoInmovilizars.Id
                                         && mSolicitudTvs.IdSol == itm.ID_solicitud
                                         select new { Name = mTVehiculoInmovilizars.Nombre }).ToList();

                            if (consu.Count != 0)
                            {
                                foreach (var item in consu)
                                {
                                    TVehiculo lista = new TVehiculo();

                                    lista.Name = item.Name;
                                    listar.Add(lista);
                                }
                            }

                        }

                        var grupovehiculo = listar.GroupBy(t => t.Name).ToList();

                        for (int i = 0; i < grupovehiculo.Count; i++)
                        {
                            TVehiculo lista2 = new TVehiculo();

                            lista2.Name = grupovehiculo[i].Key;
                            lista2.Conteo = grupovehiculo[i].Count();
                            lista2.Tipo = "Tipo de Vehículo a Inmovilizar";

                            listar2.Add(lista2);
                        }

                        reporte.TipoVehi = listar2;
                    }


                    if (val.MCacncelacion == "1")
                    {
                        var cin = Context.Causa_Cancelaciones.ToList();

                        List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                        if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                        {
                            tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Id_Usuario == userp.Id).ToList();
                        }
                        else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id && t.Estado == val.estadosol).ToList();
                        }
                        else
                        {
                            tsoli = Context.SolicitudGruas.Where(t => t.Id_Usuario == userp.Id).ToList();
                        }

                        List<MCacncelacion> listar = new List<MCacncelacion>();


                        foreach (var item in cin)
                        {
                            var count = 0;
                            MCacncelacion lista = new MCacncelacion();
                            foreach (var itm in tsoli)
                            {
                                if (item.Id == itm.Causa_de_Cancelacion_de_Servicio)
                                {
                                    count++;
                                }
                            }

                            lista.Name = item.Nombre;
                            lista.Conteo = count;
                            lista.Tipo = "Causa Cancelación";

                            listar.Add(lista);
                        }

                        reporte.CausaCan = listar;
                    }

                    return reporte;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// reportsol
        /// Description: Método para Consultar Solicitudes por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación de todos los Agentes.
        /// </summary>
        /// <param val=json de la Entidad reportsolagent></param>
        /// <returns List= Reporte por Tipo de Grúa, Vehículo, Causa Inmovilización y Motivo de Cancelación de todos los Agentes.></returns>
        public dynamic reportsol(reportsolagent val)
        {
            try
            {
                    ReporteTipoXUser reporte = new ReporteTipoXUser();

                if (val.CInmov == "1")
                {
                    var cin = Context.C_Inmovilizaciones.ToList();

                    List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                    if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol).ToList();
                    }
                    else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();
                    }
                    else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                    {
                        tsoli = Context.SolicitudGruas.Where(t => t.Estado == val.estadosol).ToList();
                    }
                    else
                    {
                        tsoli = Context.SolicitudGruas.ToList();
                    }

                    List<CInmov> listar = new List<CInmov>();


                    foreach (var item in cin)
                    {
                        var count = 0;
                        CInmov lista = new CInmov();
                        foreach (var itm in tsoli)
                        {
                            if (item.Id == itm.Causa_de_inmovilizacion)
                            {
                                count++;
                            }
                        }

                        lista.Name = item.Nombre;
                        lista.Conteo = count;
                        lista.Tipo = "Causa de la Inmovilización";

                        listar.Add(lista);
                    }

                    reporte.CausaIn = listar;
                }


                if (val.TGrua == "1")
                {
                    var cin = Context.TipoGruas.ToList();

                    List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                    if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol).ToList();
                    }
                    else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();
                    }
                    else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                    {
                        tsoli = Context.SolicitudGruas.Where(t => t.Estado == val.estadosol).ToList();
                    }
                    else
                    {
                        tsoli = Context.SolicitudGruas.ToList();
                    }

                    List<TGrua> listar = new List<TGrua>();
                    List<TGrua> listar2 = new List<TGrua>();



                    foreach (var itm in tsoli)
                    {
                        var consu = (from mCanGruasSolicitudes in Context.CanGruasSolicitudes
                                     from mTipoGruas in Context.TipoGruas
                                     where mCanGruasSolicitudes.TipoGrua == mTipoGruas.Id
                                     && mCanGruasSolicitudes.IdSol == itm.ID_solicitud
                                     select new { Name = mTipoGruas.Nombre }).ToList();

                        if (consu.Count != 0)
                        {
                            foreach (var item in consu)
                            {
                                TGrua lista = new TGrua();

                                lista.Name = item.Name;
                                listar.Add(lista);
                            }
                        }
                    }

                    var grupoGrua = listar.GroupBy(t => t.Name).ToList();

                    for (int i = 0; i < grupoGrua.Count; i++)
                    {
                        TGrua lista2 = new TGrua();

                        lista2.Name = grupoGrua[i].Key;
                        lista2.Conteo = grupoGrua[i].Count();
                        lista2.Tipo = "Tipo de Grúa";

                        listar2.Add(lista2);
                    }

                    reporte.TipoGru = listar2;
                }


                if (val.TVehiculo == "1")
                {
                    var cin = Context.TVehiculoInmovilizars.ToList();

                    List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                    if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol).ToList();
                    }
                    else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();
                    }
                    else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                    {
                        tsoli = Context.SolicitudGruas.Where(t => t.Estado == val.estadosol).ToList();
                    }
                    else
                    {
                        tsoli = Context.SolicitudGruas.ToList();
                    }

                    List<TVehiculo> listar = new List<TVehiculo>();
                    List<TVehiculo> listar2 = new List<TVehiculo>();

                    foreach (var itm in tsoli)
                    {
                        var consu = (from mSolicitudTvs in Context.SolicitudTvs
                                     from mTVehiculoInmovilizars in Context.TVehiculoInmovilizars
                                     where mSolicitudTvs.TipoV == mTVehiculoInmovilizars.Id
                                     && mSolicitudTvs.IdSol == itm.ID_solicitud
                                     select new { Name = mTVehiculoInmovilizars.Nombre }).ToList();

                        if (consu.Count != 0)
                        {
                            foreach (var item in consu)
                            {
                                TVehiculo lista = new TVehiculo();

                                lista.Name = item.Name;
                                listar.Add(lista);
                            }
                        }

                    }

                    var grupovehiculo = listar.GroupBy(t => t.Name).ToList();

                    for (int i = 0; i < grupovehiculo.Count; i++)
                    {
                        TVehiculo lista2 = new TVehiculo();

                        lista2.Name = grupovehiculo[i].Key;
                        lista2.Conteo = grupovehiculo[i].Count();
                        lista2.Tipo = "Tipo de Vehículo a Inmovilizar";

                        listar2.Add(lista2);
                    }

                    reporte.TipoVehi = listar2;
                }


                if (val.MCacncelacion == "1")
                {
                    var cin = Context.Causa_Cancelaciones.ToList();

                    List<SolicitudGruas> tsoli = new List<SolicitudGruas>();

                    if (val.finicio != null && val.estadosol != "" && val.plagente != "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.estadosol).ToList();
                    }
                    else if (val.finicio != null && val.plagente != "" && val.estadosol == "")
                    {
                        tsoli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.finicio && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();
                    }
                    else if (val.plagente != "" && val.estadosol != "" && val.finicio == null)
                    {
                        tsoli = Context.SolicitudGruas.Where(t => t.Estado == val.estadosol).ToList();
                    }
                    else
                    {
                        tsoli = Context.SolicitudGruas.ToList();
                    }

                    List<MCacncelacion> listar = new List<MCacncelacion>();


                    foreach (var item in cin)
                    {
                        var count = 0;
                        MCacncelacion lista = new MCacncelacion();
                        foreach (var itm in tsoli)
                        {
                            if (item.Id == itm.Causa_de_Cancelacion_de_Servicio)
                            {
                                count++;
                            }
                        }

                        lista.Name = item.Nombre;
                        lista.Conteo = count;
                        lista.Tipo = "Causa Cancelación";

                        listar.Add(lista);
                    }

                    reporte.CausaCan = listar;
                }

                return reporte;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// numsolaagente
        /// Description: Método para Consultar cantidad de Solicitudes por Usuario.
        /// </summary>
        /// <param val=json de la Entidad ReportSolServices></param>
        /// <returns List= Reporte de Cantidad de Solicitudes por Agente.></returns>
        public dynamic numsolaagente(ReportSolServices val)
        {
            try
            {
                var users = Context.AspNetUsers.ToList();
                List<SolicitudGruas> soli = new List<SolicitudGruas>();

                if (val.fini != null && val.Estado != null)
                {
                    soli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.fini && t.Fecha_y_hora_solicitud_servicio < val.ffin) && t.Estado == val.Estado).ToList();
                }
                else if (val.fini != null && val.Estado == null)
                {
                    soli = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.fini && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();
                }
                else if (val.fini == null && val.Estado != null)
                {
                    soli = Context.SolicitudGruas.Where(t => t.Estado == val.Estado).ToList();
                }
                else
                {
                    soli = Context.SolicitudGruas.ToList();
                }


                List<ReportSolServices> rep = new List<ReportSolServices>();

                foreach (var item in users)
                {
                    var count = 0;
                    ReportSolServices replist = new ReportSolServices();

                    foreach (var itm in soli)
                    {
                        if (itm.Id_Usuario == item.Id)
                        {
                            count++;
                        }
                        replist.fini = itm.Fecha_y_hora_solicitud_servicio;
                    }

                    replist.Name = item.firstName;
                    replist.Conteo = count;

                    rep.Add(replist);
                }

                return rep;
            }
            catch (Exception ex)
            {
                return ex;
                throw ex;
            }
        }

        /// <summary>
        /// timesol
        /// Description: Método para Consultar tiempo de atención de Solicitudes.
        /// </summary>
        /// <param val=json de la Entidad TimeSoli></param>
        /// <returns List= Reporte Tiempo de Atención de Solicitudes.></returns>
        public dynamic timesol(TimeSoli val)
        {
            try
            {
                var query = Context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > val.fini && t.Fecha_y_hora_solicitud_servicio < val.ffin)).ToList();

                List<TimeSoli> rep = new List<TimeSoli>();

                foreach (var item in query)
                {
                    TimeSoli replist = new TimeSoli();

                    replist.solicitud = item.ID_solicitud;
                    replist.fini = item.Fecha_y_hora_solicitud_servicio.ToString() == "1/01/0001 12:00:00 a.m." ? (DateTime?)null : item.Fecha_y_hora_solicitud_servicio;
                    replist.ffin = item.Fecha_y_hora_solicitud_servicio_res.ToString() == "1/01/0001 12:00:00 a.m." ? (DateTime?)null : item.Fecha_y_hora_solicitud_servicio_res;

                    if (item.Fecha_y_hora_solicitud_servicio_res.ToString() == "1/01/0001 12:00:00 a.m.")
                    {
                        replist.dias = item.Fecha_y_hora_solicitud_servicio_res.Subtract(item.Fecha_y_hora_solicitud_servicio_res);
                    }
                    else
                    {
                        replist.dias = item.Fecha_y_hora_solicitud_servicio_res.Subtract(item.Fecha_y_hora_solicitud_servicio);
                    }


                    replist.fat = item.Fecha_hora_inicio_atencion_servicio.ToString() == "1/01/0001 12:00:00 a.m." ? (DateTime?)null : item.Fecha_hora_inicio_atencion_servicio;
                    //replist.dias2 = (item.Fecha_y_hora_solicitud_servicio - (item.Fecha_hora_inicio_atencion_servicio.ToString() == "1/01/0001 12:00:00 a.m." ? item.Fecha_y_hora_solicitud_servicio : item.Fecha_hora_inicio_atencion_servicio));


                    if (item.Fecha_hora_inicio_atencion_servicio.ToString() == "1/01/0001 12:00:00 a.m.")
                    {
                        replist.dias2 = item.Fecha_hora_inicio_atencion_servicio.Subtract(item.Fecha_hora_inicio_atencion_servicio);
                    }
                    else
                    {
                        replist.dias2 = item.Fecha_hora_inicio_atencion_servicio.Subtract(item.Fecha_y_hora_solicitud_servicio);
                    }


                    replist.Estado = item.Estado;

                    rep.Add(replist);
                }

                return rep;
            }
            catch (Exception ex)
            {
                return ex;
                throw ex;
            }
        }
               

        /// <summary>
        /// Reporta Ubicación de las Gruas, simulación ws entregado por el concecionario
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public List<UbicacionGruas> SearchGrua(UbicacionGruasParams val)
        {
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["WsUbicacionGruas"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(val);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string  result = streamReader.ReadToEnd();
                    return GetListaUbicacionGruas(result);
                } 

            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

        public List<UbicacionGruas> GetListaUbicacionGruas(string data) {
            List<UbicacionGruas> listaUbicacion = new List<UbicacionGruas>();
            var Respuesta = JsonConvert.DeserializeObject<RespuestaUbicacion2>(data);
            if (Respuesta.respuesta != null)
            {
                foreach (respuestas2 res2 in Respuesta.respuesta)
                {
                    UbicacionGruas ub = new UbicacionGruas();

                    string ubicacion = res2.ubicacion;
                    string[] temp = ubicacion.Split(',');
                    if (temp.Length == 3)
                    {
                        double x = Double.Parse(temp[0].Split(':')[1]);
                        double y = Double.Parse(temp[1].Split(':')[1]);
                        double z = Double.Parse(temp[2].Split(':')[1]);

                        double[] puntos = getLocationCoordenadasPlanas(x, y, z);

                        string strUbicacionFinal = puntos[0].ToString().Replace(',', '.') + "," + puntos[1].ToString().Replace(',', '.') ;

                        ub.Placagrua = res2.placagrua;
                        ub.Ubicacion = strUbicacionFinal;
                        ub.Tiempo = res2.tiempo == null ? 0 : int.Parse(res2.tiempo);
                    }

                    listaUbicacion.Add(ub);
                }

            }
            return listaUbicacion;
        }

        public double[] getLocationCoordenadasPlanas(double x, double y, double z) {
            double a = 6378137; 
            double e = 8.1819190842622e-2;

            double asq = Math.Pow(a, 2);
            double esq = Math.Pow(e, 2);

            double b = Math.Sqrt(asq * (1 - esq));
            double bsq = Math.Pow(b, 2);
            double ep = Math.Sqrt((asq - bsq) / bsq);
            double p = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double th = Math.Atan2(a * z, b * p);

            double lon = Math.Atan2(y, x);
            double lat = Math.Atan2((z + Math.Pow(ep, 2) * b * Math.Pow(Math.Sin(th), 3)), (p - esq * a * Math.Pow(Math.Cos(th), 3)));
            double N = a / (Math.Sqrt(1 - esq * Math.Pow(Math.Sin(lat), 2)));
            double alt = p / Math.Cos(lat) - N;

            // mod lat to 0-2pi
            lon = lon % (2 * Math.PI);

            // correction for altitude near poles left out.
            lat = RadianToDegree(lat);
            lon = RadianToDegree(lon);
            double[] ret = { lat, lon, alt };

            return ret;
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        #region WS Secretaria
        /// <summary>
        /// Respuesta de si se Asgina o Cancela una Solicitud de Grúa.
        /// WSRespuestaSol
        /// </summary>
        /// <param JSON=Data de Solicitud de Grúas></param>
        /// <returns></returns>
        public RespuestaWS registrarSolicitud(SolicitudServicioDTO JSON, string Authorization)
        {

            try
            {
                //var RespuestaDTO = new RespuestaSolicitudServicioSDMDTO();

                var respuesta = new RespuestaWS();
                var mensajes = new mensajes();
                respuesta.respuesta = new respuesta();
                TipoOrdServs tipoordensearch = new TipoOrdServs();

                var AutValue = Context.AutTokens.FirstOrDefault(t => t.Token == Authorization);

                if (AutValue == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Token invalido.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    if (AutValue.TimeExpire <= horacol)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El token expiro.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                }

                var select = Context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudServicioDTO desjson = JsonConvert.DeserializeObject<SolicitudServicioDTO>(objectdes.ToString());


                if (desjson.fechaTraslado != null)
                {

                }


                if (desjson.tipoOrden != null)
                {
                    tipoordensearch = Context.TipoOrdServs.FirstOrDefault(t => t.Id == desjson.tipoOrden);
                }

                var tipotrsearch = Context.TipoOrdServs.FirstOrDefault(t => t.Id == desjson.tipoTraslado.ToString());
                var tipocausainsearch = Context.C_Inmovilizaciones.FirstOrDefault(t => t.Id == desjson.causaInmovilizacion.ToString());
                var tipoinfsearch = Context.CodigosInfracciones.FirstOrDefault(t => t.Id == desjson.tipoInfraccion.ToString());

                if (desjson.direccionGeo != null && desjson.direccionGeo != "")
                {
                    var val1 = desjson.direccionGeo.ToString().Contains("X:");
                    var val2 = desjson.direccionGeo.ToString().Contains(",Y:");
                    var val3 = desjson.direccionGeo.ToString().Contains(",Z:");



                    if (val1 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    if (val2 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    if (val3 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                }

                //var tipozonasearch = Context.CodigosInfracciones.FirstOrDefault(t => t.Id == desjson.causaInmovilizacion.ToString());

                if (desjson.entidad != "1" && desjson.entidad != "2")
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo entidad ingresado no se encuentra en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }

                if (desjson.tipoZona != 1 && desjson.tipoZona != 2)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo zona ingresado no se encuentra en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }

                if (tipoinfsearch == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo infracción ingresado no se encuentra en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }


                if (desjson.nroOrden == 0)
                {
                    //return "Mensaje de error, campo No de Orden de servicio es obligatorio.";
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo No de Orden de servicio es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.entidad == "" || desjson.entidad == null)
                {
                    //return "Mensaje de error, campo entidad es obligatorio.";
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo entidad es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.fechaSolicitud == null || desjson.fechaSolicitud == "")
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo fecha de solicitud es obligatorio o no tiene formato correcto.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.fechaTraslado != null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo fechaTraslado debe ser nula.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.horaTraslado != null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo horaTraslado debe ser nula.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoOrden == "" || desjson.tipoOrden == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo de orden es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (tipoordensearch == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo de orden no corresponde en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoTraslado == 0)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo de traslado es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (tipotrsearch == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo de traslado no corresponde en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;

                }
                else if (desjson.causaInmovilizacion == 0)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo causa de la inmovilización es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (tipocausainsearch == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo causa de inmovilización no corresponde en la tabla parametrica.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.direccionGeo == "" || desjson.direccionGeo == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo dirección geo es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoZona == 0)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo tipo de zona es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoZona == 2 && (desjson.direccionUrbana == "" || desjson.direccionUrbana == null))
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo dirección urbana es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoZona == 1 && (desjson.direccionRural == "" || desjson.direccionRural == null))
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo dirección rural es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoOrden == "3" && (desjson.causalCancelacion == "" || desjson.causalCancelacion == null))
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo causa de cancelación es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.dependenciaOperativo != 0)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "El campo dependencia del operativo debe estar nulo.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.nombreSolicitanteOperativo != null && desjson.nombreSolicitanteOperativo != "")
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "El campo dependencia del operativo debe estar nulo.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.causalModificacion != null && desjson.causalModificacion != "")
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "El campo causal de modificación estar nulo.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.confirmacion == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Campo confirmación no pude estar nulo.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    SolicitudGruas ob = Context.SolicitudGruas.Where(t => t.Numero_de_orden_del_servicio == desjson.nroOrden.ToString()).FirstOrDefault();

                    if (ob == null)
                    {
                        //return "El numero de la orden no existe, favor validar.";
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El número de la orden no existe.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }

                    ob.Fecha_y_hora_solicitud_servicio_res = horacol;
                    ob.Parqueadero_destino = desjson.parqueaderoDestino;
                    if (desjson.confirmacion == "6")
                    {
                        ob.Estado = "APROBADA";
                    }
                    else if (desjson.confirmacion == "7")
                    {
                        ob.Estado = "RECHAZADA";
                    }


                    var consulta = Context.CanGruasSolicitudes.Where(t => t.IdSol == ob.ID_solicitud).ToList();

                    var ListGruas = new List<CanGruasSolicitudes>();
                    var ListVehiculos = new List<SolicitudTVSHistories>();


                    foreach (var itms in consulta)
                    {
                        foreach (var item in desjson.gruasAsignadas)
                        {
                            if (itms.Placa == item.placa)
                            {
                                mensajes.codigo = "Error";
                                mensajes.mensaje = "El la placa ingresada ya se encuentra registada, no es posible guardar.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                        }

                    }



                    if (desjson.gruasAsignadas == null && desjson.confirmacion == "6")
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El objeto grúas asignadas es obligatorio.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    else if (desjson.gruasAsignadas != null)
                    {
                        foreach (var item in desjson.gruasAsignadas)
                        {
                            var objtvs = new CanGruasSolicitudes();
                            if (item != null)
                            {
                                var tgua = Context.TipoGruas.FirstOrDefault(t => t.Id == item.tipoGrua.ToString());

                                if (item.tipoGrua == 0)
                                {
                                    //return "Mensaje de error, campo Tipo_de_Grua es obligatorio.";
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "Campo tipo de grúa es obligatorio.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }
                                else if (item.placa == "" || item.placa == null)
                                {
                                    //return "Mensaje de error, campo placa grúa es obligatorio.";
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "Campo placa de grúa es obligatorio.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }
                                if (tgua == null)
                                {
                                    //return "Mensaje de error, tipo grua no existe.";
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "El tipo grúa ingresado no existe.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }
                                objtvs.IdSol = ob.ID_solicitud;
                                objtvs.TipoGrua = item.tipoGrua.ToString();
                                objtvs.Placa = item.placa;
                                objtvs.NroIdenConductor = item.nroIdenConductor;
                                objtvs.TipoIdenConductor = item.tipoIdenConductor;
                                objtvs.Estado = 1;

                                ListGruas.Add(objtvs);
                            }
                        }
                    }

                    if (desjson.tipoVehiculo == null && desjson.tipoTraslado == 1)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El objeto tipo vehículo es obligatorio.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    else if (desjson.tipoVehiculo != null)
                    {
                        foreach (var item in desjson.tipoVehiculo)
                        {

                            var tvhis = Context.TVehiculoInmovilizars.FirstOrDefault(t => t.Id == item.tipoVehiculo);
                            if (tvhis == null)
                            {
                                mensajes.codigo = "Error";
                                mensajes.mensaje = "El tipo de vehículo ingresado no existe.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }


                            SolicitudTVSHistories SolVHis = new SolicitudTVSHistories();
                            SolVHis.IdSol = ob.ID_solicitud;
                            SolVHis.TipoV = item.tipoVehiculo;
                            SolVHis.Cantidad = item.cantidad;
                            SolVHis.DateCreate = horacol;

                            ListVehiculos.Add(SolVHis);
                        }

                    }

                    if (desjson.confirmacion == "6" || desjson.confirmacion == "7")
                    {
                        Estados estado = new Estados();
                        estado.ID_solicitud = ob.ID_solicitud;
                        if (desjson.confirmacion == "6")
                        {
                            estado.Nombre = "APROBADA";
                        }
                        else if (desjson.confirmacion == "7")
                        {
                            estado.Nombre = "RECHAZADA";
                        }
                        estado.Observaciones = null;
                        estado.Fecha = horacol;

                        Context.Estados.Add(estado);
                    }

                    SolicitudGruasHistories est = new SolicitudGruasHistories();
                    est.IdSolicitud = ob.ID_solicitud;
                    est.entidad = desjson.entidad;
                    est.nroOrden = desjson.nroOrden;
                    var dateF = DateTime.ParseExact(desjson.fechaSolicitud, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM-dd-yyy HH:mm:ss");
                    est.fechaSolicitud = Convert.ToDateTime(dateF);
                    est.tipoOrden = desjson.tipoOrden;
                    est.tipoTraslado = desjson.tipoTraslado;
                    est.causaInmovilizacion = desjson.causaInmovilizacion;
                    est.tipoInfraccion = desjson.tipoInfraccion;
                    est.direccionGeo = desjson.direccionGeo;
                    est.tipoZona = desjson.tipoZona;
                    est.direccionUrbana = desjson.direccionUrbana;
                    est.direccionRural = desjson.direccionRural;
                    est.observaciones = desjson.observaciones;
                    est.parqueaderoOrigen = desjson.parqueaderoOrigen;
                    est.parqueaderoDestino = desjson.parqueaderoDestino;
                    est.fechaTraslado = desjson.fechaTraslado;
                    est.horaTraslado = desjson.horaTraslado;
                    est.dependenciaOperativo = desjson.dependenciaOperativo;
                    est.nombreSolicitanteOperativo = desjson.nombreSolicitanteOperativo;
                    est.causalModificacion = desjson.causalModificacion;
                    est.causalCancelacion = desjson.causalCancelacion;
                    est.confirmacion = desjson.confirmacion;
                    Context.SolicitudGruasHistories.Add(est);


                    mensajes.codigo = "EXITOSO";
                    mensajes.mensaje = "Solicitud Registrada Satisfactoriamente.";
                    mensajes.severidad = "INFORMACIÓN";
                    respuesta.mensajes.Add(mensajes);

                    respuesta.respuesta.confirmacion = "1";
                    respuesta.respuesta.nroOrden = int.Parse(ob.Numero_de_orden_del_servicio);
                    respuesta.respuesta.tipoOrden = ob.Tipo_de_orden_de_servicio;
                    respuesta.respuesta.tipoTraslado = ob.Tipo_de_servicio_de_traslado;
                    respuesta.respuesta.fechaSolicitud = ob.Fecha_y_hora_solicitud_servicio;
                    respuesta.respuesta.fechaOrdenServicio = ob.Fecha_y_hora_de_orden_de_servicio;
                    respuesta.respuesta.causaInmovilizacion = int.Parse(ob.Causa_de_inmovilizacion);
                    respuesta.respuesta.entidad = ob.Entidad;

                    Context.CanGruasSolicitudes.AddRange(ListGruas);
                    Context.SolicitudTVSHistories.AddRange(ListVehiculos);
                    Context.SaveChanges();

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message + ", " + ex.InnerException.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;

                    //return "Error " + ex.Message;
                }
                throw;
            }
        }

        /// <summary>
        /// Respuesta de Atención del Servicio de gruas.
        /// WSRegistrarAt
        /// </summary>
        /// <param JSON=Data con Información del Servicio.></param>
        /// <returns></returns>
        public RespuestaWS registrarAtencion(AtencionSolicitudServicioDTO JSON, string Authorization)
        {
            try
            {
                var respuesta = new RespuestaWS();
                var mensajes = new mensajes();
                respuesta.respuesta = new respuesta();

                var AutValue = Context.AutTokens.FirstOrDefault(t => t.Token == Authorization);

                if (AutValue == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Token invalido";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    if (AutValue.TimeExpire <= horacol)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El token expiro.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                }

                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                AtencionSolicitudServicioDTO desjson = JsonConvert.DeserializeObject<AtencionSolicitudServicioDTO>(objectdes.ToString());

                if (desjson.direccionGeo != null && desjson.direccionGeo != "")
                {
                    var val1 = desjson.direccionGeo.ToString().Contains("X:");
                    var val2 = desjson.direccionGeo.ToString().Contains(",Y:");
                    var val3 = desjson.direccionGeo.ToString().Contains(",Z:");



                    if (val1 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    if (val2 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    if (val3 == false)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "Formato dirección geo incorrecto.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                }

                var vehiculosAtendidos = new List<SolicitudesVehiculosAtendidos>();

                if (desjson.nroOrden == 0)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo No de Orden de servicio es  obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.direccionGeo == "" || desjson.direccionGeo == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo dirección geo es  obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.placaGrua == "" || desjson.placaGrua == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo placa grúa es  obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {

                    SolicitudGruas ob = Context.SolicitudGruas.FirstOrDefault(t => t.Numero_de_orden_del_servicio == desjson.nroOrden.ToString());

                    if (ob == null)
                    {
                        mensajes.codigo = "Error, datos de entrada incorrectos";
                        mensajes.mensaje = "El numero de la orden no existe, favor validar.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                    else if (ob.Estado == "ATENDIDA")
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "La solicitud ya tiene una atención registrada.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }

                    //ob.Placa_grua_Numero = desjson.Placa_grua_Numero;
                    //ob.Placa_vehiculo_trasladar = desjson.Placa_vehiculo_trasladar;
                    //ob.Fecha_hora_Finalizacion_servicio = desjson.Fecha_hora_Finalizacion_servicio;
                    //ob.Parqueadero_destino = desjson.Parqueadero_destino;
                    //ob.Link_video_inmovilizacion = desjson.Link_video_inmovilizacion;
                    //ob.Fecha_hora_envio_informacion = desjson.Fecha_hora_envio_informacion;

                    ob.Fecha_hora_inicio_atencion_servicio = horacol;
                    ob.Estado = "ATENDIDA";

                    if (desjson.vehiculos != null)
                    {
                        foreach (var item in desjson.vehiculos)
                        {
                            var tipov = Context.TVehiculoInmovilizars.FirstOrDefault(t => t.Id == item.tipoVehiculo.ToString());
                            if (tipov == null)
                            {
                                mensajes.codigo = "Error";
                                mensajes.mensaje = "Verifique los tipos de vehículo, no es posible guardar.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }

                            if (item.placa == "" || item.placa == null)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo placa del vehículo es obligatorio.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                            else if (item.fechaIniAtencion == null)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo fecha inicio de antención es obligatoria.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                            else if (item.fechaFinAtencion == null)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo fecha fin de antención es obligatoria.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                            else if (item.tipoVehiculo == 0)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo tipo de vehículo es obligatorio.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                            else if (item.linkVideo == "" || item.linkVideo == null)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo link video es obligatorio.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }
                            else if (item.parqueadero == 0)
                            {
                                mensajes.codigo = "Error, datos de entrada incorrectos";
                                mensajes.mensaje = "Mensaje de error, campo parqueadero es obligatorio.";
                                mensajes.severidad = "ERROR";

                                respuesta.mensajes.Add(mensajes);
                                respuesta.respuesta = null;
                                return respuesta;
                            }

                            SolicitudesVehiculosAtendidos obj = new SolicitudesVehiculosAtendidos();
                            obj.IdSol = ob.ID_solicitud;
                            obj.Placa = item.placa;
                            obj.FechaIniAtencion = item.fechaIniAtencion;
                            obj.FechaFinAtencion = item.fechaFinAtencion;
                            obj.TipoVehiculo = item.tipoVehiculo.ToString();
                            obj.LinkVideo = item.linkVideo;
                            obj.Parqueadero = item.parqueadero;
                            obj.DateCreate = horacol;

                            vehiculosAtendidos.Add(obj);
                        }
                    }
                    else
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "La información de los vehículos es obligatoria.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }

                    Estados est = new Estados();
                    est.ID_solicitud = ob.ID_solicitud;
                    est.Nombre = "ATENDIDA";
                    est.Observaciones = null;
                    est.Fecha = horacol;


                    Context.SolicitudesVehiculosAtendidos.AddRange(vehiculosAtendidos);
                    Context.Estados.Add(est);
                    Context.SaveChanges();

                    //return "Atención Registrada Satisfactoriamente.";

                    mensajes.codigo = "EXITOSO";
                    mensajes.mensaje = "Atención Registrada Satisfactoriamente.";
                    mensajes.severidad = "INFORMACIÓN";
                    respuesta.mensajes.Add(mensajes);

                    respuesta.respuesta.confirmacion = "1";
                    respuesta.respuesta.nroOrden = int.Parse(ob.Numero_de_orden_del_servicio);
                    respuesta.respuesta.tipoOrden = ob.Tipo_de_orden_de_servicio;
                    respuesta.respuesta.tipoTraslado = ob.Tipo_de_servicio_de_traslado;
                    respuesta.respuesta.fechaSolicitud = ob.Fecha_y_hora_solicitud_servicio;
                    respuesta.respuesta.fechaOrdenServicio = ob.Fecha_y_hora_de_orden_de_servicio;
                    respuesta.respuesta.causaInmovilizacion = int.Parse(ob.Causa_de_inmovilizacion);
                    respuesta.respuesta.entidad = ob.Entidad;

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message + ", " + ex.InnerException.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;

                    //return "Error " + ex.Message;
                }
                throw;
            }
        }

        /// <summary>
        /// Para Registrar Novedades de las Grúas durante el servicio.
        /// WSRegistrarNovedad
        /// </summary>
        /// <param JSON= Data para Registrar Novedad en el Servicio de Grúas.></param>
        /// <returns></returns>
        public RespuestaWS registrarNovedadServicio(NovedadSolicitudServicioDTO JSON, string Authorization)
        {
            try
            {
                var respuesta = new RespuestaWS();
                var mensajes = new mensajes();
                respuesta.respuesta = new respuesta();

                var AutValue = Context.AutTokens.FirstOrDefault(t => t.Token == Authorization);

                if (AutValue == null)
                {
                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Token invalido";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    if (AutValue.TimeExpire <= horacol)
                    {
                        mensajes.codigo = "Error";
                        mensajes.mensaje = "El token expiro.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }
                }

                var select = Context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                NovedadSolicitudServicioDTO desjson = JsonConvert.DeserializeObject<NovedadSolicitudServicioDTO>(objectdes.ToString());


                var tnovedad = Context.TipoNovedades.FirstOrDefault(t => t.Id == desjson.tipoNovedad);

                if (tnovedad == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo novedad no corresponde a las parametrizadas.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }

                if (desjson.nroOrden == 0)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo No de Orden de servicio es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.tipoNovedad == "" || desjson.tipoNovedad == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo tipo de novedad es obligatorio.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.fechaReasignacion == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo fecha de llegada es obligatorio o no tiene el formato correcto.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else if (desjson.fechaLlegada == null)
                {
                    mensajes.codigo = "Error, datos de entrada incorrectos";
                    mensajes.mensaje = "Mensaje de error, campo fecha de llegada es obligatorio o no tiene el formato correcto.";
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {

                    SolicitudGruas ob = Context.SolicitudGruas.FirstOrDefault(t => t.Numero_de_orden_del_servicio == desjson.nroOrden.ToString());
                    var plgrua = Context.CanGruasSolicitudes.Where(t => t.IdSol == ob.ID_solicitud).ToList();

                    if (ob == null)
                    {
                        mensajes.codigo = "Error, datos de entrada incorrectos";
                        mensajes.mensaje = "Mensaje de error, el numero de la orden no existe, favor validar.";
                        mensajes.severidad = "ERROR";

                        respuesta.mensajes.Add(mensajes);
                        respuesta.respuesta = null;
                        return respuesta;
                    }

                    ob.Fecha_hora_novedad = horacol;
                    ob.Tipo_novedad = desjson.tipoNovedad;

                    if (desjson.gruaAnterior != null)
                    {
                        foreach (var item in desjson.gruaAnterior)
                        {
                            foreach (var itm in plgrua)
                            {
                                if (item.placa == itm.Placa)
                                {
                                    CanGruasSolicitudes Gr = Context.CanGruasSolicitudes.FirstOrDefault(t => t.Id == itm.Id && t.Estado == 1);

                                    if (Gr != null)
                                    {
                                        Gr.Estado = 0;
                                    }
                                }
                            }
                        }
                    }

                    var ListGruas = new List<CanGruasSolicitudes>();

                    var ListPlacasExist = "";

                    if (desjson.gruaNueva != null)
                    {
                        foreach (var it in desjson.gruaNueva)
                        {
                            var objtvs = new CanGruasSolicitudes();
                            if (it != null)
                            {
                                var tgua = Context.TipoGruas.FirstOrDefault(t => t.Id == it.tipoGrua.ToString());
                                var valplaca = Context.CanGruasSolicitudes.FirstOrDefault(t => t.Placa == it.placa);

                                if (it.tipoGrua == 0)
                                {
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "Mensaje de error, campo Tipo_de_Grua es obligatorio.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }
                                else if (it.placa == "" || it.placa == null)
                                {
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "Mensaje de error, campo placa grúa es obligatorio.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }
                                if (tgua == null)
                                {
                                    mensajes.codigo = "Error";
                                    mensajes.mensaje = "Mensaje de error, tipo grua no existe.";
                                    mensajes.severidad = "ERROR";

                                    respuesta.mensajes.Add(mensajes);
                                    respuesta.respuesta = null;
                                    return respuesta;
                                }

                                if (valplaca == null)
                                {
                                    objtvs.IdSol = ob.ID_solicitud;
                                    objtvs.TipoGrua = it.tipoGrua.ToString();
                                    objtvs.Placa = it.placa;
                                    objtvs.NroIdenConductor = it.nroIdenConductor;
                                    objtvs.TipoIdenConductor = it.tipoIdenConductor;
                                    objtvs.Estado = 1;

                                    ListGruas.Add(objtvs);
                                }
                                else
                                {
                                    ListPlacasExist += it.placa + ",";
                                }
                            }
                        }
                        Context.CanGruasSolicitudes.AddRange(ListGruas);
                    }



                    ob.Fecha_hora_reasignacion = desjson.fechaReasignacion;
                    ob.Parqueadero_destino = desjson.parqueaderoDestino;
                    ob.Observaciones_Novedad = desjson.observaciones;
                    ob.Fecha_hora_llegada_lugar_solicitud = desjson.fechaLlegada;
                    ob.Estado = "REASIGNADA";
                    ob.Fecha_Cierre_Auto = horacol.AddMinutes(Convert.ToDouble(select.Horas));


                    Estados est = new Estados();
                    est.ID_solicitud = ob.ID_solicitud;
                    est.Nombre = "REASIGNADA";
                    est.Observaciones = desjson.observaciones;
                    est.Fecha = horacol;
                    Context.Estados.Add(est);


                    Context.SaveChanges();

                    //return "Novedad Registrada Satisfactoriamente.";

                    mensajes.codigo = "EXITOSO";
                    mensajes.mensaje = "Novedad Registrada Satisfactoriamente." + (ListPlacasExist != "" ? "No se registraron las placas " + ListPlacasExist.ToString() + " ya se habían registrado previamente." : "");
                    mensajes.severidad = "INFORMACIÓN";
                    respuesta.mensajes.Add(mensajes);

                    respuesta.respuesta.confirmacion = "1";
                    respuesta.respuesta.nroOrden = int.Parse(ob.Numero_de_orden_del_servicio);
                    respuesta.respuesta.tipoOrden = ob.Tipo_de_orden_de_servicio;
                    respuesta.respuesta.tipoTraslado = ob.Tipo_de_servicio_de_traslado;
                    respuesta.respuesta.fechaSolicitud = ob.Fecha_y_hora_solicitud_servicio;
                    respuesta.respuesta.fechaOrdenServicio = ob.Fecha_y_hora_de_orden_de_servicio;
                    respuesta.respuesta.causaInmovilizacion = int.Parse(ob.Causa_de_inmovilizacion);
                    respuesta.respuesta.entidad = ob.Entidad;

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message + ", " + ex.InnerException.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }
                else
                {
                    var respuesta = new RespuestaWS();
                    var mensajes = new mensajes();
                    respuesta.respuesta = new respuesta();

                    mensajes.codigo = "Error";
                    mensajes.mensaje = "Error " + ex.Message;
                    mensajes.severidad = "ERROR";

                    respuesta.mensajes.Add(mensajes);
                    respuesta.respuesta = null;
                    return respuesta;
                }

                throw;
            }
        }

        /// <summary>
        /// Generar Token Autenticación WS SDM
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public TokenDTO login(string username)
        {
            TokenDTO keyToken = new TokenDTO();

            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            AutTokens tokensvalidate = Context.AutTokens.FirstOrDefault();

            if (tokensvalidate == null)
            {
                AutTokens aut = new AutTokens();

                aut.Token = "Bearer " + handler.WriteToken(token);
                aut.TimeExpire = horacol.AddHours(24);

                Context.AutTokens.Add(aut);
                Context.SaveChanges();
            }
            else
            {
                tokensvalidate.Token = "Bearer " + handler.WriteToken(token);
                tokensvalidate.TimeExpire = horacol.AddHours(24);
                Context.SaveChanges();
            }

            keyToken.token = handler.WriteToken(token);

            return keyToken;
        }
        #endregion
    }
}