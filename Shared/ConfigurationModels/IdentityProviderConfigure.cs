using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ConfigurationModels
{
    public class IdentityProviderConfigure
    {
        public string Section { get; set; } = "AuthProvider";
        public string LoginProvider { get; set; } = string.Empty;
        public string RefreshTokenProvider { get; set; } = string.Empty;
    }
}
