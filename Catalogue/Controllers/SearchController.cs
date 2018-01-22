using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Catalogue.Models.Tables;
using System.Net;

using Catalogue.Controllers.Utils;

namespace Catalogue.Controllers
{
    public class SearchController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        /// <summary>
        /// Forms partial view with a list of found employees
        /// </summary>
        /// <param name="name"></param>
        /// <returns>PartialView with a list of employees</returns>
        public ActionResult EmployeeSearch(string name, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            ViewBag.Position = db.Positions.Find(positionId);
            ViewBag.Department = db.Departments.Find(departmentId);

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
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            if (positionId != null)
            {
                searchQuery = searchQuery.Where(e => e.PositionId == positionId);
            }

            if (departmentId != null)
            {
                searchQuery = searchQuery.Where(e => e.DepartmentId == departmentId);
            }

            if (administrationId != null)
            {
                List<int> departmentIds = db.Departments.Where(d => d.AdministrationId == administrationId).Select(i => i.DepartmentId).ToList();
                searchQuery = searchQuery.Where(e => departmentIds.Contains(e.DepartmentId));
            }

            if (divisionId != null)
            {
                List<int> administrationIds = db.Administrations.Where(a => a.DivisionId == divisionId).Select(i => i.AdministrationId).ToList();
                List<int> departmentsIds = db.Departments.Where(d => administrationIds.Contains(d.AdministrationId)).Select(i => i.DepartmentId).ToList();
                searchQuery = searchQuery.Where(e => departmentsIds.Contains(e.DepartmentId));
            }

            employeeMatches = AddIncludes(searchQuery);

            if (employeeMatches.Count <= 0)
            {
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Home/Error.cshtml");
            }

            return PartialView(employeeMatches);
        }

        /// <summary>
        /// Forms a search query for employees by one param
        /// </summary>
        /// <param name="part_1"></param>
        /// <returns>query</returns>
        private IQueryable<Employee> BuildSearchQuery(string part_1)
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
        private List<Employee> AddIncludes(IQueryable<Employee> query)
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