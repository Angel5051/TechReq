using System.Threading.Tasks;
using System.Security.Claims;
using TeachRed.Domain.Models;
using TeachRed.Domain.Response;

namespace TeachRed.Service.Interfaces
{
    public interface IAccountService
    {
        // Метод регистрации (возвращает код-строку)
        Task<BaseResponse<string>> Register(Users model);

        // Метод входа
        Task<BaseResponse<ClaimsIdentity>> Login(Users model);

        // Метод подтверждения почты (ТОЛЬКО ОДИН, правильный)
        Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(Users model, string code, string confirmCode);

        Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(Users model);

    }
}