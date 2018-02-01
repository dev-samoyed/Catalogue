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
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            IPagedList<Employee> employees = db.Employees.OrderBy(e => e.EmployeeFullName).Include(d => d.Department).ToPagedList(pageNumber, pageSize);
            //ViewBag.Administrations = administrations;

            List<Position> positions = db.Positions.ToList();
            ViewBag.Positions = positions;

            List<Department> departments = db.Departments.ToList();
            ViewBag.Departments = departments;

            List<Administration> admins = db.Administrations.ToList();
            ViewBag.Admins = admins;

            List<Division> divisions = db.Divisions.ToList();
            ViewBag.Divisions = divisions;

            return View(employees);
        }

        /// <summary>
        /// Shows list of employees of the department
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns>list of employees</returns>
        public ActionResult AjaxDepartmentEmployees (int? DepartmentId)
        {
            if (DepartmentId == null)
            {
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            IQueryable<Employee> query = db.Employees
                .Where(e => e.DepartmentId == DepartmentId);

            List<Employee> employees = AddIncludes(query);

            return PartialView(employees);
        }

        /// <summary>
        /// Shows employee's info by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>employee's info</returns>
        public ActionResult AjaxShowEmployee (int? id)
        {
            if (id == null)
            {
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            Employee employee = db.Employees
                .Include(p => p.Position)
                .Include(d => d.Department)
                .SingleOrDefault(e => e.EmployeeId == id);

            return PartialView(employee);
        }

        private List<Employee> AddIncludes(IQueryable<Employee> query)
        {
            List<Employee> employeeMatches = query
                .OrderBy(c => c.EmployeeFullName)
                .Include(p => p.Position)
                .Include(d => d.Department)
                .ToList();

            return employeeMatches;
        }

        public string Test ()
        {
            bool result = User.IsInRole("admin");
            return result ? "admin" : "user";
        }

    }
}