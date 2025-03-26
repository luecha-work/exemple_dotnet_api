using AutoMapper;
using IRepository;
using IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repository.EntityFramework;
using Shared.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IBlockBruteForceService> _blockBruteForceService;
        private readonly Lazy<ISystemSessionService> _systemSessionService;

        public ServiceManager(
          IRepositoryFactory repositoryFactory,
          IMapper mapper,
          IHttpContextAccessor httpContextAccessor,
          IConfiguration configuration,
          IOptions<JwtConfiguration> configurationJwt,
          IOptions<IdentityProviderConfigure> configurationIdentityConfigure
      )
        {

            _httpContextAccessor = httpContextAccessor;
            // var _repositoryDPManager = repositoryFactory.Create(RepoType.Dapper);
            var _repositoryEFManager = repositoryFactory.Create(RepoType.EntityFramework);

            _blockBruteForceService = new Lazy<IBlockBruteForceService>(
                () => new BlockBruteForceService(_repositoryEFManager)
            );
            _systemSessionService = new Lazy<ISystemSessionService>(
                () => new SystemSessionService(_repositoryEFManager, httpContextAccessor)
            );
            _authenticationService = new Lazy<IAuthenticationService>(
                () => new AuthenticationService(_repositoryEFManager, SystemSessionService, BlockBruteForceService, configuration, configurationJwt, configurationIdentityConfigure, mapper)
            );
        }

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IBlockBruteForceService BlockBruteForceService => _blockBruteForceService.Value;
        public ISystemSessionService SystemSessionService => _systemSessionService.Value;

        private IUserProvider? GetUserProviderAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Items.ContainsKey("userProvider"))
            {
                return context?.Items["userProvider"] as IUserProvider ?? null;
            }

            return null;
        }
    }
}
