using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Shared.ConfigurationModels
{
    public interface IUserProvider
    {
        Account UserInfo { get; set; }
        List<string> RoleInfo { get; set; }
    }

    public class UserProvider : IUserProvider
    {
        public Account UserInfo { get; set; } = new Account();
        public List<string> RoleInfo { get; set; } = new List<string>();
    }
}
