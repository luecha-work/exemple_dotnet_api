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
    public class BookLoanRepository : GenericRepository<BookLoan>, IBookLoanRepository
    {
        public BookLoanRepository(IDbTransaction transaction)
            : base(transaction) { }

        public Task CreateAsync(BookLoan entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BookLoan entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BookLoan>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<BookLoan>> FindByConditionAsync(Expression<Func<BookLoan, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<BookLoan> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BookLoan entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<BookLoan>.Create(BookLoan entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<BookLoan>.Update(BookLoan entity)
        {
            throw new NotImplementedException();
        }
    }
}