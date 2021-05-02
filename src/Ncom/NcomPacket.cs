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
    public abstract class NcomPacket
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


        /// <summary>
        /// The final checksum that verifies the entire encoded NCOM packet (bytes 1-70). 
        /// </summary>
        /// <remarks>
        /// Note that this value is only set when an NCOM packet has been decoded using 
        /// <see cref="Unmarshal(byte[], int)"/>. When <c>false</c>, one should assume that all the 
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
        public NavigationStatus NavigationStatus { get; set; } = NavigationStatus.Invalid;


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

        /// <summary>
        /// Marshals this <see cref="NcomPacket"/> into a byte array of length 
        /// <see cref="PacketLength"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="byte"/> array of length equal to <see cref="PacketLength"/>.
        /// </returns>
        public virtual byte[] Marshal()
        {
            // Create an array of required length
            byte[] buffer = new byte[PacketLength];

            // Add the Sync byte
            buffer[0] = SyncByte;

            // Add the Navigation status byte to the buffer
            buffer[21] = (byte)NavigationStatus;

            // Return the buffer
            return buffer;
        }

        /// <summary>
        /// Unmarshals the data stored in the specified <paramref name="buffer"/> into this 
        /// <see cref="NcomPacket"/>. If no marshalled NCOM packet can be found, 
        /// <see langword="false"/> is returned.
        /// </summary>
        /// <param name="buffer">The byte array containing the marshalled NCOM packet.</param>
        /// <param name="offset">
        /// The zero-based index indicating the location in the buffer to start looking for a sync 
        /// byte from.
        /// </param>
        /// <returns>
        /// <see langword="false"/> if no marshalled NCOM packet can be found, otherwise 
        /// <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When the <paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// When the <paramref name="offset"/> is less than <c>0</c>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If the first byte of the buffer is not <see cref="SyncByte"/> then the method will look
        /// for the first occurance of the sync byte.
        /// </para>
        /// <para>
        /// The <paramref name="offset"/> can be greater than the length of the 
        /// <paramref name="buffer"/> without throwing an exception, returning 
        /// <see langword="false"/>. This allows for easy unmarshalling of multiple consecutive 
        /// packets, e.g. <see langword="while" /> loops.
        /// </para>
        /// </remarks>
        public virtual bool Unmarshal(byte[] buffer, int offset)
        {
            // Check that the buffer is not null
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            // Check that the offset is in the expected range
            if (offset < 0) throw new IndexOutOfRangeException("Offset in buffer can not be less than zero");

            // Seek the sync byte
            while (offset <= buffer.Length - PacketLength)
            {
                // Have we found the sync byte?
                if (buffer[offset++] == SyncByte)
                {
                    // packet found. offset now points to [1]

                    // Get navigation status byte
                    NavigationStatus = ByteHandling.ParseEnum(buffer[offset + 20], NavigationStatus.Unknown);

                    // Calculate Checksum 3
                    Checksum3 = CalculateChecksum(buffer, offset, 70) == buffer[offset + PacketLength - 2];

                    // Unmarshalled OK, return true
                    return true;
                }
            }

            // Couldn't find packet, return false;
            return false;
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
        protected static byte CalculateChecksum(byte[] buffer, int offset, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            byte cs = 0;

            unchecked
            {
                for (int i = offset; (i < buffer.Length) && (length > 0); i++)
                {
                    cs += buffer[i];
                    length--;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method is meant for pure value equality and should only be called internally with non-null values")]
        protected virtual bool IsEqual(NcomPacket pkt)
        {
            return this.NavigationStatus == pkt.NavigationStatus;
        }

    }
}
