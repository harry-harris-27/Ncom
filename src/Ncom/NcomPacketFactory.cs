using NCOM.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM
{
    public class NcomPacketFactory
    {

        /* ---------- Public methods ----------------------------------------------------------/**/

        public virtual List<NcomPacket> ProcessNcom(byte[] buffer)
        {
            // Create a list to store packets in
            List<NcomPacket> pkts = new List<NcomPacket>();
            NcomPacket pkt;

            // Iterate over the buffer to find sync bytes
            int p = 0;
            while (p <= buffer.Length - NcomPacket.PacketLength)
            {
                // Have we found a sync byte?
                if (buffer[p++] == NcomPacket.SyncByte)
                { 
                    // Test the navigation status byte (byte 21). If it is 11, parse as structure B, else 
                    // structure A.
                    if (buffer[(p - 1) + 21] == (byte)NavigationStatus.InternalUse)
                    {
                        pkt = new NcomPacketB();
                    }
                    else
                    {
                        pkt = new NcomPacketA();
                    }

                    // Try to unmarshal the Ncom packet
                    if (pkt.Unmarshal(buffer, p - 1))
                    {
                        pkts.Add(pkt);
                        p += NcomPacket.PacketLength - 1;
                    }
                }
            }

            // Return the list of processed Ncom packets
            return pkts;
        }

    }
}
