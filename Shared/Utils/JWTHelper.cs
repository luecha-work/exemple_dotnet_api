using Microsoft.IdentityModel.Tokens;
using Shared.ConfigurationModels;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Shared.Utils
{
    public static class JWTHelper
    {
        public static string GetAccoutIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var accountIdClaim = jwtToken
                .Claims.FirstOrDefault(claim => claim.Type == "account_id")
                ?.Value;

            return accountIdClaim ?? "";
        }

        public static string GetSessionIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var accountIdClaim = jwtToken
                .Claims.FirstOrDefault(claim => claim.Type == "session_id")
                ?.Value;

            return accountIdClaim ?? "";
        }

        public static string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var accountIdClaim = jwtToken
                .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                ?.Value;

            return accountIdClaim ?? "";
        }

        public static List<int> GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleIdsClaims = jwtToken.Claims.Where(claim => claim.Type == ClaimTypes.Role);
            return roleIdsClaims.Select(claim => int.Parse(claim.Value)).ToList();
        }

        public static string GetJWTTokenClaim(string token, string claimName)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claimValue = jwtToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
            return claimValue ?? "";
        }

        public static bool VerifyToken(string token, JwtConfiguration _jwtConfiguration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey)
                ),
                ValidateIssuer = true,
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = _jwtConfiguration.ValidAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out validatedToken
                );
                return true;
            }
            catch (Exception ex)
            {
                // การตรวจสอบลายเซ็นต์ Token หรือการหมดอายุล้มเหลว
                return false;
            }
        }
    }
}
