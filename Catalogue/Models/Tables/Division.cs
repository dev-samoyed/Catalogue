using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Division
    {
        public int DivisionId { get; set; }

        [Display(Name = "Административное деление")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я -]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string DivisionName { get; set; }

        public ICollection<Administration> Administrations { get; set; }
    }
}