using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityFramework
{
    public class BlockBruteForceRepository : GenericRepository<BlockBruteForce>,
            IBlockBruteForceRepository
    {
        private readonly ExempleApiDbContext _context;

        public BlockBruteForceRepository(ExempleApiDbContext context)
           : base(context)
        {
            _context = context;
        }

        public async Task<BlockBruteForce?> GetBlockBruteForceByIdAsync(Guid id)
        {
            return await _context.BlockBruteForces.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
