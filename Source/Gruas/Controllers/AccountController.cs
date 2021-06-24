using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Gruas.Models;
using Gruas.Providers;
using Gruas.Results;
using Gruas.Models.Utility;
using System.Net;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using Services.DataAccess;
using Services.Entitys.Entities;
using Services.Entitys.DTOs;
using System.Web.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;

namespace Gruas.Controllers
{

    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiBaseController
    {
        static TimeZoneInfo horazone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        DateTime horacol = TimeZoneInfo.ConvertTime(DateTime.Now, horazone);

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);           

            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync("34f6e97e-6e00-4734-9f38-b54e96a00eca", model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            User user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                var RoleName = await UserManager.GetRolesAsync(user.Roles.FirstOrDefault().UserId);

                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user, RoleName.FirstOrDefault());
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }
        [AllowAnonymous]
        [Route("ResetPass")]
        public async Task<HttpResponseMessage> ResetPass(RegisterExternalBindingModel m)
        {
            try
            {

                string password = Membership.GeneratePassword(10, 5);
                password = Regex.Replace(password, @"[^a-zA-Z0-9]",Match => "9")+'$'+'b';

                string newPwd = Guid.NewGuid().ToString().Substring(0, 8) + '$';

                List<string> errors = new List<string>();
                errors = ModelErrorChecker.Check(ModelState);

                var resultsC = UserManager.FindByEmail(m.Email);
                if (resultsC == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "El Email No se Encuentra Registrado, Contacte el Administrador");
                }
                else
                {
                    var results = UserManager.GeneratePasswordResetToken(resultsC.Id);
                    if (results == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Se Ha Presentado Un Error, Intente Nuevamente");
                    }
                    else
                    {
                        var resultd = UserManager.ResetPassword(resultsC.Id, results, password);
                        if (resultd == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Se Ha Presentado Un Error, Intente Nuevamente");
                        }
                        else
                        {
                            if (resultd.Succeeded)
                            {
                                var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                                LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                                inline.ContentId = Guid.NewGuid().ToString();

                                SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                                server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                                server.EnableSsl = true;
                                MailMessage mnsj = new MailMessage();
                                mnsj.Subject = "Creación de Usuario";
                                mnsj.To.Add(new MailAddress(m.Email));
                                mnsj.From = new MailAddress(m.Email, "Plataforma Gruas");
                                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                                string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                                body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                                body += "<BODY><div style='width: 677px;height: 64px;'>";
                                body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                                body += "</div>";
                                body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                                body += "<h1 style='color: cadetblue'>Cambió de Contraseña</h1><h3 style='color:#878788'>Buen Día " + resultsC.firstName + "</h3>";
                                body += "<h4 style='color:#878788'>Se ha efectuado el cambio de contraseña de forma satisfactoria, los datos los vera relacionados a continuación:</h4>";
                                body += "<h4 style='color:#878788'>Usuario: " + resultsC.Email + " </br>Contraseña Nueva: " + password + "</h4></DIV></BODY></HTML>";

                                ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                                AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                                mnsj.AlternateViews.Add(alternate);
                                alternate.LinkedResources.Add(inline);



                                mnsj.Body = body;
                                server.Send(mnsj);

                                DateTime fechaHoy = DateTime.Now;
                                var fechaToday = fechaHoy.ToString("d");

                                Context context = new Context();
                                AspNetUsers ob = context.AspNetUsers.Where(t => t.Id == resultsC.Id).FirstOrDefault();
                                ob.AccessFailedCount = 1;
                                ob.LockoutEnabled = true;
                                if (ob.DiasExpiracion == 0)
                                {
                                    ob.DiasExpiracion = 30;
                                }
                                ob.LockoutEndDateUtc = fechaHoy.AddDays(Convert.ToDouble(ob.DiasExpiracion));
                                context.SaveChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, "Cambio de Contraseña Satisfactorio");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotAcceptable, ((string[])resultd.Errors)[0]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Se ha producido un error.");
                throw;
            }
        }

        public dynamic valipass(string id, string passnew)
        {
            try
            {
                List<string> errors = new List<string>();
                errors = ModelErrorChecker.Check(ModelState);

                if (errors.Count > 0)
                {
                    return errors[0] + "," + 2;
                }
                else
                {

                    Context context = new Context();
                    var validpass = context.PasswordHistorys.Where(t => t.User == id).FirstOrDefault();

                    var input = passnew;
                    var result = "";
                    using (SHA1Managed sha1 = new SHA1Managed())
                    {
                        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                        var sb = new StringBuilder(hash.Length * 2);

                        foreach (byte b in hash)
                        {
                            // can be "x2" if you want lowercase
                            sb.Append(b.ToString("X2"));
                        }

                        result = sb.ToString();
                    }

                    var a = result;

                    if (validpass == null)
                    {
                        PasswordHistorys pass = new PasswordHistorys();
                        pass.User = id;
                        pass.Pass1 = result;
                        pass.Pass2 = null;
                        pass.Pass3 = null;
                        pass.Count = 1;

                        context.PasswordHistorys.Add(pass);
                        context.SaveChanges();
                        return "0,0";
                    }
                    else
                    {
                        if (validpass.Count == 1)
                        {
                            if (validpass.Pass1 == result || validpass.Pass2 == result || validpass.Pass3 == result)
                            {
                                return "1,1";
                            }
                            else
                            {
                                validpass.Pass2 = result;
                                validpass.Count = 2;
                                context.SaveChanges();
                                return "0,0";
                            }
                        }
                        else if (validpass.Count == 2)
                        {
                            if (validpass.Pass1 == result || validpass.Pass2 == result || validpass.Pass3 == result)
                            {
                                return "1,1";
                            }
                            else
                            {
                                validpass.Pass3 = result;
                                validpass.Count = 3;
                                context.SaveChanges();
                                return "0,0";
                            }
                        }
                        else
                        {
                            if (validpass.Pass1 == result || validpass.Pass2 == result || validpass.Pass3 == result)
                            {
                                return "1,1";
                            }
                            else
                            {
                                validpass.Pass1 = result;
                                validpass.Count = 1;
                                context.SaveChanges();
                                return "0,0";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [AllowAnonymous]
        [Route("ChangePass")]
        public async Task<HttpResponseMessage> ChangePass(ChangePasswordBindingModel model)
        {
            try
            {
                var resultpvalpass = valipass(model.Id, model.NewPassword);


                var spl = resultpvalpass.Split(',');
                var splk = ((string[])spl)[1];
                if (splk == "2")
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ((string[])spl)[0]);
                }

                if (splk == "1")
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Esta contraseña ya se uso previamente.");
                }
                else
                {
                    Context context = new Context();



                    List<string> errors = new List<string>();
                    errors = ModelErrorChecker.Check(ModelState);



                    IdentityResult results = UserManager.ChangePassword(model.Id, model.OldPassword, model.NewPassword);

                    if (!results.Succeeded)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, ((string[])results.Errors)[0]);
                    }
                    else
                    {
                        var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                        LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                        inline.ContentId = Guid.NewGuid().ToString();

                        SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                        server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                        server.EnableSsl = true;
                        MailMessage mnsj = new MailMessage();
                        mnsj.Subject = "Creación de Usuario";
                        mnsj.To.Add(new MailAddress(model.Email));
                        mnsj.From = new MailAddress(model.Email, "Plataforma Gruas");
                        //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                        string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                        body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                        body += "<BODY><div style='width: 677px;height: 64px;'>";
                        body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                        body += "</div>";
                        body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                        body += "<h1 style='color: cadetblue'>Cambió de Contraseña</h1><h3 style='color:#878788'>Buen Día " + model.firstName + "</h3>";
                        body += "<h4 style='color:#878788'>Se ha efectuado el cambio de contraseña de forma satisfactoria, los nuevos datos de acceso los vera relacionados a continuación:</h4>";
                        body += "<h4 style='color:#878788'>Usuario: " + model.Email + " </br>Contraseña Nueva: " + model.NewPassword + "</h4></DIV></BODY></HTML>";

                        ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                        AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                        mnsj.AlternateViews.Add(alternate);
                        alternate.LinkedResources.Add(inline);



                        mnsj.Body = body;
                        server.Send(mnsj);

                        AspNetUsers ob = context.AspNetUsers.Where(t => t.Id == model.Id).FirstOrDefault();
                        ob.AccessFailedCount = 0;
                        context.SaveChanges();


                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [AllowAnonymous]
        [Route("updatereg")]
        public async Task<HttpResponseMessage> updatereg(RegisterBindingModel model)
        {
            try
            {
                Context context = new Context();
                AspNetUsers ob = context.AspNetUsers.Where(t => t.Id == model.Id).FirstOrDefault();

                ob.firstName = model.firstName;
                ob.TipoDocumento = model.TipoDocumento;

                ob.NumeroDocumento = model.NumeroDocumento;
                ob.PhoneNumber = model.PhoneNumber;
                ob.Email = model.Email;
                if (ob.PlacaAgente != null)
                {
                    ob.PlacaAgente = model.PlacaAgente;
                }
                ob.NombreJefe = model.NombreJefe;
                ob.LockoutEnabled = model.LockoutEnabled;
                ob.DiasExpiracion = model.DiasExpiracion;

                DateTime thisDate = new DateTime(horacol.Year, horacol.Month, horacol.Day);
                ob.LockoutEndDateUtc = thisDate.AddDays(Convert.ToDouble(model.DiasExpiracion));


                context.SaveChanges();
                var mensajeinsert = "Actualización Satisfactoria.";

                return Request.CreateResponse(HttpStatusCode.OK, mensajeinsert);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, ex);
                throw ex;
            }
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(RegisterBindingModel model)
        {
            try
            {
                List<string> errors = new List<string>();
                errors = ModelErrorChecker.Check(ModelState);

                var id = "";
                var Email = "";


                DateTime thisDate = new DateTime(horacol.Year, horacol.Month, horacol.Day);

                model.LockoutEndDateUtc = thisDate.AddDays(Convert.ToDouble(model.DiasExpiracion));

                if (errors.Count == 0)
                {
                    if (model.PlacaAgente == null)
                    {
                        model.PlacaAgente = "";
                    }

                    var user = new User() { UserName = model.Email, Email = model.Email, firstName = model.firstName, LockoutEnabled = model.LockoutEnabled, PhoneNumber = model.PhoneNumber, TipoDocumento = model.TipoDocumento, NumeroDocumento = model.NumeroDocumento, PlacaAgente = model.PlacaAgente, NombreJefe = model.NombreJefe, TelefonoJefe = model.TelefonoJefe, Entidad = model.Entidad, Agente = model.Agente, DiasExpiracion = model.DiasExpiracion, LockoutEndDateUtc = model.LockoutEndDateUtc };

                    id = user.Id;
                    Email = user.Email;

                    

                    if (UserManager.FindByEmail(model.Email) == null)
                    {

                        IdentityResult result = await UserManager.CreateAsync(user, model.Password);


                        if (!result.Succeeded)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, ((string[])result.Errors)[0]);
                        }
                        else
                        {



                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Email address is already in use.");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, errors[0]);
                }

                if (model.Tipe == "ReqUser")
                {
                    var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                    LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                    inline.ContentId = Guid.NewGuid().ToString();

                    SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                    server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                    server.EnableSsl = true;
                    MailMessage mnsj = new MailMessage();
                    mnsj.Subject = "Creación de Usuario";
                    mnsj.To.Add(new MailAddress(model.Email));
                    mnsj.From = new MailAddress(model.Email, "Plataforma Gruas");
                    //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                    string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                    body += "<BODY><div style='width: 677px;height: 64px;'>";
                    body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                    body += "</div>";
                    body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                    body += "<h1 style='color: cadetblue'>Solicitud de Creación</h1><h3 style='color:#878788'>Buen Día " + model.firstName + "</h3>";
                    body += "<h4 style='color:#878788'>Se ha creado de forma satisfactoria su perfil de acceso a la plataforma de grúas por parte del Administrador; los datos los verá relacionados a continuación:</h4>";
                    body += "<h4 style='color:#878788'>Usuario: " + model.Email + " </br>Contraseña: " + model.Password + "</h4></DIV></BODY></HTML>";

                    ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                    mnsj.AlternateViews.Add(alternate);
                    alternate.LinkedResources.Add(inline);
                    mnsj.Body = body;
                    server.Send(mnsj);

                    Context context = new Context();
                    RequestUsers ob = context.RequestUsers.Where(t => t.Id == model.IdSolicitud).FirstOrDefault();

                    ob.Validado = 1;
                    context.SaveChanges();
                }
                else
                {
                    var path = String.Format(@"{0}img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                    LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                    inline.ContentId = Guid.NewGuid().ToString();

                    SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                    server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                    server.EnableSsl = true;
                    MailMessage mnsj = new MailMessage();
                    mnsj.Subject = "Creación de Usuario";
                    mnsj.To.Add(new MailAddress(model.Email));
                    mnsj.From = new MailAddress(model.Email, "Plataforma Gruas");
                    //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                    string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                    body += "<BODY><div style='width: 677px;height: 64px;'>";
                    body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                    body += "</div>";
                    body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                    body += "<h1 style='color: cadetblue'>Credenciales de Acceso 'Creación'</h1><h3 style='color:#878788'>Buen Día " + model.firstName + "</h3>";
                    body += "<h4 style='color:#878788'>Se ha creado de forma satisfactoria su perfil de acceso a la plataforma de grúas, los datos los verá relacionados a continuación:</h4>";
                    body += "<h4 style='color:#878788'>Usuario: " + model.Email + " </br>Contraseña: " + model.Password + "</h4></DIV></BODY></HTML>";

                    ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                    mnsj.AlternateViews.Add(alternate);
                    alternate.LinkedResources.Add(inline);



                    mnsj.Body = body;
                    server.Send(mnsj);
                }


                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (Exception x)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, x);
                throw x;
            }
        }



        [Route("RegisterReports")]
        public async Task<HttpResponseMessage> RegisterReports(RegisterReportsBindingModel model)
        {
            try
            {

                List<string> errors = new List<string>();
                errors = ModelErrorChecker.Check(ModelState);

                if (errors.Count == 0)
                {
                    var user = new User()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        firstName = model.Name,
                        LockoutEnabled = !model.IsActivo,

                    };


                    if (UserManager.FindByEmail(model.Email) == null)
                    {

                        IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                        if (!result.Succeeded)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors);
                        }
                        else
                        {

                            var Resuluser = await UserManager.FindByEmailAsync(model.Email);
                            // await UserManager.AddToRoleAsync(Resuluser.Id, RolesUsers.Reports);




                        }
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Email ya esta registrado");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, errors);

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message);
            }

        }


        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new User() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }



        [AllowAnonymous]
        [Route("LoginApp")]
        public dynamic login()
        {
            try
            {
                var username = HttpContext.Current.Request.Headers["username"];
                var password = HttpContext.Current.Request.Headers["password"];

                var results = UserManager.FindAsync(username, password);

                if (results.Result == null)
                {
                    return "Usuario o Contraseña Incorrectos.";
                }
                else if (results.Result.LockoutEnabled == false)
                {
                    return "El usuario se encuentra inactivo, Contacte el Administrador.";
                }
                else
                {
                    if (results.Result.Agente == true)
                    {
                        return results.Result;
                    }
                    else
                    {
                        return "El Usuario que Intenta Ingresar no Está Autorizado, Contacte el Administrador.";
                    }
                }
            }
            catch (Exception Ex)
            {
                return Ex;
                throw;
            }
        }

        [AllowAnonymous]
        [Route("ValAcceso")]
        public dynamic login(AspNetUsers user)
        {
            try
            {
                Context context = new Context();
                var query = context.AspNetUsers.Where(t => t.Email == user.Email).FirstOrDefault();

                var fecha = query.LockoutEndDateUtc;

                DateTime thisDate = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day);
                var formatfecha = thisDate.ToString("d");

                DateTime fechaHoy = DateTime.Now;
                var fechaToday = fechaHoy.ToString("d");



                if (formatfecha == fechaToday)
                {
                    AspNetUsers ob = context.AspNetUsers.Where(t => t.Id == query.Id).FirstOrDefault();

                    ob.LockoutEnabled = false;
                    context.SaveChanges();

                    return "La Contraseña Expiro";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                return ex;
                throw;
            }
        }

        #endregion
    }
}
