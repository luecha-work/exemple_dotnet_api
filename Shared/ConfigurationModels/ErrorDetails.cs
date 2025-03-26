using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.ConfigurationModels
{
    public class ErrorDetails
    {
        public required string Message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
