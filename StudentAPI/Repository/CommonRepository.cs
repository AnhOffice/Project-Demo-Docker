using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;

namespace StudentAPI.Repository
{
    public class CommonRepository<T, TKey> : ICommonRepository<T, TKey> where T : class
    {
        private readonly StudentDBContext _context;
        private readonly DbSet<T> _dbSet;

        public CommonRepository(StudentDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAllCanLinQ() => _dbSet;

        public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();

        public async Task<T> GetById(TKey id) => await _dbSet.FindAsync(id);
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TKey id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
