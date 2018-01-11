using Catalogue.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Catalogue.Models
{
    public class Position
    {
        public int PositionId { get; set; }

        [Display(Name = "Наименование должности")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string PositionName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}