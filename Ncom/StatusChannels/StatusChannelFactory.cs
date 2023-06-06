using System;

namespace Ncom.StatusChannels
{
    public static class StatusChannelFactory
    {
        public static IStatusChannel? Unmarshal(byte statusChannelByte, ReadOnlySpan<byte> buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // Check the length of the available part of the buffer
            if (buffer.Length < StatusChannel.StatusChannelLength)
            {
                throw new ArgumentException("Buffer is smaller than required");
            }

            // Create the status channel
            var statusChannel = CreateStatusChannelInstance(statusChannelByte);
            statusChannel?.Unmarshal(buffer);

            // Return the status channel object
            return statusChannel;
        }


        private static IStatusChannel? CreateStatusChannelInstance(byte statusChannelByte)
        {
            if (StatusChannel.TryGetStatusChannelType(statusChannelByte, out Type statusChannelType))
            {
                return (IStatusChannel)Activator.CreateInstance(statusChannelType);
            }

            //throw new NotImplementedException();
            return null;
        }
    }
}
