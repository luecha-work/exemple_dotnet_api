using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public sealed class FailedAuthenticateSessionException : ApplicationException
    {
        public FailedAuthenticateSessionException()
            : base("Validate token failed to authenticate session. Please log in again.") { }
    }

    [Serializable]
    public sealed class VerificationTokenNotFoundForCheckSessionException : ApplicationException
    {
        public VerificationTokenNotFoundForCheckSessionException()
            : base("No verification token found for checke session.") { }
    }
}
