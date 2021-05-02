using System;

namespace Ncom.StatusChannels
{
    public static class IStatusChannelExtensions
    {
        /// <summary>
        /// Creates a new instance of the <typeparamref name="TStatusChannel"/> logically equal to this 
        /// instance.
        /// </summary>
        /// <typeparam name="TStatusChannel">The status channel type.</typeparam>
        /// <param name="statusChannel"></param>
        /// <returns>A <typeparamref name="TStatusChannel"/> instance</returns>
        public static TStatusChannel Clone<TStatusChannel>(this TStatusChannel statusChannel)
            where TStatusChannel : class, IStatusChannel
        {
            return (TStatusChannel)statusChannel.Clone();
        }


        public static void Marshal(this IStatusChannel statusChannel, byte[] buffer, ref int offset)
        {
            statusChannel.Marshal(buffer, offset);
            offset += NcomPacketA.StatusChannelLength;
        }

        public static void Unmarshal(this IStatusChannel statusChannel, byte[] buffer, ref int offset)
        {
            statusChannel.Unmarshal(buffer, offset);
            offset += NcomPacketA.StatusChannelLength;
        }
    }
}
