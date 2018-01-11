using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Display(Name = "Наименование отдела")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string DepartmentName { get; set; }

       
        public ICollection<Employee> Employees { get; set; }

        [Required(ErrorMessage = "Заполните поле!")]
        public int? AdministrationId { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public Administration Administration { get; set; }
    }
}