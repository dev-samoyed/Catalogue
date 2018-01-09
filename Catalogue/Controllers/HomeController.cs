using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Catalogue.Models.Tables;

namespace Catalogue.Controllers
{
    public class HomeController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        public ActionResult Index()
        {
            var administrations = db.Administrations.Include(e => e.Departments);
            ViewBag.Administrations = administrations;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}