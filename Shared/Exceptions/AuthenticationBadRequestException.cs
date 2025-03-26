using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public sealed class SingUpWithUsernameDuplicateBadRequestException : BadRequestException
    {
        public SingUpWithUsernameDuplicateBadRequestException(string username)
            : base($"Duplicate Sing-up with Username: {username}.") { }
    }

    [Serializable]
    public sealed class SingUpEmailDuplicateBadRequestException : BadRequestException
    {
        public SingUpEmailDuplicateBadRequestException(string email)
            : base($"Duplicate Sing-up with Email: {email}.") { }
    }
}
