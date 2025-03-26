using Entities;
using IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityFramework
{
    public class AccountRolesRepository : GenericRepository<AccountRoles>, IAccountRolesRepository
    {
        public AccountRolesRepository(ExempleApiDbContext context)
            : base(context) { }
    }
}
