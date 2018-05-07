using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Catalogue.Models.Tables;
using PagedList.Mvc;
using PagedList;

using Catalogue.Controllers.Utils;
using System.Web.UI;

namespace Catalogue.Controllers
{
    public class HomeController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IPagedList<Employee> employees = db.Employees.OrderBy(e => e.EmployeeFullName).ToPagedList(pageNumber, pageSize);

            List<Position> positions = db.Positions.OrderBy(p => p.PositionName).ToList();
            ViewBag.Positions = positions;

            List<Department> departments = db.Departments.OrderBy(d => d.DepartmentName).ToList();
            ViewBag.Departments = departments;

            List<Administration> admins = db.Administrations.OrderBy(a => a.AdministrationName).ToList();
            ViewBag.Admins = admins;

            List<Division> divisions = db.Divisions.OrderBy(d => d.DivisionName).ToList();
            ViewBag.Divisions = divisions;

            string view = "";
            if (User.IsInRole("manager"))
                view = "~/Views/Home/ManagerIndex.cshtml";
            else
                view = "~/Views/Home/Index.cshtml";

            return View(view, employees);
        }
    }
}