using Microsoft.Extensions.DependencyInjection; // Не забудьте этот using для IServiceCollection
using TeachRed.Service.Implementations;
using TeachRed.Service.Interfaces;
using TeachRed.Service.Realizations;
using TechReq.DAL.Interfaces;
using TechReq.DAL.Storage;
using TechReq.Domain.ModelsDb;

namespace TechReq
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            // Регистрация хранилища пользователей
            services.AddScoped<IBaseStorage<UsersDb>, UserStorage>();

            // ✅ ВАЖНО: Добавьте эту строку! Без неё возникает ошибка.
            services.AddScoped<IBaseStorage<ServiceDb>, ServiceStorage>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            // Регистрация сервисов
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IServiceService, ServiceService>();

            // Настройки MVC (если они еще не добавлены в Program.cs, можно оставить здесь или там)
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}