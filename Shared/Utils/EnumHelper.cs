using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class EnumHelper
    {
        public static string GetEnumValue(System.Enum value)
        {
            EnumMemberAttribute attribute = GetEnumMemberAttribute(value);
            return attribute?.Value ?? string.Empty;
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(System.Enum value)
        {
            Type type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());

            return (EnumMemberAttribute)
                Attribute.GetCustomAttribute(fieldInfo, typeof(EnumMemberAttribute));
        }
    }
}
