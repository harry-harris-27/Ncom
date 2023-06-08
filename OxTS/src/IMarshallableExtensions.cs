using System;

namespace OxTS
{
    public static class IMarshallableExtensions
    {

        #region Marshal

        public static byte[] Marshal(this IMarshallable marshallable)
        {
            var buffer = new byte[marshallable.GetMarshalledSize()];
            marshallable.TryMarshal(buffer.AsSpan(), out _);
            return buffer;
        }

        public static bool TryMarshal(this IMarshallable marshallable, Span<byte> buffer)
        {
            return marshallable.TryMarshal(buffer, out _);
        }

        public static bool TryMarshal(this IMarshallable marshallable, Span<byte> buffer, ref int offset, out int bytes)
        {
            if (marshallable.TryMarshal(buffer.Slice(offset), out bytes))
            {
                offset += bytes;
                return true;
            }

            return false;
        }

        #endregion

        #region Unmarshal

        public static bool TryUnmarshal(this IMarshallable marshallable, byte[] buffer, out int bytes)
        {
            return marshallable.TryUnmarshal(buffer, out bytes);
        }

        public static bool TryUnmarshal(this IMarshallable marshallable, ReadOnlySpan<byte> buffer)
        {
            return marshallable.TryUnmarshal(buffer);
        }

        public static bool TryUnmarshal(this IMarshallable marshallable, ReadOnlySpan<byte> buffer, ref int offset, out int bytes)
        {
            if (marshallable.TryUnmarshal(buffer.Slice(offset), out bytes))
            {
                offset += bytes;
                return true;
            }

            return false;
        }

        #endregion

    }
}
