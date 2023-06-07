using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OxTS
{
    internal static class ByteHandling
    {
        #region Marshalling

        public static void Marshal(Span<byte> buffer, ref int offset, short value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 2;
        }

        public static void Marshal(Span<byte> buffer, ref int offset, ushort value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 2;
        }

        public static void MarshalInt24(Span<byte> buffer, ref int offset, int value)
        {
            buffer = buffer.Slice(offset);
            if (buffer.Length > 3)
            {
                byte temp = buffer[3];
                Marshal(buffer, value);
                buffer[3] = temp;
            }
            else
            {
                Marshal(buffer.Slice(offset, 3), value);
            }

            offset += 3;
        }

        public static void Marshal(Span<byte> buffer, ref int offset, int value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 4;
        }

        public static void Marshal(Span<byte> buffer, ref int offset, uint value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 4;
        }

        public static void Marshal(Span<byte> buffer, ref int offset, float value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 4;
        }

        public static void Marshal(Span<byte> buffer, ref int offset, double value)
        {
            Marshal(buffer.Slice(offset), value);
            offset += 8;
        }

        public static void Marshal<T>(Span<byte> buffer, T value)
        {
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), value);
        }

        public static void Marshal(Span<byte> buffer, ref int offset, IMarshallable marshallable)
        {
            marshallable.Marshal(buffer.Slice(offset));
            offset += marshallable.MarshalledSize;
        }

        #endregion

        #region Unmarshalled

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out short storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 2;
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out ushort storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 2;
        }

        public static void UnmarshalInt24(ReadOnlySpan<byte> buffer, out int storage)
        {
            Unmarshal(buffer.Slice(0, 3), out storage);

            storage &= 0x00FFFFFF;                                  // Ignore any 4th bytes that are included
            storage |= ((sbyte)((storage >> 16) & 0xFF)) << 16;     // Handle negatives
        }

        public static void UnmarshalInt24(ReadOnlySpan<byte> buffer, ref int offset, out int storage)
        {
            UnmarshalInt24(buffer.Slice(offset, 3), out storage);
            offset += 3;
        }

        public static void UnmarshalInt24(ReadOnlySpan<byte> buffer, ref int offset, out float storage, float scaling)
        {
            UnmarshalInt24(buffer, ref offset, out int int24);
            storage = int24 * scaling;
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out int storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 4;
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out uint storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 4;
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out float storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 4;
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, out double storage)
        {
            Unmarshal(buffer.Slice(offset), out storage);
            offset += 8;
        }

        public static void Unmarshal<T>(ReadOnlySpan<byte> buffer, out T storage)
            where T : struct
        {
            ref byte reference = ref MemoryMarshal.GetReference(buffer);
            storage = Unsafe.ReadUnaligned<T>(ref reference);
        }

        public static void Unmarshal(ReadOnlySpan<byte> buffer, ref int offset, IMarshallable marshallable)
        {
            marshallable.Unmarshal(buffer.Slice(offset));
            offset += marshallable.MarshalledSize;
        }

        #endregion

        #region Enum

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
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

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

        #endregion
    }
}
