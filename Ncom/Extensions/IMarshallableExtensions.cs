using System;

namespace Ncom
{
    public static class IMarshallableExtensions
    {

        #region

        public static byte[] Marshal(this IMarshallable marshallable)
        {
            var buffer = new byte[marshallable.MarshalledSize];
            marshallable.Marshal(buffer.AsSpan());
            return buffer;
        }

        #endregion

        #region Unmarshal

        public static void Unmarshal(this IMarshallable marshallable, byte[] buffer)
        {
            marshallable.Unmarshal(buffer.AsSpan());
        }

        public static void Unmarshal(this IMarshallable marshallable, byte[] buffer, int offset)
        {
            marshallable.Unmarshal(buffer.AsSpan(offset));
        }

        public static void Unmarshal(this IMarshallable marshallable, byte[] buffer, int offset, int length)
        {
            marshallable.Unmarshal(buffer.AsSpan(offset, length));
        }

        #endregion
    }
}
