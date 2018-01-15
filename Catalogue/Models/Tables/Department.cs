using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catalogue.Models.Tables
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Заполните поле!")]
       // [StringLength(5, ErrorMessage = "Длина строки превышает 50 символов")]
        [Display(Name = "Наименование отдела")]
        public string DepartmentName { get; set; }

       
        public ICollection<Employee> Employees { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать орган управления!")]
        public int AdministrationId { get; set; }

        public Administration Administration { get; set; }
    }
}