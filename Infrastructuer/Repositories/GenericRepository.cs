using Application.Repositories;
using Infrastructuer.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructuer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public void Delete(T input)
        {
            _dbSet.Remove(input);
        }

        public IQueryable<T> GetAll()
        {
            var data = _dbSet.AsQueryable();
            return data;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var data = await _dbSet.FindAsync(id);
            return data;
        }

        public async Task InsertAsync(T input)
        {
            await _dbSet.AddAsync(input);
        }

        public async Task InsertRange(List<T> input)
        {
            await _dbSet.AddRangeAsync(input);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T input)
        {
            _dbSet.Update(input);
        }
    }
}
