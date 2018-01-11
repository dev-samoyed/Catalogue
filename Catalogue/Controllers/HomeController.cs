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

        public ActionResult ShowEmployee (int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Employee employee = db.Employees.Include(p => p.Position).Include(d => d.Department).SingleOrDefault(e => e.EmployeeId == id);
            return View(employee);
        }

        /// <summary>
        /// Возвращает частичное представление со списком найденных сотрудников
        /// </summary>
        /// <param name="name"></param>
        /// <returns>PartialView</returns>
        public ActionResult EmployeeSearch (string name)
        {
            Stack<string> parts = new Stack<string>();
            string[] partsArray = name.Split(' ');
            int wordsAmount = partsArray.Length < 3 ? partsArray.Length : 3;

            List<Employee> employeeMatches = new List<Employee>();

            if (!string.IsNullOrEmpty(name))
                for (int i = 0; i < wordsAmount; i++)
                    parts.Push(partsArray[i]);

            if (parts.Count == 1)
            {
                string part_1 = parts.Pop();
                employeeMatches = SearchResults(part_1);
            }
            else if (parts.Count == 2)
            {
                string part_1 = parts.Pop(), part_2 = parts.Pop();
                employeeMatches = SearchResults(part_1, part_2);
            }
            else if (parts.Count == 3)
            {
                string part_1 = parts.Pop(), part_2 = parts.Pop(), part_3 = parts.Pop();
                employeeMatches = SearchResults(part_1, part_2, part_3);
            }
            else if (parts.Count <= 0)
            {
                return PartialView("~/Views/Home/NotFound.cshtml");
            }

            return PartialView(employeeMatches);
        }

        /// <summary>
        /// Поиск совпадений в сотрудниках по одному значению
        /// </summary>
        /// <param name="part_1"></param>
        /// <returns>employeeMatches</returns>
        private List<Employee> SearchResults (string part_1)
        {
            IQueryable<Employee> query = db.Employees
                .Where(n => n.EmployeeFullName.Contains(part_1));

            List<Employee> employeeMatches = AddIncludes(query);

            return employeeMatches;
        }

        /// <summary>
        /// Поиск совпадений в сотрудниках по двум значениям
        /// </summary>
        /// <param name="part_1"></param>
        /// <param name="part_2"></param>
        /// <returns></returns>
        private List<Employee> SearchResults (string part_1, string part_2)
        {
            IQueryable<Employee> query = db.Employees
                .Where(n => n.EmployeeFullName.Contains(part_1))
                .Where(m => m.EmployeeFullName.Contains(part_2));

            List<Employee> employeeMatches = AddIncludes(query);

            return employeeMatches;
        }

        /// <summary>
        /// Поиск совпадений в сотрудниках по трем значениям
        /// </summary>
        /// <param name="part_1"></param>
        /// <param name="part_2"></param>
        /// <param name="part_3"></param>
        /// <returns></returns>
        private List<Employee> SearchResults (string part_1, string part_2, string part_3)
        {
            IQueryable<Employee> query = db.Employees
                .Where(n => n.EmployeeFullName.Contains(part_1))
                .Where(m => m.EmployeeFullName.Contains(part_2))
                .Where(l => l.EmployeeFullName.Contains(part_3));

            List<Employee> employeeMatches = AddIncludes(query);

            return employeeMatches;
        }

        /// <summary>
        /// Окончание запроса поиска совпадений в сотрудника,
        /// добавляет сотрднику связи с его отделом и должностью
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<Employee> AddIncludes (IQueryable<Employee> query)
        {
            List<Employee> employeeMatches = query
                .Include(p => p.Position)
                .Include(d => d.Department)
                .ToList();

            return employeeMatches;
        }
    }
}