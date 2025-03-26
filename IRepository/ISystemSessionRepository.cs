using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IRepository
{
    public interface ISystemSessionRepository: IGenericRepositoryEntityFramework<SystemSession>
    {
        Task<SystemSession> GetBySessionIdAsync(Guid sessionId);
    }
}
