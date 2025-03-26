using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public sealed class LoginBadRequestException : BadRequestException
    {
        public LoginBadRequestException(string message)
            : base(message) { }
    }
}
