using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    public abstract class StatusChannel : IMarshal
    {

        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel(byte statusChannelByte)
        {
            StatusChannelByte = statusChannelByte;
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        public int MarshalLength { get { return 9; } }

        public byte StatusChannelByte { get; private set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public virtual byte[] Marshal()
        {
            byte[] buffer = new byte[MarshalLength];
            buffer[0] = StatusChannelByte;
            return buffer;
        }

        public virtual bool Unmarshal(byte[] buffer, int offset)
        {
            return offset + MarshalLength < buffer.Length;
        }

    }
}
