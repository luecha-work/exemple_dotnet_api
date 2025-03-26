using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class RefreshTokenExpirationTimeException : ApplicationException
    {
        public RefreshTokenExpirationTimeException()
            : base(
                $"This session has expired. And the token cannot be refreshed. Please log in again."
            )
        { }
    }
}