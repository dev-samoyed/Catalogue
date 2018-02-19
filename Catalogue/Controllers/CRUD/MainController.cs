using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catalogue.Controllers.CRUD
{
    public class MainController : Controller
    {
        // GET: Main
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}