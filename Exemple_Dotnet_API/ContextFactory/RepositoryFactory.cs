using IRepository;

namespace Exemple_Dotnet_API.ContextFactory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepositoryManager Create(RepoType repoType)
        {
            return repoType switch
            {
                RepoType.EntityFramework
                   => (IRepositoryManager)_serviceProvider.GetRequiredService<Repository.EntityFramework.RepositoryManager>(),

                RepoType.Dapper
                    => (IRepositoryManager)_serviceProvider.GetRequiredService<Repository.Dapper.RepositoryManager>(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
