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
    public class AccountRolesRepository : GenericRepository<AccountRoles>, IAccountRolesRepository
    {
        public AccountRolesRepository(IDbTransaction transaction)
            : base(transaction)
        {

        }

        public Task CreateAsync(AccountRoles entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AccountRoles entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountRoles>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<AccountRoles>> FindByConditionAsync(Expression<Func<AccountRoles, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<AccountRoles> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AccountRoles entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<AccountRoles>.Create(AccountRoles entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<AccountRoles>.Update(AccountRoles entity)
        {
            throw new NotImplementedException();
        }
    }
}