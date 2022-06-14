using System;

namespace Ncom
{
    internal static class ByteHandling
    {
        public static TEnum ParseEnum<TEnum>(byte b, TEnum defaultVal) where TEnum : struct, IConvertible
        {
            if (!TryParseEnum<TEnum>(b, out TEnum returnValue))
            {
                returnValue = defaultVal;
            }
            return returnValue;
        }

        public static bool TryParseEnum<TEnum>(byte b, out TEnum value) where TEnum : struct, IConvertible
        {
            // Check T is an enum
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enumerated type");

            value = default;

            // Is the value 'b' defined in the enum type
            if (Enum.IsDefined(typeof(TEnum), b))
            {
                // Safe to cast byte to enum
                value = (TEnum)Enum.ToObject(typeof(TEnum), b);
                return true;
            }

            return false;
        }

        #region Marshalling

        public static void Marshal(byte[] buffer, ref int offset, short value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
            offset += 2;
        }

        public static void Marshal(byte[] buffer, ref int offset, ushort value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
            offset += 2;
        }

        public static void MarshalInt24(byte[] buffer, ref int offset, int value)
        {
            Array.Copy(BitConverter.GetBytes(value << 8), 1, buffer, offset, 3);
            offset += 3;
        }

        public static void Marshal(byte[] buffer, ref int offset, int value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public static void Marshal(byte[] buffer, ref int offset, float value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public static void Marshal(byte[] buffer, ref int offset, double value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
            offset += 8;
        }

        #endregion

        #region Unmarshalled

        public static void UnmarshalInt16(byte[] buffer, ref int offset, ref short storage)
        {
            storage = BitConverter.ToInt16(buffer, offset);
            offset += 2;
        }

        public static short UnmarshalInt16(byte[] buffer, ref int offset)
        {
            short value = 0;
            UnmarshalInt16(buffer, ref offset, ref value);
            return value;
        }

        public static void UnmarshalUInt16(byte[] buffer, ref int offset, ref ushort storage)
        {
            storage = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
        }

        public static ushort UnmarshalUInt16(byte[] buffer, ref int offset)
        {
            ushort value = 0;
            UnmarshalUInt16(buffer, ref offset, ref value);
            return value;
        }

        public static void UnmarshalInt24(byte[] buffer, ref int offset, ref int storage)
        {
            storage = buffer[offset + 2] << 24;
            storage |= buffer[offset + 1] << 16;
            storage |= buffer[offset] << 8;

            offset += 3;
        }

        public static int UnmarshalInt24(byte[] buffer, ref int offset)
        {
            int value = 0;
            UnmarshalInt24(buffer, ref offset, ref value);
            return value;
        }

        public static void UnmarshalInt32(byte[] buffer, ref int offset, ref int storage)
        {
            storage = BitConverter.ToInt32(buffer, offset);
            offset += 4;
        }

        public static int UnmarshalInt32(byte[] buffer, ref int offset)
        {
            int value = 0;
            UnmarshalInt32(buffer, ref offset, ref value);
            return value;
        }

        public static void UnmarshalSingle(byte[] buffer, ref int offset, ref float storage)
        {
            storage = BitConverter.ToSingle(buffer, offset);
            offset += 4;
        }

        public static float UnmarshalSingle(byte[] buffer, ref int offset)
        {
            float value = 0;
            UnmarshalSingle(buffer, ref offset, ref value);
            return value;
        }

        public static void UnmarshalDouble(byte[] buffer, ref int offset, ref double storage)
        {
            storage = BitConverter.ToDouble(buffer, offset);
            offset += 8;
        }

        public static double UnmarshalDouble(byte[] buffer, ref int offset)
        {
            double value = 0;
            UnmarshalDouble(buffer, ref offset, ref value);
            return value;
        }

        #endregion
    }
}
