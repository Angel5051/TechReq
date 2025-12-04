using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeachRed.Domain.Models;
using TechReq.DAL.Interfaces;
using TechReq.Domain.ModelsDb;


namespace TechReq.DAL.Storage
{
    // Класс должен быть публичным, чтобы его можно было использовать вне сборки
    public class UserStorage : IBaseStorage<UsersDb>
    {
        private readonly ApplicationDbContext _db;

        public UserStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        // --- Реализация IBaseStorage<UsersDb> ---

        public async Task<UsersDb> Get(Guid id)
        {
            // Получает сущность по Id. Предполагается, что Users — это DbSet<UserDb> в ApplicationDbContext
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<UsersDb> GetAll()
        {
            // Возвращает IQueryable для дальнейшей фильтрации/сортировки на уровне БД
            return _db.Users;
        }

        public async Task Add(UsersDb item)
        {
            await _db.Users.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task<UsersDb> Update(UsersDb item)
        {
            _db.Users.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task Delete(UsersDb item)
        {
            _db.Users.Remove(item);
            await _db.SaveChangesAsync();
        }

       
    }
}