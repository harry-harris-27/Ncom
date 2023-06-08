using System;
using System.Collections.Generic;

namespace OxTS.NCOM
{
    public class NCOMPacketFactory
    {
        public List<NCOMPacket> ProcessNCOM(Span<byte> buffer)
        {
            // Create a list to store packets in
            List<NCOMPacket> packets = new List<NCOMPacket>();
            NCOMPacket packet;

            // Iterate over the buffer to find sync bytes
            int p = 0;
            while (p <= buffer.Length - NCOMPacket.PacketLength)
            {
                // Have we found a sync byte?
                if (buffer[p] == NCOMPacket.SyncByte)
                {
                    // Test the navigation status byte (byte 21). If it is 11, parse as structure B, else
                    // structure A.
                    if (buffer[p + 21] == (byte)NavigationStatus.InternalUse)
                    {
                        packet = new NCOMPacketB();
                    }
                    else
                    {
                        packet = new NCOMPacketA();
                    }

                    if (packet.TryUnmarshal(buffer, ref p, out _))
                    {
                        packets.Add(packet);
                    }
                }
                else
                {
                    p++;
                }
            }

            // Return the list of processed NCOM packets
            return packets;
        }
    }
}
