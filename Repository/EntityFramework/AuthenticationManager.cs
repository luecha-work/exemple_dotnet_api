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
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<Account> _userManager;

        public AuthenticationManager(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GenerateUserTokenAsync(Account account, string loginProvider, string refreshTokenProvider)
        {
            var token = await _userManager.GenerateUserTokenAsync(
               account,
               loginProvider,
               refreshTokenProvider
           );

            return token;
        }

        public async Task RemoveAuthenticationTokenAsync(Account account, string loginProvider, string refreshTokenProvider)
        {
            await _userManager.RemoveAuthenticationTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider
            );
        }

        public async Task<IdentityResult> SetAuthenticationTokenAsync(Account account, string loginProvider, string refreshTokenProvider, string newRefreshToken)
        {
            var result = await _userManager.SetAuthenticationTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider,
                newRefreshToken
            );

            return result;
        }

        public Task<IdentityResult> UpdateSecurityStampAsync(Account account)
        {
            var result = _userManager.UpdateSecurityStampAsync(account);
            return result;
        }

        public async Task<bool> VerifyUserTokenAsync(Account account, string loginProvider, string refreshTokenProvider, string refreshToken)
        {
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(
              account,
              loginProvider,
              refreshTokenProvider,
              refreshToken
          );

            var authenticationreRreshToken = await _userManager.GetAuthenticationTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider
            );

            if (authenticationreRreshToken != refreshToken)
            {
                return false;
            }

            return isValidRefreshToken;
        }
    }
}
