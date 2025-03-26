using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.EntityFramework
{
    public class GenericRepository<T> : IGenericRepositoryEntityFramework<T>
        where T : class
    {
        private readonly ExempleApiDbContext _context;

        public GenericRepository(ExempleApiDbContext context)
        {
            _context = context;
        }

        public Task<List<T>> FindAllAsync() => _context.Set<T>().ToListAsync();
        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) => await Task.FromResult(_context.Set<T>().Where(expression));
        public async Task<T?> FindOneByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
        public void Create(T entity) => _context.Set<T>().Add(entity);
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity != null;
        }
    }
}
