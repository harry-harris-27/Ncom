using Ncom.Enumerations;
using Ncom.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom
{
    /// <summary>
    /// <para>
    /// Ncom is a data format designed by OxTS for the efficient communication of navigation 
    /// measurements and other data. It is a very compact format and only includes core 
    /// measurements, which makes it particularly suitable for inertial navigation systems.
    /// </para>
    /// <para>
    /// The Ncom packet comprises 72 bytes that can be transmitted over Ethernet (port number 3000) 
    /// or RS232 serial links. To increase efficiency, many of the data packets are sent as 24-bit 
    /// sign integer words because 16-bit do not provide the range/precision required for many of 
    /// the quantities, whereas 32-bit precision makes the packet much longer than required.
    /// </para>
    /// <para>
    /// Two versions of the Ncom packet exist, referred to as Ncom structure-A and Ncom 
    /// structure-B. Byte 21 of an Ncom packet - the navigation status byte, identifies which 
    /// structure a packet employees. For implementations of each see <see cref="NcomPacketA"/> and 
    /// <see cref="NcomPacketB"/> respectively.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// All words are sent in little-endian format (meaning "little-end first" or "least significant byte first" [LSB]), 
    /// which is compatible with Intel microprocessors. A definition of word lengths is shown below:
    /// </para>
    /// <para>
    ///     Byte (UByte)    =>      8-bit integer (unsigned)
    ///     Short (UShort)  =>      16-bit integer (unsigned)
    ///     Word (UWord)    =>      24-bit integer (unsigned)
    ///     Long (ULong)    =>      32-bit integer (unsigned)
    ///     Float           =>      32-bit IEEE 752 floating-point
    ///     Double          =>      64-bit IEEE 754 floating-point
    /// </para>
    /// </remarks>
    /// <seealso cref="NcomPacketA"/>
    /// <seealso cref="NcomPacketB"/>
    public abstract class NcomPacket
    {

        /* ---------- Constants ---------------------------------------------------------------/**/

        /// <summary>
        /// <para>
        /// The first-byte of an Ncom packet is the sync byte, which always has a value of 0xE7.
        /// </para>
        /// <para>
        /// Note the in order to reduce latency with RS232 serial transmissions, the sync character 
        /// is transmitted as the end of the previous cycle. On the communication link there will 
        /// be a pause between the transmission of the sync and next character. It is not advised 
        /// to use this pause to synchronise the packet, even though the operating system should 
        /// guarantee the transmission timing of the packet.
        /// </para>
        /// <para>
        /// Over Ethernet the sync character is transmitted as the first character of the UDP 
        /// packet.
        /// </para>
        /// </summary>
        public const byte SYNC_BYTE = 0xE7;

        /// <summary>
        /// The length of a marshalled Ncom packet.
        /// </summary>
        public const int PACKET_LENGTH = 72;


        /* ---------- Private variables -------------------------------------------------------/**/



        /* ---------- Constructors ------------------------------------------------------------/**/
        
        /// <summary>
        /// Copy constructor. Deeply copies the specified Ncom packet and returns a new instance.
        /// If a null reference is passed, a default Ncom packet is returned.
        /// </summary>
        /// <param name="pkt">The original Ncom packet object to copy.</param>
        public NcomPacket(NcomPacket pkt)
        {
            if (pkt != null)
            {
                this.Checksum3 = pkt.Checksum3;
                this.NavigationStatus = pkt.NavigationStatus;
            }
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        /// <summary>
        /// The final checksum that verifies the entire packet (bytes 1-70).
        /// <para>
        /// Note that this is set to <code>false</code> by default. Whenever
        /// <see cref="Unmarshal(byte[], int)"/> is called, then this value is set to the validity 
        /// of the packet. If set to false, then the packet's members should assume to be 
        /// incorrect.
        /// </para>
        /// </summary>
        public bool Checksum3 { get; protected set; } = false;

        /// <summary>
        /// Gets the number of bytes of the marshalled Ncom packet, see 
        /// <see cref="PACKET_LENGTH"/>.
        /// </summary>
        public int MarshalLength { get { return PACKET_LENGTH; } }

        /// <summary>
        /// <para>
        /// Byte 21 of the Ncom packet is called the navigation status. The value of the navigation 
        /// status byte is initially used to test the structure of the Ncom packet. If the value of the 
        /// navigation status byte is 0, 1, 2, 3, 4, 5, 6, 7, 10, 20, 21 or 22 the packet format is 
        /// Ncom structure-A, and can be decoded. Any other value indicates the packet should be 
        /// ignored.
        /// </para>
        /// <para>
        /// As well as revealing the packet structure, the navigation status byte also describes the 
        /// state of inertial navigation system (INS) and when the packet was created. In the case of 
        /// asynchronous Ncom packets, the value of the navigation status byte can be used to identify 
        /// what triggered the packet.
        /// </para>
        /// <para>
        /// For structure-A packets checksum 1 (byte 22), which immediately follows the navigation 
        /// status byte, allows the measurements in Batch A, and the value of the navigation status 
        /// byte, to be verified and used without waiting to receive the entire Ncom packet. This is 
        /// useful in time-critical applications receiving data over RS323 as the inertial measurements 
        /// can be used to updated other solutions with minimal latency.
        /// </para>
        /// </summary>
        public NavigationStatus NavigationStatus { get; set; } = NavigationStatus.Invalid;


        /* ---------- Public methods ----------------------------------------------------------/**/

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="value">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise 
        /// <c>false</c>.
        /// </returns>
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
        /// Marshals the data into a byte array of length <see cref="PACKET_LENGTH"/>.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Marshal()
        {
            // Create an array of required length
            byte[] buffer = new byte[PACKET_LENGTH];

            // Add the Sync byte
            buffer[0] = SYNC_BYTE;

            // Add the Navigation status byte to the buffer
            buffer[21] = (byte)NavigationStatus;

            // Return the buffer
            return buffer;
        }

        /// <summary>
        /// Unmarshals the data stored in <paramref name="buffer"/> into this instance of an Ncom 
        /// packet. If no marshalled Ncom packet can be found, false is returned.
        /// <para>
        /// If the first byte of the buffer is not <see cref="SYNC_BYTE"/> then the method will 
        /// look for the first occurance of the sync byte.
        /// </para>
        /// </summary>
        /// <param name="buffer">The byte array containing the marshalled Ncom packet.</param>
        /// <param name="offset">
        /// The zero-based index indicating the location in the buffer to start looking for a sync 
        /// byte from.
        /// </param>
        /// <returns>False if no marshalled Ncom packet can be found, otherwise true.</returns>
        public virtual bool Unmarshal(byte[] buffer, int offset)
        {
            // Seek the sync byte
            while (offset <= buffer.Length - PACKET_LENGTH)
            {
                // Have we found the sync byte?
                if (buffer[offset++] == SYNC_BYTE)
                {
                    // packet found. offset now points to [1]

                    // Get navigation status byte
                    //NavigationStatus = EnumParser.ParseByte(buffer[offset + 20], NavigationStatus.Unknown);
                    NavigationStatus = (NavigationStatus)buffer[offset + 20];

                    // Calculate Checksum 3
                    Checksum3 = CalculateChecksum(buffer, offset, 70) == buffer[offset + PACKET_LENGTH - 2];

                    // Unmarshalled OK, return true
                    return true;
                }
            }

            // Couldn't find packet, return false;
            return false;
        }


        /* ---------- Protected methods -------------------------------------------------------/**/

        /// <summary>
        /// Calculates the Ncom packet checksums. Note that the Sync byte is not included in any 
        /// checksum calculations.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks>
        /// Checksums allow the integrity of the Ncom packet to be checked and intermediate 
        /// checksums also allow check validity of data upto that point without having to wait for 
        /// the whole packet.
        /// </remarks>
        protected static byte CalculateChecksum(byte[] buffer, int offset, int length)
        {
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
        /// <see cref="Equals(object)"/>. When overriding the default equals method, override this 
        /// method instead.
        /// </summary>
        /// <param name="pkt">The Ncom packet to check to</param>
        /// <returns></returns>
        /// <remarks>
        /// Note that this method does not implement any non-nullity checks. If there is the 
        /// possiblity that Ncom packet argument could be null, then use the default 
        /// <see cref="Equals(object)"/> method.
        /// </remarks>
        protected virtual bool IsEqual(NcomPacket pkt)
        {
            return this.NavigationStatus == pkt.NavigationStatus;
        }

    }
}
