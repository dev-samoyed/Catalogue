using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Catalogue.Models.Tables;
using System.Net;

namespace Catalogue.Controllers
{
    public class HomeController : Controller
    {
        // errors string values
        public static readonly string notFound = "Не найдено";

        CatalogueContext db = new CatalogueContext();

        public ActionResult Index()
        {
            IQueryable<Administration> administrations = db.Administrations.Include(e => e.Departments);
            ViewBag.Administrations = administrations;

            return View();
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
                ViewBag.Error = notFound;
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
                ViewBag.Error = notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            Employee employee = db.Employees
                .Include(p => p.Position)
                .Include(d => d.Department)
                .SingleOrDefault(e => e.EmployeeId == id);

            return PartialView(employee);
        }

        /// <summary>
        /// Forms partial view with a list of found employees
        /// </summary>
        /// <param name="name"></param>
        /// <returns>PartialView with a list of employees</returns>
        public ActionResult EmployeeSearch (string name)
        {
            int maxNumberOfWordsInFullName = 3;

            Stack<string> parts = new Stack<string>();
            string[] partsArray = name.Split(' ');

            List<Employee> employeeMatches = new List<Employee>();
            IQueryable<Employee> searchQuery = Enumerable.Empty<Employee>().AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                int wordsAmount = partsArray.Length < maxNumberOfWordsInFullName ? partsArray.Length : maxNumberOfWordsInFullName;
                for (int i = 0; i < wordsAmount; i++)
                    parts.Push(partsArray[i]);
            }

            if (parts.Count == 1)
            {
                string part_1 = parts.Pop();
                searchQuery = BuildSearchQuery(part_1);
            }
            else if (parts.Count == 2)
            {
                string part_1 = parts.Pop(), part_2 = parts.Pop();
                searchQuery = BuildSearchQuery(part_1, part_2);
            }
            else if (parts.Count == 3)
            {
                string part_1 = parts.Pop(), part_2 = parts.Pop(), part_3 = parts.Pop();
                searchQuery = BuildSearchQuery(part_1, part_2, part_3);
            }
            else if (parts.Count <= 0)
            {
                ViewBag.Error = notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            employeeMatches = AddIncludes(searchQuery);

            if (employeeMatches.Count <= 0)
            {
                ViewBag.Error = notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            return PartialView(employeeMatches);
        }

        /// <summary>
        /// Forms a search query for employees by one param
        /// </summary>
        /// <param name="part_1"></param>
        /// <returns>query</returns>
        private IQueryable<Employee> BuildSearchQuery (string part_1)
        {
            IQueryable<Employee> query = db.Employees
                .Where(n => n.EmployeeFullName.Contains(part_1));

            return query;
        }

        /// <summary>
        /// Forms a search query for employees by two params
        /// </summary>
        /// <param name="part_1"></param>
        /// <param name="part_2"></param>
        /// <returns>query</returns>
        private IQueryable<Employee> BuildSearchQuery(string part_1, string part_2)
        {
            IQueryable<Employee> query = db.Employees
                .Where(e => e.EmployeeFullName.Contains(part_1)
                        && e.EmployeeFullName.Contains(part_2));

            return query;
        }

        /// <summary>
        /// Forms a search query for employees by three params
        /// </summary>
        /// <param name="part_1"></param>
        /// <param name="part_2"></param>
        /// <param name="part_3"></param>
        /// <returns>query</returns>
        private IQueryable<Employee> BuildSearchQuery(string part_1, string part_2, string part_3)
        {
            IQueryable<Employee> query = db.Employees
                .Where(e => e.EmployeeFullName.Contains(part_1)
                        && e.EmployeeFullName.Contains(part_2)
                        && e.EmployeeFullName.Contains(part_3));

            return query;
        }

        /// <summary>
        /// Adds relationships Position and Department to the (Employee) query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>list of employees</returns>
        private List<Employee> AddIncludes (IQueryable<Employee> query)
        {
            List<Employee> employeeMatches = query
                .OrderBy(c => c.EmployeeFullName)
                .Include(p => p.Position)
                .Include(d => d.Department)
                .ToList();

            return employeeMatches;
        }
        
    }
}