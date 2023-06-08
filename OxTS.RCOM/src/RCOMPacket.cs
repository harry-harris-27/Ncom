using System;

namespace OxTS.RCOM
{
    /// <summary>
    /// Represents a single OxTS RCOM data packet.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RCOM is a data format designed by OxTS for the efficommunication of RT-Range data. The
    /// format contains the data required to calculate range measurements between inertial
    /// navigation systems and information about lane positioning.
    /// </para>
    /// <para>
    /// There are five different types of RCOM packet within the RCOM format. Each one contains
    /// different information depending on it's intended use.
    /// </para>
    /// </remarks>
    public abstract class RCOMPacket : IMarshallable
    {

        /// <summary>
        /// The first byte of an RCOM packet.
        /// </summary>
        public const byte SyncByte = 0x57;

        /// <summary>
        /// Gets the size of an RCOM packet header, including the <see cref="SyncByte"/>.
        /// </summary>
        internal const int PacketHeaderSize = 4;


        protected RCOMPacket(RCOMPacketType packetType)
        {
            PacketType = packetType;
        }


        /// <summary>
        /// Gets the <see cref="RCOMPacketType"/> of this <see cref="RCOMPacket"/>.
        /// </summary>
        public RCOMPacketType PacketType { get; }


        public bool Checksum { get; private set; } = false;


        /// <inheritdoc/>
        public abstract int GetMarshalledSize();

        /// <inheritdoc/>
        public virtual bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            bytes = GetMarshalledSize();

            if (buffer.Length < bytes)
            {
                bytes = -1;
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public virtual bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            bytes = -1;

            // Check for valid header
            if (buffer.Length < PacketHeaderSize) return false;

            // Check that the packet starts with the correct Sync byte
            if (buffer[0] != SyncByte) return false;

            // Check is the packet of right type to be unmarshalled
            if (buffer[1] != (byte)PacketType)
            {
                return false;
            }

            // Check the payload length
            ByteHandling.Unmarshal(buffer.Slice(2), out ushort dataLength);
            if (dataLength < buffer.Length - PacketHeaderSize)
            {
                return false;
            }

            // Check the checksum
            Checksum = ByteHandling.CalculateChecksum(buffer.Slice(1, PacketHeaderSize + dataLength - 1)) == buffer[PacketHeaderSize + dataLength];

            bytes = PacketHeaderSize + dataLength;
            return false;
        }
    }
}
