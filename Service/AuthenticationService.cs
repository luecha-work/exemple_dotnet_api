using AutoMapper;
using Entities;
using IRepository;
using IService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ConfigurationModels;
using Shared.DTOs;
using Shared.Enum;
using Shared.Exceptions;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ISystemSessionService _systemSessionService;
        private readonly IBlockBruteForceService _blockForceService;
        private readonly IConfiguration _configuration;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IdentityProviderConfigure _configurationIdentityProvider;
        private readonly IMapper _mapper;

        private Account _user;
        private SystemSession _session;

        public AuthenticationService(
            IRepositoryManager repositoryManager,
            ISystemSessionService systemSessionService,
            IBlockBruteForceService blockforceService,
            IConfiguration configuration,
            IOptions<JwtConfiguration> configurationJwt,
            IOptions<IdentityProviderConfigure> configurationIdentityConfigure,
            IMapper mapper
        )
        {
            _repositoryManager = repositoryManager;
            _systemSessionService = systemSessionService;
            _blockForceService = blockforceService;
            _configuration = configuration;
            _jwtConfiguration = configurationJwt.Value;
            _configurationIdentityProvider = configurationIdentityConfigure.Value;
            _mapper = mapper;
        }

        public async Task<string> CreateRefreshTokenAsync()
        {
            await _repositoryManager.AuthenticationManager.RemoveAuthenticationTokenAsync(
                _user,
                _configurationIdentityProvider.LoginProvider,
                _configurationIdentityProvider.RefreshTokenProvider
            );

            var newRefreshToken = await _repositoryManager.AuthenticationManager.GenerateUserTokenAsync(
                _user,
                _configurationIdentityProvider.LoginProvider,
                _configurationIdentityProvider.RefreshTokenProvider
            );

            await _repositoryManager.AuthenticationManager.SetAuthenticationTokenAsync(
                _user,
                _configurationIdentityProvider.LoginProvider,
                _configurationIdentityProvider.RefreshTokenProvider,
                newRefreshToken
            );

            return newRefreshToken;
        }

        public async Task<UserProvider> GetUserProviderAsync(string accountId)
        {
            _user = await _repositoryManager.AccountRepository.FindAccountByAccountIdAsync(int.Parse(accountId))
                ?? throw new Exception($"Account with id {accountId} was not found.");


            List<string> roles = (
                await _repositoryManager.AccountRepository.GetRolesForAccountAsync(_user)
            ).ToList();

            return new UserProvider()
            {
                UserInfo = _user,
                RoleInfo = roles
            };
        }

        public async Task<AuthResponseDto> LoginLocalAsync(AuthenticationLocalDto loginLocalDto)
        {
            var account = await _repositoryManager.AccountRepository.FindAccountByEmailAsync(
                loginLocalDto.Email
            );

            if (account == null)
            {
                await _blockForceService.BlockBruteForceManagementAsync(loginLocalDto.Email);
                throw new LoginBadRequestException("Please check your username or password incorrect.");
            }

            _user = account;

            bool isValidUser = await _repositoryManager.AccountRepository.CheckPasswordAsync(
                _user,
                loginLocalDto.Password
            );

            if (_user == null || !isValidUser)
            {
                // TODO: Implement bruteforce management
                await _blockForceService.BlockBruteForceManagementAsync(loginLocalDto.Email);

                throw new LoginBadRequestException("Please check your username or password incorrect.");
            }

            var clientDetail = new BaseAuthenticationDto()
            {
                Os = loginLocalDto.Os,
                Browser = loginLocalDto.Browser,
                PlatForm = loginLocalDto.PlatForm
            };
            // TODO: Implement session management
            _session = await _systemSessionService.CreateSystemSessionAsync(
                _user,
                clientDetail
            );

            var token = await GenerateTokenAsync();

            _session.Token = token;
            _session.UpdatedAt = DateTime.UtcNow;

            //TDOD: Update session
            await _repositoryManager.SystemSessionRepository.UpdateAsync(_session);

            return new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = await CreateRefreshTokenAsync()
            };
        }

        public async Task<AccountDto> SingUpAsync(SingUpDto singUpDto)
        {
            //TODO:Valisdate User to create
            var isAdEmailDuplicate =
                await _repositoryManager.AccountRepository.FindAccountByEmailAsync(singUpDto.Email);

            var isAdUsernameDuplicate =
                await _repositoryManager.AccountRepository.FindAccountByUsernameAsync(
                    singUpDto.UserName
                );

            if (isAdEmailDuplicate != null)
                throw new SingUpEmailDuplicateBadRequestException(singUpDto.Email);

            if (isAdUsernameDuplicate != null)
                throw new SingUpWithUsernameDuplicateBadRequestException(singUpDto.Email);

            var AccountForCreate = _mapper.Map<Account>(singUpDto);
            AccountForCreate.CreatedBy = "system";
            AccountForCreate.CreatedAt = DateTime.UtcNow;

            var errors = await _repositoryManager.AccountRepository.CreateAccountAsync(
                AccountForCreate,
                singUpDto.Password
            );

            if (!errors.Any())
            {


                var role = await _repositoryManager.RoleRepository.FindByRoleNameAsync("User");

                var AccountsRole = new AccountRoles()
                {
                    UserId = AccountForCreate.Id,
                    RoleId = role.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
                };

                await _repositoryManager.AccountRolesRepository.CreateAsync(AccountsRole);
            }

            var accountResult = _mapper.Map<AccountDto>(AccountForCreate);

            return accountResult;
        }

        public bool VerifyAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            return JWTHelper.VerifyToken(accessToken, _jwtConfiguration);
        }

        public Task<bool> VerifyAccessTokenAsync(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto?> VerifyRefreshTokenAsync(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.AccessToken);
            var username = tokenContent
               .Claims.ToList()
               .Find(q => q.Type == JwtRegisteredClaimNames.Sub)
               ?.Value;

            var sessionId = tokenContent.Claims.FirstOrDefault(q => q.Type == "session_id")?.Value;

            if (sessionId == null)
            {
                throw new ArgumentNullException(nameof(sessionId), "Session ID cannot be null.");
            }
            Guid guidSessionId = Guid.Parse(sessionId);

            int requestUserId = int.Parse(
               tokenContent.Claims.FirstOrDefault(q => q.Type == "account_id")?.Value ?? throw new ArgumentNullException("account_id")
            );

            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null.");
            }

            _user = await _repositoryManager.AccountRepository.FindAccountByUsernameAsync(username)
                ?? throw new InvalidOperationException($"User with username {username} was not found.");

            _session = await _repositoryManager.SystemSessionRepository.GetBySessionIdAsync(guidSessionId);

            if (_user == null || _user.Id != requestUserId || _session == null)
                return null;

            //TODO: Validate Last Time Refresh Token
            var refreshTokenTimeAgain =
               _session.RefreshTokenAt != null
                   ? _session.RefreshTokenAt.Value.AddMinutes(5)
                   : (DateTime?)null;

            if (
               _session.RefreshTokenAt != null
               && _session.RefreshTokenAt < refreshTokenTimeAgain
            )
                throw new RefreshTokensTooOftenException();

            if (_session.SessionStatus == EnumHelper.GetEnumValue(SessionStatusEnum.Expired))
                throw new RefreshTokenExpirationTimeException();

            if (_session.SessionStatus == EnumHelper.GetEnumValue(SessionStatusEnum.Blocked))
                throw new BlockedRefreshTokenExpirationException();

            var isValidRefreshToken = await _repositoryManager.AuthenticationManager.VerifyUserTokenAsync(
               _user,
               _configurationIdentityProvider.LoginProvider,
               _configurationIdentityProvider.RefreshTokenProvider,
               request.RefreshToken
            );

            if (isValidRefreshToken)
            {
                var token = await GenerateTokenAsync();

                _session.Token = token;
                _session.UpdatedAt = DateTime.UtcNow;
                _session.RefreshTokenAt = DateTime.UtcNow;

                _repositoryManager.SystemSessionRepository.Update(_session);


                return new AuthResponseDto
                {
                    AccessToken = token,
                    RefreshToken = await CreateRefreshTokenAsync()
                };
            }

            await _repositoryManager.AuthenticationManager.UpdateSecurityStampAsync(_user);

            await _systemSessionService.BlockSystemSessionAsync(_session);

            throw new InvalidRefreshTokenException();
        }

        private async Task<string> GenerateTokenAsync()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey) //TODO: Get Key from JwtSettings in  applications.json
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (_user == null)
            {
                throw new InvalidOperationException("User cannot be null when retrieving roles.");
            }
            var roles = await _repositoryManager.AccountRepository.GetRolesForAccountAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            // var userClaims = await _repositoryManager.AccountRepository.GetClaimsForAccount(_user);

            if (_user == null)
            {
                throw new InvalidOperationException("User cannot be null when generating token.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email ?? string.Empty),
                new Claim("session_id", _session?.Id.ToString() ?? string.Empty),
                new Claim("account_id", _user.Id.ToString()),
            }
            // .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToInt32(_jwtConfiguration.DurationInMinutes)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
