using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Administration
    {
        public int AdministrationId { get; set; }

        [Display(Name = "Орган управления")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string AdministrationName { get; set; }

        [Display(Name = "Почтовый индекс")]
        [Required(ErrorMessage = "Заполните поле!")]
        public int AdministrationPost { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string AdministrationAddress { get; set; }

        [Display(Name = "Факс")]
        public string AdministrationFax { get; set; }

        [Display(Name = "E-mail")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string AdministrationEMail{ get; set; }

        [Display(Name = "Skype")]
        public string AdministrationSkype { get; set; }

        [Display(Name = "Код")]
        [Required(ErrorMessage = "Заполните поле!")]
        public int AdministrationCode { get; set; }

        public ICollection<Department> Departments{ get; set; }

        [Required(ErrorMessage = "Необходимо выбрать административное деление!")]
        public int DivisionId { get; set; }

        public Division Division { get; set; }      
    }
}