using Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IAuthenticationManager
    {
        Task RemoveAuthenticationTokenAsync(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        );
        Task<IdentityResult> SetAuthenticationTokenAsync(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string newRefreshToken
        );
        Task<string> GenerateUserTokenAsync(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        );
        Task<bool> VerifyUserTokenAsync(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string refreshToken
        );
        Task<IdentityResult> UpdateSecurityStampAsync(Account account);
    }
}
