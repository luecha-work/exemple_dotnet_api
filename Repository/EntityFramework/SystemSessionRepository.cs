using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityFramework
{
    public class SystemSessionRepository : GenericRepository<SystemSession>, ISystemSessionRepository
    {
        private readonly ExempleApiDbContext _context;

        public SystemSessionRepository(ExempleApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SystemSession> GetBySessionIdAsync(Guid sessionId)
        {
            return await _context.SystemSessions.FirstOrDefaultAsync(x => x.Id == sessionId);
        }
    }
}
