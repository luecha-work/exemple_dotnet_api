using Shared.ConfigurationModels;
using IService;
using IRepository;
// using Serilog;
using Microsoft.Extensions.Logging;

namespace Service
{
    public class HangfireJobService : IHangfireJobService
    {
        private readonly IRepositoryManager _repositoryEFManager;
        private readonly IRepositoryManager _repositoryDPManager;
        private readonly IServiceManager _service;
        private readonly IUserProvider _userProvider;
        private readonly ILogger _logger;

        public HangfireJobService(
            IRepositoryFactory repositoryFactory,
            IServiceManager service,
            IUserProvider userProvider,
            ILogger<HangfireJobService> logger
        )
        {
            _repositoryDPManager = repositoryFactory.Create(RepoType.Dapper);
            _repositoryEFManager = repositoryFactory.Create(RepoType.EntityFramework);
            _service = service;
            _userProvider = userProvider;
            _logger = logger;
        }

        public void TestCreateJob()
        {
            _logger.LogInformation("************ TestCreateJob");
        }

        public void TestCreateRecurringJob()
        {
            _logger.LogInformation("************ TestCreateRecurringJob");
        }

        public void TestRepositoryDPManagerForBook()
        {
            throw new NotImplementedException();
        }

        public void TestRepositoryEFManagerForBook()
        {
            throw new NotImplementedException();
        }
    }
}
