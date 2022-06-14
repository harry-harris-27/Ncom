using System;
using System.Collections.Generic;
using System.Linq;

namespace Ncom.StatusChannels
{
    public static class StatusChannelFactory
    {

        private static readonly IDictionary<byte, IStatusChannel> statusChannelLookup = new IStatusChannel[]
        {
            new StatusChannel0(),
            new StatusChannel1(),
            new StatusChannel2(),
            new StatusChannel3(),
            new StatusChannel4(),
            new StatusChannel5(),
            new StatusChannel6(),
            new StatusChannel7(),
            new StatusChannel8(),
        }.ToDictionary(x => x.StatusChannelByte);


        public static IStatusChannel Unmarshal(byte statusChannelByte, byte[] buffer, int offset = 0)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            // Check the length of the available part of the buffer
            if (buffer.Length - offset < NcomPacketA.StatusChannelLength)
                throw new ArgumentException("Buffer is smaller than required");

            // Create the status channel
            if (statusChannelLookup.TryGetValue(statusChannelByte, out IStatusChannel statusChannel))
            {
                statusChannel.Unmarshal(buffer, offset);
            }

            // Return the status channel object
            return statusChannel?.Clone();
        }

        public static IStatusChannel Unmarshal(byte statusChannelByte, byte[] buffer, ref int offset)
        {
            var statusChannel = Unmarshal(statusChannelByte, buffer, offset);
            offset += NcomPacketA.StatusChannelLength;
            return statusChannel;
        }
        
    }
}
