using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachRed.Domain.Models;

namespace TeachRed.Domain
{
    public class UsersValidators : AbstractValidator<Users>
    {
        public UsersValidators()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Логин обязателен для заполнения.")
                .MinimumLength(3).WithMessage("Логин должен содержать минимум 3 символа.")
                .MaximumLength(35).WithMessage("Логин не должен превышать 35 символов.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.")
                .MaximumLength(35).WithMessage("Пароль не должен превышать 35 символов.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен для заполнения.")
                .EmailAddress().WithMessage("Требуется указать корректный адрес электронной почты.")
                .MaximumLength(35).WithMessage("Email не должен превышать 35 символов.");
        }
    }
}
