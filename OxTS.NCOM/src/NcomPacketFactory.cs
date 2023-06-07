using OxTS.NCOM.Enumerations;
using System;
using System.Collections.Generic;

namespace OxTS.NCOM
{
    public class NcomPacketFactory
    {
        public List<NcomPacket> ProcessNcom(Span<byte> buffer)
        {
            // Create a list to store packets in
            List<NcomPacket> packets = new List<NcomPacket>();
            NcomPacket packet;

            // Iterate over the buffer to find sync bytes
            int p = 0;
            while (p <= buffer.Length - NcomPacket.PacketLength)
            {
                // Have we found a sync byte?
                if (buffer[p] == NcomPacket.SyncByte)
                {
                    // Test the navigation status byte (byte 21). If it is 11, parse as structure B, else
                    // structure A.
                    if (buffer[p + 21] == (byte)NavigationStatus.InternalUse)
                    {
                        packet = new NcomPacketB();
                    }
                    else
                    {
                        packet = new NcomPacketA();
                    }

                    ByteHandling.Unmarshal(buffer, ref p, packet);
                    packets.Add(packet);
                }
                else
                {
                    p++;
                }
            }

            // Return the list of processed Ncom packets
            return packets;
        }
    }
}
