using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    /// <summary>
    /// Bytes 63 to 70 of an NCOM structure-A packet are collectively call Batch S. Batch S 
    /// contains status channel information from the INS. The information transmitted in Batch S 
    /// is defined by the value of the status byte, which defines the structure of each status 
    /// channel and the information it contains.
    /// <para>
    /// There are so many status messages it is impossible to transmit them all in a single NCOM 
    /// packet. So instead, the status messages are split into a number of groups called channels,
    /// made up of 8 bytes each - and one channel is inserted into each NCOM packet.
    /// </para>
    /// <para>
    /// It is important to note that the value of the status channel byte does not increase 
    /// incrementally, This is because some status channels are more important than others, and 
    /// need to be transmitted more often. It is also important to note that the channel 
    /// transmission order may change between software versions. However, the channel transmission 
    /// list will repeat approximately once every 200 NCOM structure-A packets.
    /// </para>
    /// </summary>
    public abstract class StatusChannel : IMarshal
    {

        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel(byte statusChannelByte)
        {
            StatusChannelByte = statusChannelByte;
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        public int MarshalLength { get { return 8; } }

        public byte StatusChannelByte { get; private set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public virtual byte[] Marshal()
        {
            return new byte[MarshalLength];
        }

        public virtual bool Unmarshal(byte[] buffer, int offset)
        {
            return offset + MarshalLength < buffer.Length;
        }

    }
}
