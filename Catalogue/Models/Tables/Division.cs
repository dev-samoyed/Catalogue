using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Division
    {
        [Required(ErrorMessage = "Выберите деление")]
        public int DivisionId { get; set; }

        [Display(Name = "Административное деление")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string DivisionName { get; set; }

        public ICollection<Administration> Administrations { get; set; }
    }
}