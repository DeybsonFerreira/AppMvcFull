using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppMvcFull.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly AppMvcFullDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppMvcFullDbContext db)
        {
            _context = db;
            _dbSet = db.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await _dbSet.AsNoTracking().Where(expression).ToListAsync();
            return result;
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var result = await _dbSet.FindAsync(id);
            return result;
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            var model = await GetAsync(id);
            _dbSet.Remove(model);
            await SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() => _context?.Dispose();

    }
}
