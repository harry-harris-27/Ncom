using System;

namespace OxTS
{
    /// <summary>
    /// Represents a 3-dimensional vector of floats that marshals using 24-bit integers and some scaling factor
    /// </summary>
    internal class Vector3 : IMarshallable
    {
        private const int Vector3Length = 9;

        private float scaleFactor;


        public Vector3(float scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }


        public float X {get; set; }
        public float Y {get; set; }
        public float Z { get; set; }


        public int GetMarshalledSize() => Vector3Length;

        public bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (buffer.Length < Vector3Length)
            {
                bytes = -1;
                return false;
            }

            ByteHandling.MarshalInt24(buffer.Slice(0), (int)(X / scaleFactor));
            ByteHandling.MarshalInt24(buffer.Slice(3), (int)(Y / scaleFactor));
            ByteHandling.MarshalInt24(buffer.Slice(6), (int)(Z / scaleFactor));

            bytes = Vector3Length;
            return true;
        }

        public bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (buffer.Length < Vector3Length)
            {
                bytes = -1;
                return false;
            }

            int offset = 0;
            ByteHandling.UnmarshalInt24(buffer, ref offset, out int x);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out int y);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out int z);

            this.X = x * scaleFactor;
            this.Y = y * scaleFactor;
            this.Z = z * scaleFactor;

            bytes = Vector3Length;
            return true;
        }
    }
}
