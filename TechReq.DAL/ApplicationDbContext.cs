// ApplicationDbContext.cs (в папке DAL)

using Microsoft.EntityFrameworkCore;
using TechReq.Domain.ModelsDb; // ✅ Используем правильное пространство имен для моделей

namespace TechReq.DAL
{
    public class ApplicationDbContext : DbContext
    {
        // Конструктор необходим для DI
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet'ы для таблиц:
        public DbSet<UsersDb> Users { get; set; }
        public DbSet<RequestDb> Requests { get; set; }
        public DbSet<ServiceDb> Services { get; set; }
        public DbSet<RequestFileDb> RequestFiles { get; set; }
    }
}