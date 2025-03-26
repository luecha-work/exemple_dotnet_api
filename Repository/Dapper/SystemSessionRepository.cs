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
    public class SystemSessionRepository
        : GenericRepository<SystemSession>, ISystemSessionRepository
    {
        public SystemSessionRepository(IDbTransaction transaction)
            : base(transaction) { }

        public Task CreateAsync(SystemSession entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(SystemSession entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SystemSession>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<SystemSession>> FindByConditionAsync(Expression<Func<SystemSession, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<SystemSession> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SystemSession> GetBySessionIdAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SystemSession entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<SystemSession>.Create(SystemSession entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<SystemSession>.Update(SystemSession entity)
        {
            throw new NotImplementedException();
        }
    }
}