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
            name = name.Trim();

            if (name.Length <= 0)
                return RedirectToAction("NotFoundResult");

            int maxNumberOfWordsInFullName = 3;
            Stack<string> words = new Stack<string>();
            IQueryable<Employee> searchQuery = Enumerable.Empty<Employee>().AsQueryable();

            string[] inputWords = name.Split(' ');

            int wordsAmount = inputWords.Length < maxNumberOfWordsInFullName ? inputWords.Length : maxNumberOfWordsInFullName;
            for (int i = 0; i < wordsAmount; i++)
                words.Push(inputWords[i]);

            if (words.Count <= 0)
                return RedirectToAction("NotFoundResult");

            if (words.Count == 1)
            {
                string part_1 = words.Pop();
                searchQuery = BuildSearchQuery(part_1);
            }
            else if (words.Count == 2)
            {
                string part_1 = words.Pop(), part_2 = words.Pop();
                searchQuery = BuildSearchQuery(part_1, part_2);
            }
            else if (words.Count == 3)
            {
                string part_1 = words.Pop(), part_2 = words.Pop(), part_3 = words.Pop();
                searchQuery = BuildSearchQuery(part_1, part_2, part_3);
            }
            
            // search query with filters
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

            List<Employee> employeeMatches = AddIncludes(searchQuery);

            if (employeeMatches.Count <= 0)
                return RedirectToAction("NotFoundResult");

            string view = "";
            if (User.IsInRole("admin"))
            {
                view = "~/Views/Search/AdminEmployeeSearch.cshtml";
            }
            else
            {
                view = "~/Views/Search/EmployeeSearch.cshtml";
            }

            return PartialView(view, employeeMatches);
        }

        /// <summary>
        /// Forms not found partial view
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFoundResult ()
        {
            ViewBag.Error = Errors.notFound;
            return PartialView("~/Views/Home/Error.cshtml");
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

            // returns not found if input string is empty
            if (title.Trim().Length <= 0)
            {
                ViewBag.Error = Errors.notFound;
                return PartialView("~/Views/Search/Error.cshtml");
            }

            if (type == "department")
            {
                List<Department> departments = BuildDepartmentSearchQuery(words).ToList();
                BindSearchResults(departments, ref view, "Departments.cshtml");
            }
            else if (type == "administration")
            {
                List<Administration> administrations = BuildAdministartionSearchQuery(words).ToList();
                BindSearchResults(administrations, ref view, "Administrations.cshtml");
            }
            else if (type == "position")
            {
                List<Position> positions = BuildPositionSearchQuery(words).ToList();
                BindSearchResults(positions, ref view, "Positions.cshtml");
            }
            else if (type == "division")
            {
                List<Division> divisions = BuildDivisionSearchQuery(words).ToList();
                BindSearchResults(divisions, ref view, "Divisions.cshtml");
            }

            return PartialView(view);
        }
        
        [HttpPost]
        public ActionResult EmployeeFilter (string name, int? page, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            List<Position> positions = db.Positions.ToList();
            ViewBag.Positions = positions;

            List<Department> departments = db.Departments.ToList();
            ViewBag.Departments = departments;

            List<Administration> admins = db.Administrations.ToList();
            ViewBag.Admins = admins;

            List<Division> divisions = db.Divisions.ToList();
            ViewBag.Divisions = divisions;

            name = name.Trim();
            if (name.Length <= 0)
            {
                employees = db.Employees;
                employees = FilterAdditions(employees, positionId, departmentId, administrationId, divisionId);
            }
            else
            {
                int maxNumberOfWordsInFullName = 3;
                Stack<string> words = new Stack<string>();

                string[] inputWords = name.Split(' ');

                int wordsAmount = inputWords.Length < maxNumberOfWordsInFullName ? inputWords.Length : maxNumberOfWordsInFullName;
                for (int i = 0; i < wordsAmount; i++)
                    words.Push(inputWords[i]);

                if (words.Count <= 0)
                    return RedirectToAction("NotFoundResult");

                if (words.Count == 1)
                {
                    string part_1 = words.Pop();
                    employees = BuildSearchQuery(part_1);
                }
                else if (words.Count == 2)
                {
                    string part_1 = words.Pop(), part_2 = words.Pop();
                    employees = BuildSearchQuery(part_1, part_2);
                }
                else if (words.Count == 3)
                {
                    string part_1 = words.Pop(), part_2 = words.Pop(), part_3 = words.Pop();
                    employees = BuildSearchQuery(part_1, part_2, part_3);
                }

                employees = FilterAdditions(employees, positionId, departmentId, administrationId, divisionId);
            }

            List<Employee> employeeMatches = AddIncludes(employees);

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return PartialView(employeeMatches.ToPagedList(pageNumber, pageSize));
        }

        private IQueryable<Employee> FilterAdditions (IQueryable<Employee> query, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            if (positionId != null)
                query = query.Where(e => e.PositionId == positionId);

            if (departmentId != null)
                query = query.Where(e => e.DepartmentId == departmentId);

            if (administrationId != null)
            {
                List<int> departmentIds = GetDepartmentIds("administration", administrationId);
                query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            }

            if (divisionId != null)
            {
                List<int> departmentIds = GetDepartmentIds("division", divisionId);
                query = query.Where(e => departmentIds.Contains(e.DepartmentId));
            }

            return query;
        }

        public ActionResult EmployeeAjaxPagination (int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return PartialView(db.Employees.Include(e => e.Department).Include(p => p.Position).OrderBy(i => i.EmployeeFullName).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Binds entity search results and entity view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="view"></param>
        /// <param name="entityView"></param>
        private void BindSearchResults<T> (List<T> items, ref string view, string entityView)
        {
            if (items.Count <= 0)
            {
                ViewBag.Error = Errors.notFound;
                view += "Error.cshtml";
            }
            else
            {
                ViewBag.Items = items;
                view += entityView;
            }
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