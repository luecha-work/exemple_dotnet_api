using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IBlockBruteForceRepository : IGenericRepositoryEntityFramework<BlockBruteForce>
    {
        Task<BlockBruteForce?> GetBlockBruteForceByIdAsync(Guid id);
    }
}
