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
        /// Forms a partial view with a list of found employees
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
                searchQuery = searchQuery.Where(e => e.PositionId == positionId);

            if (departmentId != null)
                searchQuery = searchQuery.Where(e => e.DepartmentId == departmentId);

            if (administrationId != null)
            {
                List<int> departmentIds = GetDepartmentIds("administration", administrationId);
                searchQuery = searchQuery.Where(e => departmentIds.Contains(e.DepartmentId));
            }
                

            if (divisionId != null)
            {
                List<int> departmentIds = GetDepartmentIds("division", divisionId);
                searchQuery = searchQuery.Where(e => departmentIds.Contains(e.DepartmentId));
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
        /// Forms a partial view with a list of found entities
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public ActionResult AdminSearch (string title, string type)
        {
            string view = "~/Views/Search/";
            string[] words = title.ToLower().Split(' ');

            if (title.Length <= 0)
            {
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Search/Error.cshtml");
            }

            if (type == "department")
            {
                List<Department> departments = BuildDepartmentSearchQuery(words).ToList();
                if (departments.Count <= 0)
                {
                    ViewBag.Error = Errors.notFound;
                    view += "Error.cshtml";
                } else
                {
                    ViewBag.Departments = departments;
                    view += "Departments.cshtml";
                }
            }
            else if (type == "administration")
            {
                List<Administration> administrations = BuildAdministartionSearchQuery(words).ToList();
                if (administrations.Count <= 0)
                {
                    ViewBag.Error = Errors.notFound;
                    view += "Error.cshtml";
                } else
                {
                    ViewBag.Administrations = administrations;
                    view += "Administrations.cshtml";
                }
            }
            else if (type == "position")
            {
                List<Position> positions = BuildPositionSearchQuery(words).ToList();
                if (positions.Count <= 0)
                {
                    ViewBag.Error = Errors.notFound;
                    view += "Error.cshtml";
                } else
                {
                    ViewBag.Positions = positions;
                    view += "Positions.cshtml";
                }
            }
            else if (type == "division")
            {
                List<Division> divisions = BuildDivisionSearchQuery(words).ToList();
                if (divisions.Count <= 0)
                {
                    ViewBag.Error = Errors.notFound;
                    view += "Error.cshtml";
                } else
                {
                    ViewBag.Divisions = divisions;
                    view += "Division.cshtml";
                }
            }

            return PartialView(view);
        }

        /// <summary>
        /// Builds a search query that matches all words of the array 'words'
        /// </summary>
        /// <param name="words"></param>
        /// <returns>IEnumerable query</returns>
        private IEnumerable<Department> BuildDepartmentSearchQuery (params string[] words)
        {
            IEnumerable<Department> query = db.Departments
                .Include(a => a.Administration)
                .ToList()
                .Where(d => words.All(d.DepartmentName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Administration> BuildAdministartionSearchQuery (params string[] words)
        {
            IEnumerable<Administration> query = db.Administrations
                .Include(a => a.Division)
                .ToList()
                .Where(d => words.All(d.AdministrationName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Position> BuildPositionSearchQuery (params string[] words)
        {
            IEnumerable<Position> query = db.Positions
                .ToList()
                .Where(d => words.All(d.PositionName.ToLower().Contains));

            return query;
        }
        private IEnumerable<Division> BuildDivisionSearchQuery (params string[] words)
        {
            IEnumerable<Division> query = db.Divisions
                .ToList()
                .Where(d => words.All(d.DivisionName.ToLower().Contains));

            return query;
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
        private List<Employee> AddIncludes(IQueryable<Employee> query)
        {
            List<Employee> employeeMatches = query
                .OrderBy(c => c.EmployeeFullName)
                .Include(p => p.Position)
                .Include(d => d.Department)
                .ToList();

            return employeeMatches;
        }

        /// <summary>
        /// Returns a list of the ids of deparments related to a particular administration/division
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns>list of ids</returns>
        private List<int> GetDepartmentIds(string type, int? id)
        {
            List<int> departmentIds = new List<int>();
            if (type == "administration")
            {
                departmentIds = db.Departments
                    .Where(d => d.AdministrationId == id)
                    .Select(i => i.DepartmentId)
                    .ToList();
            }
            else if (type == "division")
            {
                List<int> administrationIds = db.Administrations
                    .Where(a => a.DivisionId == id)
                    .Select(i => i.AdministrationId)
                    .ToList();
                departmentIds = db.Departments
                    .Where(d => administrationIds.Contains(d.AdministrationId))
                    .Select(i => i.DepartmentId)
                    .ToList();
            }

            return departmentIds;
        }
    }
}