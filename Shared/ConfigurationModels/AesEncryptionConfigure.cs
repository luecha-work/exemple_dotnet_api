using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ConfigurationModels
{
    public class AesEncryptionConfigure
    {
        public string Section { get; set; } = "AesEncryptionSettings";
        public string EncryptionKey { get; set; } = string.Empty;
        public string EncryptionVector { get; set; } = string.Empty;
    }
}
