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
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я ]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string EmployeeFullName { get; set; }

        [Display(Name = "Кабинет")]
        [StringLength(10, ErrorMessage = "Длина строки не должна превышать 10 символов")]
        //[Required(ErrorMessage = "Заполните поле!")]
        public string EmployeeRoom { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string EmployeeAddress { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string EmployeePhone { get; set; }

        [Display(Name = "Мобильный телефон")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string EmployeePersonalPhone { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string EmployeeEmail { get; set; }

        [Display(Name = "Skype")]
        [StringLength(30, ErrorMessage = "Длина строки не должна превышать 30 символов")]
        public string EmployeeSkype { get; set; }

        [Display(Name = "Фото сотрудника")]

        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.jpeg|.PNG|.JPG|.JPEG)$", ErrorMessage = "Формат файла должен быть .jpg, .png, .jpeg")]       

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
        //[Required(ErrorMessage = "Необходимо выбрать дату принятия сотрудника!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateAdoption { get; set; }

        [Display(Name = "Дата увольнения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateDismissal { get; set; }
    }
}