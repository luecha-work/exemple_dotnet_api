using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enum
{
    public enum SessionStatusEnum
    {
        [EnumMember(Value = "B")]
        Blocked,

        [EnumMember(Value = "A")]
        Active,

        [EnumMember(Value = "E")]
        Expired
    }
}
