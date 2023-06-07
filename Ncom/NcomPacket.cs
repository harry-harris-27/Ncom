using Ncom.Enumerations;
using System;

namespace Ncom
{
    /// <summary>
    /// Represents a single OxTS NCOM data packet.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ncom is a data format designed by OxTS for the efficient communication of navigation
    /// measurements and other data. It is a very compact format and only includes core
    /// measurements, which makes it particularly suitable for inertial navigation systems.
    /// </para>
    /// <para>
    /// Two versions of the NCOM packet exist, referred to as NCOM structure-A and NCOM
    /// structure-B. Byte 21 of an Ncom packet, the <see cref="NavigationStatus"/> byte, identifies
    /// which structure a packet employs. See <see cref="NcomPacketA" /> and
    /// <see cref="NcomPacketB" /> respectively.
    /// </para>
    /// </remarks>
    /// <seealso cref="NcomPacketA" />
    /// <seealso cref="NcomPacketB" />
    public abstract class NcomPacket : IMarshallable
    {

        /// <summary>
        /// <para>
        /// The first-byte of an NCOM packet is the sync byte, which always has a value of
        /// <c>0xE7</c>.
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

        internal const int Checksum3Index = PacketLength - 1;


        /// <summary>
        /// Initializes a new instance of the <see cref="NcomPacket"/> class.
        /// </summary>
        public NcomPacket() : this(null)
        {

        }

        /// <summary>
        /// Copy constructor. Initializes a new instance of the <see cref="NcomPacket"/> class,
        /// logically equivalent to to the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="NcomPacket"/> with which to initialise this instance.
        /// </param>
        public NcomPacket(NcomPacket source)
        {
            if (source != null)
            {
                this.Checksum3 = source.Checksum3;
                this.NavigationStatus = source.NavigationStatus;
            }
        }


        /// <inheritdoc/>
        public int MarshalledSize => PacketLength;

        /// <summary>
        /// The final checksum that verifies the entire encoded NCOM packet (bytes 1-70), ignoring the <see cref="SyncByte"/>.
        /// </summary>
        /// <remarks>
        /// Note that this value is only set when an NCOM packet has been decoded using
        /// <see cref="Unmarshal(ReadOnlySpan{byte})"/>. When <c>false</c>, one should assume that all the
        /// members of this instance are incorrect/unreliable.
        /// </remarks>
        public bool Checksum3 { get; protected set; } = false;

        /// <summary>
        /// Gets or sets the value Navigation Status byte.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of this status byte is initially used to the structure of the encoded packet
        /// (see <see cref="NcomPacketA"/> and <see cref="NcomPacketB"/>).
        /// </para><para>
        /// As well as revealing the packet structure, the navigation status byte also describes the
        /// state of inertial navigation system (INS) and when the packet was created. In the case of
        /// asynchronous NCOM packets, the value of the navigation status byte can be used to identify
        /// what triggered the packet.
        /// </para>
        /// </remarks>
        public virtual NavigationStatus NavigationStatus { get; set; } = NavigationStatus.Invalid;


        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="value">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise
        /// <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method tests object nullity, type and reference before testing equivalence. For a
        /// pure equivalence test, use <see cref="IsEqual(NcomPacket)"/>.
        /// </remarks>
        /// <seealso cref="IsEqual(NcomPacket)"/>
        public override bool Equals(object value)
        {
            // Is it null?
            if (value is null) return false;

            // Is the same object
            if (ReferenceEquals(this, value)) return true;

            // Is same type?
            if (value.GetType() != this.GetType()) return false;

            return IsEqual((NcomPacket)value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        /// <remarks>
        /// The hash code generation process ignores the <see cref="Checksum3"/> property, since it
        /// is not used when testing for equality.
        /// </remarks>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + (byte)NavigationStatus;

            return hash;
        }

        /// <inheritdoc/>
        public virtual void Marshal(Span<byte> buffer)
        {
             // Add the Sync byte
            buffer[0] = SyncByte;

            // Add the Navigation status byte to the buffer
            buffer[21] = (byte)NavigationStatus;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// When the first byte of the <paramref name="buffer"/> is not equals to <see cref="SyncByte"/>.
        /// </exception>
        public virtual void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            // Check that the buffer is largew enough to contain a marshalled NCOM packet
            this.CheckedBufferSize(buffer);

            // Check that it starts wit the sync byte
            if (buffer[0] != SyncByte)
            {
                throw new ArgumentNullException("Marshalled NCOM packet starts with unexpected value. Expected 0x" + SyncByte.ToString("X2") + ".");
            }

            // Get navigation status byte
            NavigationStatus = ByteHandling.ParseEnum(buffer[21], NavigationStatus.Unknown);

            // Calculate Checksum 3
            Checksum3 = CalculateChecksum(buffer.Slice(1, PacketLength - 2)) == buffer[PacketLength - 1];
        }


        /// <summary>
        /// Calculates the NCOM packet checksums. Note that the Sync byte is not included in any
        /// checksum calculations.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks>
        /// Checksums allow the integrity of the NCOM packet to be checked and intermediate
        /// checksums also allow check validity of data upto that point without having to wait for
        /// the whole packet.
        /// </remarks>
        protected static byte CalculateChecksum(ReadOnlySpan<byte> buffer)
        {
            byte cs = 0;
            unchecked
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    cs += buffer[i];
                }
            }

            return cs;
        }

        /// <summary>
        /// A pure implementation of value equality that avoids the routine type and null checks in
        /// <see cref="object.Equals(object)"/>. When overriding the default equals method, override this
        /// method instead.
        /// </summary>
        /// <param name="pkt">The NCOM packet to check to</param>
        /// <returns></returns>
        /// <remarks>
        /// Note that this method does not implement any non-nullity checks. If there is the
        /// possiblity that Ncom packet argument could be null, then use the default
        /// <see cref="object.Equals(object)"/> method.
        /// </remarks>
        protected virtual bool IsEqual(NcomPacket pkt)
        {
            return this.NavigationStatus == pkt.NavigationStatus;
        }

    }
}
