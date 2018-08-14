using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    public static class StatusChannelFactory
    {

        private const string STATUS_CHANNEL_CLASS_PREFIX = "StatusChannel";


        public static StatusChannel ProcessStatusChannel(byte[] buffer, int offset)
        {
            // Get the status channel byte
            byte statusChannelByte = buffer[offset];

            // Try to create an instance of the status channel
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                object obj = assembly.CreateInstance(STATUS_CHANNEL_CLASS_PREFIX + statusChannelByte.ToString());
                StatusChannel channel = obj as StatusChannel;
                if (channel != null)
                {
                    channel.Unmarshal(buffer, offset + 1);
                    return channel;
                }
            }

            // Mostly likely cause for error is that the status channel class has not been implemented yet
            catch (Exception ex) { }

            return null;
        }

    }
}
