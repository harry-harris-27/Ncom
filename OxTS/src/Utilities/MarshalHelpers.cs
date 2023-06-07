using System;

namespace OxTS
{
    internal static class MarshalHelpers
    {
        public static void CheckedBufferSize(this IMarshallable marshallable, ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length < marshallable.MarshalledSize)
            {
                throw new IndexOutOfRangeException("buffer is too small to contain the marshalled data structure");
            }
        }
    }
}
