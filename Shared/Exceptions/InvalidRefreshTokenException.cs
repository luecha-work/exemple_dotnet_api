using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class InvalidRefreshTokenException : ApplicationException
    {
        public InvalidRefreshTokenException()
            : base($"There was a problem validating the refresh token. Please log in again.") { }
    }
}