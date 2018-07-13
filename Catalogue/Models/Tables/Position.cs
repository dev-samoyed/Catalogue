using Catalogue.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Models.Tables
{
    public class Position
    {
        public int PositionId { get; set; }

        [Display(Name = "Наименование должности")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я - / ()]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string PositionName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}