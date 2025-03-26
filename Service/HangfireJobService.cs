using Shared.ConfigurationModels;
using IService;
using IRepository;
using Serilog;

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
            IUserProvider userProvider
        )
        {
            _repositoryDPManager = repositoryFactory.Create(RepoType.Dapper);
            _repositoryEFManager = repositoryFactory.Create(RepoType.EntityFramework);
            _service = service;
            _userProvider = userProvider;
            _logger = Log.ForContext<HangfireJobService>();
        }

        public void TestCreateJob()
        {
            _logger.Information("************ TestCreateJob");
        }

        public void TestCreateRecurringJob()
        {
            _logger.Information("************ TestCreateRecurringJob");
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
