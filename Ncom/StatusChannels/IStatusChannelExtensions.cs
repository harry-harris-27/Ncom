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
    }
}
