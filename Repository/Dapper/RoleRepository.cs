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
    public class RoleRepository: GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbTransaction transaction)
            : base(transaction) { }

        public Task CreateAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Role>> FindByConditionAsync(Expression<Func<Role, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> FindByRoleNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Role>.Create(Role entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<Role>.Update(Role entity)
        {
            throw new NotImplementedException();
        }
    }
    
}