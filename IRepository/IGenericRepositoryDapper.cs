using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IGenericRepositoryDapper<T>
    {
        T FindOneById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        int Create(T entity);
        int CreateWithOutputId(T entity);
        int Update(T entity);
        int Delete(int id);
        int ExecuteSqlCommand(string sql, object parameters = null);
        IEnumerable<T> QuerySqlCommand(string sql, object parameters = null);
    }
}