using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class BlockedRefreshTokenExpirationException : ApplicationException
    {
        public BlockedRefreshTokenExpirationException()
            : base(
                $"This session has been blocked. New token cannot be refreshed. Please log in again."
            )
        { }
    }
}