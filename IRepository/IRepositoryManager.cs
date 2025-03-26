using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IRepositoryManager
    {
        IAccountsRepository AccountRepository { get; }
        IRoleRepository RoleRepository { get; }
        IAccountRolesRepository AccountRolesRepository { get; } 
        IAuthenticationManager AuthenticationManager { get; }
        IBlockBruteForceRepository BlockBruteForceRepository { get; }
        ISystemSessionRepository SystemSessionRepository { get; }

        

        void Commit();
    }
}
