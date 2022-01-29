using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace exam_10.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login")]
        [Display(Name = "Логин")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}
