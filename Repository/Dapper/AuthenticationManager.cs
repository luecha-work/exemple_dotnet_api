using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using IRepository;
using Microsoft.AspNetCore.Identity;

namespace Repository.Dapper
{
    public class AuthenticationManager : IAuthenticationManager
    {
        public AuthenticationManager(IDbTransaction transaction) { }
        public Task<string> GenerateUserTokenAsync(Account account, string loginProvider, string refreshTokenProvider)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAuthenticationTokenAsync(Account account, string loginProvider, string refreshTokenProvider)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SetAuthenticationTokenAsync(Account account, string loginProvider, string refreshTokenProvider, string newRefreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateSecurityStampAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyUserTokenAsync(Account account, string loginProvider, string refreshTokenProvider, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}