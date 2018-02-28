using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Catalogue.Models.Tables;
using PagedList.Mvc;
using PagedList;
using System.Net;

using Catalogue.Controllers.Utils;

namespace Catalogue.Controllers
{
    public class SearchController : Controller
    {
        CatalogueContext db = new CatalogueContext();

        // Builds the ajax employee search query with pagination
        [HttpPost]
        public ActionResult EmployeeFilter(string name, int? page, int? positionId, int? departmentId, int? administrationId, int? divisionId)
        {
            IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            name = name.Trim();
            if (name.Length <= 0)
                employees = db.Employees.OrderBy(c => c.EmployeeFullName);
            else
                employees = BuildEmployeeSearchQueryByName(name);

            employees = FilterAdditions(employees, positionId, departmentId, administrationId, divisionId);

            employees = AddIncludes(employees);

            string view = "";
            if (User.IsInRole("admin"))
                view = "~/Views/Search/AdminEmployeeFilter.cshtml";
            else if (User.IsInRole("manager"))
                view = "~/Views/Search/ManagerEmployeeFilter.cshtml";
            else
                view = "~/Views/Search/EmployeeFilter.cshtml";

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return PartialView(view, employees.ToPagedList(pageNumber, pageSize));
        }

        // Forms not found partial view
        public ActionResult NotFoundResult ()
        {
            return PartialView("~/Views/Error/NotFoundError.cshtml");
        }

        // Forms a partial view with a list of found entities
        [Authorize(Roles = "admin")]
        public ActionResult AdminSearch(string title, string type)
        {
            string view = "~/Views/Search/";
            string[] words = title.ToLower().Split(' ');

            // returns not found if input string is empty
            if (title.Trim().Length <= 0)
                return RedirectToAction("NotFoundResult");

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

        [Authorize(Roles = "manager")]
        public ActionResult EmployeeDetails (int? id)
        {
            if (id == null)
                return HttpNotFound();


            Employee employee = db.Employees.Include(p => p.Position).Include(d => d.Department).Include(e => e.Department.Administration).SingleOrDefault(e => e.EmployeeId == id);

            return View(employee);
        }

        // Builds the main employee search query
        private IQueryable<Employee> BuildEmployeeSearchQueryByName(string name)
        {
            IQueryable<Employee> employees = Enumerable.Empty<Employee>().AsQueryable();

            int maxNumberOfWordsInFullName = 3;
            Stack<string> words = new Stack<string>();


            string[] inputWords = name.Split(' ');

            int wordsAmount = inputWords.Length < maxNumberOfWordsInFullName ? inputWords.Length : maxNumberOfWordsInFullName;
            for (int i = 0; i < wordsAmount; i++)
                words.Push(inputWords[i]);

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

            return employees;
        }

        // Adds filters to the search query
        private IQueryable<Employee> FilterAdditions(IQueryable<Employee> query, int? positionId, int? departmentId, int? administrationId, int? divisionId)
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

        // Binds entity search results and entity view
        private void BindSearchResults<T> (List<T> items, ref string view, string entityView)
        {
            if (items.Count <= 0)
            {
                view += "NotFound.cshtml";
            }
            else
            {
                ViewBag.Items = items;
                view += entityView;
            }
        }

        // Builds a search query that matches all words of the array 'words'
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

        // Forms a search query for employees by one param
        private IQueryable<Employee> BuildSearchQuery(string part_1)
        {
            IQueryable<Employee> query = db.Employees
                .Where(n => n.EmployeeFullName.Contains(part_1));

            return query;
        }

        // Forms a search query for employees by two params
        private IQueryable<Employee> BuildSearchQuery(string part_1, string part_2)
        {
            IQueryable<Employee> query = db.Employees
                .Where(e => e.EmployeeFullName.Contains(part_1)
                        && e.EmployeeFullName.Contains(part_2));

            return query;
        }

        // Forms a search query for employees by three params
        private IQueryable<Employee> BuildSearchQuery(string part_1, string part_2, string part_3)
        {
            IQueryable<Employee> query = db.Employees
                .Where(e => e.EmployeeFullName.Contains(part_1)
                        && e.EmployeeFullName.Contains(part_2)
                        && e.EmployeeFullName.Contains(part_3));

            return query;
        }

        // Adds relationships Position and Department to the (Employee) query
        private IQueryable<Employee> AddIncludes(IQueryable<Employee> query)
        {
            IQueryable<Employee> employeeMatches = query
                .OrderBy(c => c.EmployeeFullName)
                .Include(d => d.Department)
                .Include(e => e.Department.Administration)
                .Include(p => p.Position);

            return employeeMatches;
        }

        // Returns a list of the ids of deparments related to a particular administration/division
        private List<int> GetDepartmentIds(string type, int? id)
        {
            List<int> departmentIds = new List<int>();

            if (type == "administration")
                departmentIds = GetDepartmentIdsByAdministration(id);
            else if (type == "division")
                departmentIds = GetDepartmentIdsByDivision(id);

            return departmentIds;
        }

        // Returns a list of the ids of departments related to a administration
        private List<int> GetDepartmentIdsByAdministration (int? administrationId)
        {
            return db.Departments
                .Where(d => d.AdministrationId == administrationId)
                .Select(i => i.DepartmentId)
                .ToList();
        }

        // Returns a list of the ids of departments related to a division
        private List<int> GetDepartmentIdsByDivision (int? divisionId)
        {
            List<int> administrationIds = db.Administrations
                .Where(d => d.DivisionId == divisionId)
                .Select(i => i.AdministrationId)
                .ToList();

            return db.Departments
                .Where(d => administrationIds.Contains(d.AdministrationId))
                .Select(i => i.DepartmentId)
                .ToList();
        }
    }
}