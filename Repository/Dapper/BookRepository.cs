using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;
using IRepository;

namespace Repository.Dapper
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
         public BookRepository(IDbTransaction transaction)
            : base(transaction) { }

        public Task CreateAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Book entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Book>> FindByConditionAsync(Expression<Func<Book, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Book> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Book>.Create(Book entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Book>.Update(Book entity)
        {
            throw new NotImplementedException();
        }
    }
}