using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechReq.DAL.Interfaces;
using TechReq.Domain.ModelsDb;

namespace TechReq.DAL.Storage
{
    public class ServiceStorage : IBaseStorage<ServiceDb>
    {
        private readonly ApplicationDbContext _db;

        public ServiceStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(ServiceDb item)
        {
            await _db.Services.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(ServiceDb item)
        {
            _db.Services.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<ServiceDb> Get(Guid id)
        {
            return await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<ServiceDb> GetAll()
        {
            return _db.Services;
        }

        public async Task<ServiceDb> Update(ServiceDb item)
        {
            _db.Services.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}