using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Bytes 63 to 70 of an Ncom structure-A packet are collectively call Batch S. Batch S 
    /// contains status channel information from the INS. The information transmitted in Batch S 
    /// is defined by the value of the status byte, which defines the structure of each status 
    /// channel and the information it contains.
    /// <para>
    /// There are so many status messages it is impossible to transmit them all in a single Ncom 
    /// packet. So instead, the status messages are split into a number of groups called channels,
    /// made up of 8 bytes each - and one channel is inserted into each Ncom packet.
    /// </para>
    /// <para>
    /// It is important to note that the value of the status channel byte does not increase 
    /// incrementally, This is because some status channels are more important than others, and 
    /// need to be transmitted more often. It is also important to note that the channel 
    /// transmission order may change between software versions. However, the channel transmission 
    /// list will repeat approximately once every 200 Ncom structure-A packets.
    /// </para>
    /// </summary>
    public abstract class StatusChannel
    {

        /* ---------- Constants ---------------------------------------------------------------/**/

        internal const int STATUS_CHANNEL_LENGTH = 9;


        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel(byte statusChannelByte)
        {
            StatusChannelByte = statusChannelByte;
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        public int MarshalLength { get { return 9; } }      // 8 + status byte

        public byte StatusChannelByte { get; private set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override bool Equals(object value)
        {
            // Is it null?
            if (value is null) return false;

            // Is the same object
            if (ReferenceEquals(this, value)) return true;

            // Is same type?
            if (value.GetType() != this.GetType()) return false;

            return IsEqual((StatusChannel)value);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + StatusChannelByte;

            return hash;
        }

        public virtual byte[] Marshal()
        {
            byte[] buf = new byte[MarshalLength];

            // Status byte
            buf[0] = StatusChannelByte;

            return buf;
        }

        public virtual bool Unmarshal(byte[] buffer, int offset)
        {
            if (offset + MarshalLength > buffer.Length) return false;

            // Status byte
            StatusChannelByte = buffer[offset];

            return true;
        }


        /* ---------- Protected methods -------------------------------------------------------/**/

        /// <summary>
        /// A pure implementation of value equality that avoids the routine checks in 
        /// <see cref="Equals(object)"/>.
        /// To override the default equals method, override this method instead.
        /// </summary>
        /// <param name="pkt"></param>
        /// <returns></returns>
        protected virtual bool IsEqual(StatusChannel pkt)
        {
            return this.StatusChannelByte == pkt.StatusChannelByte;
        }

    }
}
