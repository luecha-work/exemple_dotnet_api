using Entities;
using IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityFramework
{
    public class AccountRepository : GenericRepository<Account>, IAccountsRepository
    {
        private readonly ExempleApiDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountRepository(
            ExempleApiDbContext context,
            UserManager<Account> userManager,
            RoleManager<Role> roleManager
            ) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckPasswordAsync(Account account, string password)
        {
            var result = await _userManager.CheckPasswordAsync(account, password);

            return result;
        }

        public async Task<IEnumerable<IdentityError>> CreateAccountAsync(Account account, string password)
        {
            var result = await _userManager.CreateAsync(account, password);

            return result.Errors;
        }

        public async Task<Account?> FindAccountByAccountIdAsync(int AccountId)
        {
            var account = await _userManager.FindByIdAsync(AccountId.ToString());
            return account;
        }

        public async Task<Account?> FindAccountByEmailAsync(string email)
        {
            var account = await _userManager.FindByEmailAsync(email);
            return account;
        }

        public async Task<Account?> FindAccountByUsernameAsync(string username)
        {
            var account = await _userManager.FindByNameAsync(username);
            return account;
        }

        public async Task<IList<Claim>> GetClaimsForAccountAsync(Account account)
        {
            var claims = await _userManager.GetClaimsAsync(account);
            return claims;
        }

        public async Task<IList<string>> GetRolesForAccountAsync(Account account)
        {
            var roles = await _userManager.GetRolesAsync(account);
            return roles;
        }

        public async Task<IEnumerable<IdentityError>> LockAccountAsync(string email)
        {
            var account = await FindAccountByEmailAsync(email);

            if (account == null)
            {
                return new List<IdentityError> { new IdentityError { Description = "Account not found." } };
            }

            var result = await _userManager.SetLockoutEnabledAsync(account, true);

            return result.Succeeded ? Enumerable.Empty<IdentityError>() : result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> UnLockAccountAsync(string email)
        {
            var account = await FindAccountByEmailAsync(email);

            if (account == null)
            {
                return new List<IdentityError> { new IdentityError { Description = "Account not found." } };
            }

            var result = await _userManager.SetLockoutEnabledAsync(account, false);

            return result.Succeeded ? Enumerable.Empty<IdentityError>() : result.Errors;
        }
    }
}
