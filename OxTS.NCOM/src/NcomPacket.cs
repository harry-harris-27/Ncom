using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// Represents a single OxTS NCOM data packet.
    /// </summary>
    /// <remarks>
    /// <para>
    /// NCOM is a data format designed by OxTS for the efficient communication of navigation
    /// measurements and other data. It is a very compact format and only includes core
    /// measurements, which makes it particularly suitable for inertial navigation systems.
    /// </para>
    /// <para>
    /// Two versions of the NCOM packet exist, referred to as NCOM structure-A and NCOM
    /// structure-B. Byte 21 of an NCOM packet, the <see cref="NavigationStatus"/> byte, identifies
    /// which structure a packet employs. See <see cref="NCOMPacketA" /> and
    /// <see cref="NCOMPacketB" /> respectively.
    /// </para>
    /// </remarks>
    /// <seealso cref="NCOMPacketA" />
    /// <seealso cref="NCOMPacketB" />
    public abstract class NCOMPacket : IMarshallable
    {

        /// <summary>
        /// <para>
        /// The first byte of an NCOM packet.
        /// </para>
        /// <para>
        /// Note the in order to reduce latency with RS232 serial transmissions, the sync character
        /// is transmitted as the end of the previous cycle. On the communication link there will
        /// be a pause between the transmission of the sync and next character. It is not advised
        /// to use this pause to synchronise the packet, even though the operating system should
        /// guarantee the transmission timing of the packet.
        /// </para>
        /// <para>
        /// Over Ethernet the sync byte is transmitted as the first character of the UDP
        /// packet.
        /// </para>
        /// </summary>
        public const byte SyncByte = 0xE7;

        /// <summary>
        /// The length of a marshalled NCOM packet.
        /// </summary>
        public const int PacketLength = 72;

        internal const int NavigationStatusIndex = 21;
        internal const int Checksum3Index = PacketLength - 1;


        /// <summary>
        /// Initializes a new instance of the <see cref="NCOMPacket"/> class.
        /// </summary>
        public NCOMPacket() { }


        /// <summary>
        /// The final checksum that verifies the entire encoded NCOM packet (bytes 1-70), ignoring the <see cref="SyncByte"/>.
        /// </summary>
        /// <remarks>
        /// Note that this value is only set when an NCOM packet has been decoded using
        /// <see cref="TryUnmarshal(ReadOnlySpan{byte}, out int)"/>. When <c>false</c>, one should assume that all the
        /// members of this instance are incorrect/unreliable.
        /// </remarks>
        public bool Checksum3 { get; protected set; } = false;

        /// <summary>
        /// Gets or sets the value Navigation Status byte.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of this status byte is initially used to the structure of the encoded packet
        /// (see <see cref="NCOMPacketA"/> and <see cref="NCOMPacketB"/>).
        /// </para><para>
        /// As well as revealing the packet structure, the navigation status byte also describes the
        /// state of inertial navigation system (INS) and when the packet was created. In the case of
        /// asynchronous NCOM packets, the value of the navigation status byte can be used to identify
        /// what triggered the packet.
        /// </para>
        /// </remarks>
        public virtual NavigationStatus NavigationStatus { get; set; } = NavigationStatus.Invalid;


        /// <inheritdoc/>
        public int GetMarshalledSize() => PacketLength;

        /// <inheritdoc/>
        public bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (buffer.Length < PacketLength)
            {
                bytes = -1;
                return false;
            }
            bytes = PacketLength;

             // Add the Sync byte
            buffer[0] = SyncByte;

            // Add the Navigation status byte to the buffer
            buffer[NavigationStatusIndex] = (byte)NavigationStatus;

            // Marshal packet-specified data
            Marshal(buffer);

            // Perform final checksum
            buffer[Checksum3Index] = ByteHandling.CalculateChecksum(buffer.Slice(1, Checksum3Index - 1));

            return true;
        }

        /// <inheritdoc/>
        public bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (buffer.Length < PacketLength || buffer[0] != SyncByte)
            {
                bytes = -1;
                return false;
            }
            bytes = PacketLength;

            // Get navigation status byte
            NavigationStatus = ByteHandling.ParseEnum(buffer[NavigationStatusIndex], NavigationStatus.Unknown);

            // Validate Checksum 3
            Checksum3 = ByteHandling.CalculateChecksum(buffer.Slice(1, PacketLength - 2)) == buffer[PacketLength - 1];

            // Unmarshal packet-specific data
            Unmarshal(buffer);

            return true;
        }


        /// <summary>
        /// Marshals this data structure into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to write marshalled data to.</param>
        /// <remarks>
        /// The called to this method guarentees that the <paramref name="buffer"/> is large enough
        /// to store the marshalled data.
        /// </remarks>
        protected abstract void Marshal(Span<byte> buffer);

        /// <summary>
        /// Unmsarshals this data structure from the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to read marshalled data from.</param>
        /// <remarks>
        /// The called to this method guarentees that the <paramref name="buffer"/> is large enough
        /// to store the marshalled data.
        /// </remarks>
        protected abstract void Unmarshal(ReadOnlySpan<byte> buffer);

    }
}
