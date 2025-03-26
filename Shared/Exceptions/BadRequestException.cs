using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
            : base(message) { }
    }
}
