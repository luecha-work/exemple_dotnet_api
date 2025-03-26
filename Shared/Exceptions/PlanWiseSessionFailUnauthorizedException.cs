using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public sealed class PlanWiseSessionFailUnauthorizedException: UnauthorizedException
    {
        public PlanWiseSessionFailUnauthorizedException()
           : base($"The system checked and found a problem with the session. Please log in again.")
        { }
    }
}
