using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ParrotPaymentsApp.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Введенные пароли не совпадают.")]
        public string RepeatedPassword { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Введенный адрес не является адресом электронной почты.")]
        public string Email { get; set; }
    }
}