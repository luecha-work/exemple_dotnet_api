using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ConfigurationModels
{
    public class JwtConfiguration
    {
        public string Section { get; set; } = "JwtSettings";
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; } = 0;
        public string SecretKey { get; set; } = string.Empty;
    }
}
