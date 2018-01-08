using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Employees
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string EployeeRoom { get; set; }
        public string EmployeeAddress { get; set; }
        public int EmployeePhone { get; set; }
        public string EmployeePersonalPhone { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeSkype { get; set; }

        //public int? PositionId { get; set; }
        //public virtual Position Position { get; set; }

        //public int? DepartmentId { get; set; }
        //public virtual Department Department { get; set; }
    }
}