using System.ComponentModel.DataAnnotations;

namespace TeachRed.Domain.ViewModels.LoginAndRegistration
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
