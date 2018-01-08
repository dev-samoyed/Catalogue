using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        


        public ICollection<Employee> Employees { get; set; }

        public int? AdministrationId { get; set; }
        public Administration Administration { get; set; }
    }
}