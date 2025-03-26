using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IRepositoryFactory
    {
        IRepositoryManager Create(RepoType repoType);
    }

    public enum RepoType
    {
        EntityFramework,
        Dapper
    }
}
