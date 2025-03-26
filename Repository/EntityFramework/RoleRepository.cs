using Entities;
using IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityFramework
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(ExempleApiDbContext context, RoleManager<Role> roleManager)
            : base(context)
        {
            _roleManager = roleManager;
        }

        public async Task<Role?> FindByRoleNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }
    }
}
