using System;

namespace OxTS.RCOM
{
    public class EightLineVector : IMarshallable
    {

        internal const int MarshalledSize = 16;

        private readonly float scaleFactor;


        public EightLineVector(float scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }


        public float Line1 { get; set; }
        public float Line2 { get; set; }
        public float Line3 { get; set; }
        public float Line4 { get; set; }
        public float Line5 { get; set; }
        public float Line6 { get; set; }
        public float Line7 { get; set; }
        public float Line8 { get; set; }


        /// <inheritdoc/>
        public int GetMarshalledSize() => MarshalledSize;

        /// <inheritdoc/>
        public bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (buffer.Length < MarshalledSize)
            {
                bytes = -1;
                return false;
            }

            int offset = 0;
            ByteHandling.Marshal(buffer, ref offset, (int)(Line1 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line2 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line3 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line4 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line5 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line6 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line7 / scaleFactor));
            ByteHandling.Marshal(buffer, ref offset, (int)(Line8 / scaleFactor));

            bytes = MarshalledSize;
            return true;
        }

        /// <inheritdoc/>
        public bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (buffer.Length < MarshalledSize)
            {
                bytes = -1;
                return false;
            }

            int offset = 0;
            ByteHandling.Unmarshal(buffer, ref offset, out int line1);
            ByteHandling.Unmarshal(buffer, ref offset, out int line2);
            ByteHandling.Unmarshal(buffer, ref offset, out int line3);
            ByteHandling.Unmarshal(buffer, ref offset, out int line4);
            ByteHandling.Unmarshal(buffer, ref offset, out int line5);
            ByteHandling.Unmarshal(buffer, ref offset, out int line6);
            ByteHandling.Unmarshal(buffer, ref offset, out int line7);
            ByteHandling.Unmarshal(buffer, ref offset, out int line8);

            Line1 = line1 * scaleFactor;
            Line2 = line2 * scaleFactor;
            Line3 = line3 * scaleFactor;
            Line4 = line4 * scaleFactor;
            Line5 = line5 * scaleFactor;
            Line6 = line6 * scaleFactor;
            Line7 = line7 * scaleFactor;
            Line8 = line8 * scaleFactor;

            bytes = MarshalledSize;
            return true;
        }
    }
}
