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
    public class BlockBruteForceRepository : GenericRepository<BlockBruteForce>, IBlockBruteForceRepository
    {
        public BlockBruteForceRepository(IDbTransaction transaction)
            : base(transaction) { }

        public Task CreateAsync(BlockBruteForce entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BlockBruteForce entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BlockBruteForce>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<BlockBruteForce>> FindByConditionAsync(Expression<Func<BlockBruteForce, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<BlockBruteForce> FindOneByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlockBruteForce?> GetBlockBruteForceByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BlockBruteForce entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<BlockBruteForce>.Create(BlockBruteForce entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepositoryEntityFramework<BlockBruteForce>.Update(BlockBruteForce entity)
        {
            throw new NotImplementedException();
        }
    }
}