using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachRed.Domain.ViewModels.LoginAndRegistration
{
    public class ConfirmEmailViewModel
    {
        [Required(ErrorMessage = "Введите код")]
        public string CodeConfirm { get; set; }
        public string GeneratedCode { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Code { get; set; }
        public string ConfirmCode { get; set; }
    }
}
