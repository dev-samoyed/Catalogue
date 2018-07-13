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
        [StringLength(255, ErrorMessage = "Длина строки не должна превышать 255 символов")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я , -]+$", ErrorMessage = "Ввод цифр запрещен")]
        [Display(Name = "Наименование отдела")]
        public string DepartmentName { get; set; }

       
        public ICollection<Employee> Employees { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать орган управления!")]
        public int AdministrationId { get; set; }

        public Administration Administration { get; set; }

        [Display(Name = "Почтовый индекс")]
        [Range(700000, 729999, ErrorMessage = "Почтовый индекс должен быть в диапазоне 700000 - 729999")]
        public int? DepartmentPost { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string DepartmentAddress { get; set; }

        [Display(Name = "Факс")]
        [StringLength(12, ErrorMessage = "Длина строки не должна превышать 12 символов")]
        public string DepartmentFax { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string DepartmentEMail { get; set; }

        [Display(Name = "Skype")]
        [StringLength(20, ErrorMessage = "Длина строки не должна превышать 20 символов")]
        public string DepartmentSkype { get; set; }

        [Display(Name = "Код")]
        [Range(0, 09999, ErrorMessage = "Код городского телефона должен быть в диапазоне 0 - 09999")]
        public int? DepartmentCode { get; set; }
    }
}