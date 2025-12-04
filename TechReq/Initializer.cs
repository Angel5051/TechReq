// Если Parts лежит в Domain.Models, добавьте:
using TeachRed.Service.Implementations;
using TeachRed.Service.Interfaces;
using TechReq.DAL.Interfaces;
using TechReq.DAL.Storage;
using TechReq.Domain.ModelsDb;

namespace TechReq
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UsersDb>, UserStorage>();

    }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}