using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace Catalogue.Models.Tables
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Кабинет")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string EmployeeRoom { get; set; }

        [Display(Name = "Адрес")]
        public string EmployeeAddress { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string EmployeePhone { get; set; }

        [Display(Name = "Мобильный телефон")]
        public string EmployeePersonalPhone { get; set; }

        [Display(Name = "E-mail")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string EmployeeEmail { get; set; }

        [Display(Name = "Skype")]
        public string EmployeeSkype { get; set; }

        [Display(Name = "Фото сотрудника")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg)$", ErrorMessage = "Формат файла должен быть .jpg, .png, .jpeg")]       
        public string EmployeePhoto { get; set; }

        /// <summary>
        /// relationship one to many
        /// </summary>
        /// 
        [Required(ErrorMessage = "Необходимо выбрать должность!")]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать отдел!")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Display(Name = "Дата принятия")]
        [Required(ErrorMessage = "Необходимо выбрать дату принятия сотрудника!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAdoption { get; set; }

        [Display(Name = "Дата увольнения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateDismissal { get; set; }

        [Display(Name = "Уволен")]
        public bool Dismissed { get; set; }
    }
}