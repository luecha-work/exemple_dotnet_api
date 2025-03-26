using System.Runtime.Serialization;

namespace Shared.Enum
{
    public enum BlockForceStatusEnum
    {
        [EnumMember(Value = "L")]
        Locked,

        [EnumMember(Value = "U")]
        UnLock
    }
}