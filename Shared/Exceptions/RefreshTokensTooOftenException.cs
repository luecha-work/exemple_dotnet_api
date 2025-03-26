using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class RefreshTokensTooOftenException : ApplicationException
    {
        public RefreshTokensTooOftenException()
            : base(
                $"This session just refreshed. The token did not arrive in 5 minutes. Please try again later."
            )
        { }
    }
}