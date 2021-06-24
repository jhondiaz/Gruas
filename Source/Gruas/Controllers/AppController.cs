using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gruas.Controllers
{
    /// <summary>
    /// Create an ActionResult and PartialView for each angular partial view you want to attatch to a route in the angular app.js file.
    /// </summary>
    [Authorize]
    public class AppController : Controller
    {
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return PartialView();
        }



        public ActionResult Home()
        {
            return PartialView();
        }



        [Authorize]
        public ActionResult TodoManager()
        {
            return PartialView();
        }
    }
}