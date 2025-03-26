using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace IRepository
{
    public interface IGenericRepositoryEntityFramework<T>
        where T : class
    {
        //Task<PagedList<T>> FindPagedResultAsync(
        //    BaseQueryParameters queryParameters,
        //    Expression<Func<T, bool>> filterCondition = null
        //);
        void Create(T entity);
        Task CreateAsync(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(int id);
        Task<List<T>> FindAllAsync();
        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> FindOneByIdAsync(int id);
        void Update(T entity);
        Task UpdateAsync(T entity);
    }
}
