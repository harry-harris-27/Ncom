using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom.Utilities
{
    internal static class EnumParser
    {
        
        public static T ParseByte<T>(byte b, T defaultVal) where T : struct, IConvertible
        {
            // Check T is an enum
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            // Is the value 'b' defined in the enum type
            if (!Enum.IsDefined(typeof(T), b)) return defaultVal;

            // Safe to cast byte to enum
            return (T) Enum.ToObject(typeof(T), b);
        }

    }
}
