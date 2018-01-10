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

        public ActionResult DepartmentEmployees (int DepartmentId)
        {
            var employees = db.Employees
                .Where(e => e.DepartmentId == DepartmentId)
                .OrderBy(d => d.EmployeeFullName)
                .Include(c => c.Department)
                .Include(b => b.Position).ToList();

            return View(employees);
        }

        public ActionResult EmployeeSearch (string name)
        {
            var employeeMatches = db.Employees
                .Where(a => a.EmployeeFullName.Contains(name))
                .Include(p => p.Position)
                .Include(d => d.Department)
                .ToList();

            //if (employeeMatches.Count <= 0)
            //{
            //    return HttpNotFound();
            //}

            return PartialView(employeeMatches);
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