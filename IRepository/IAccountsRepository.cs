using Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IAccountsRepository : IGenericRepositoryEntityFramework<Account>
    {
        Task<Account?> FindAccountByEmailAsync(string email);
        Task<Account?> FindAccountByAccountIdAsync(int AccountId);
        Task<Account?> FindAccountByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(Account account, string password);
        Task<IList<string>> GetRolesForAccountAsync(Account account);
        Task<IList<Claim>> GetClaimsForAccountAsync(Account account);
        Task<IEnumerable<IdentityError>> CreateAccountAsync(Account account, string password);
        Task<IEnumerable<IdentityError>> LockAccountAsync(string email);
        Task<IEnumerable<IdentityError>> UnLockAccountAsync(string email);
    }
}
