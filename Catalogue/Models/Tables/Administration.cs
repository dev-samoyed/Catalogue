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
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я ]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string AdministrationName { get; set; }

        [Display(Name = "Почтовый индекс")]
        [Range(700000, 729999, ErrorMessage = "Почтовый индекс должен быть в диапазоне 700000 - 729999")]
        [Required(ErrorMessage = "Заполните поле!")]
        public int AdministrationPost { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string AdministrationAddress { get; set; }

        [Display(Name = "Факс")]

        [StringLength(12, ErrorMessage = "Длина строки не должна превышать 12 символов")]
        public string AdministrationFax { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string AdministrationEMail{ get; set; }

        [Display(Name = "Skype")]
        [StringLength(20, ErrorMessage = "Длина строки не должна превышать 20 символов")]
        public string AdministrationSkype { get; set; }

        [Display(Name = "Код")]
        [Range(0, 09999, ErrorMessage = "Код городского телефона должен быть в диапазоне 0 - 09999")]
        [Required(ErrorMessage = "Заполните поле!")]
        public int AdministrationCode { get; set; }

        public ICollection<Department> Departments{ get; set; }

        [Required(ErrorMessage = "Необходимо выбрать административное деление!")]
        public int DivisionId { get; set; }

        public Division Division { get; set; }      
    }
}