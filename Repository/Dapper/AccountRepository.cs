using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using IRepository;
using Microsoft.AspNetCore.Identity;

namespace Repository.Dapper
{
    public class AccountRepository : GenericRepository<Account>, IAccountsRepository
    {
        public AccountRepository(IDbTransaction transaction)
            : base(transaction)
        {

        }

        public Task<bool> CheckPasswordAsync(Account account, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityError>> CreateAccountAsync(Account account, string password)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> FindAccountByAccountIdAsync(int AccountId)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> FindAccountByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> FindAccountByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Account>> FindByConditionAsync(Expression<Func<Account, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Account> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsForAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesForAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityError>> LockAccountAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityError>> UnLockAccountAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Account>.Create(Account entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Account>.Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}