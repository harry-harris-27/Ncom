using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Utilities
{
    internal static class ByteHandling
    {

        static public int Cast3BytesToInt32(byte[] buffer, int offset)
        {
            int val = 0;
            
            val = buffer[offset + 2] << 24;
            val |= buffer[offset + 1] << 16;
            val |= buffer[offset] << 8;

            return val >> 8;
        }

        static public byte[] CastInt32To3Bytes(int val)
        {
            byte[] result = new byte[3];

            val <<= 8;

            byte[] convert = BitConverter.GetBytes(val);
            result[0] = convert[1];
            result[1] = convert[2];
            result[2] = convert[3];

            return result;
        }
        

    }
}
