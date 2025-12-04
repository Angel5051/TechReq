using System.ComponentModel.DataAnnotations;

namespace TeachRed.Domain.ViewModels.LoginAndRegistration
{
    public class LoginViewModel
    {
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
