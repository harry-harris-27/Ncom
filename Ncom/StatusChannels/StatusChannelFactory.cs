using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Utility class for handling the creation and unmarshalling of NCOM <see cref="IStatusChannel"/>s.
    /// </summary>
    public static class StatusChannelFactory
    {
        /// <summary>
        /// Creates a <see cref="IStatusChannel"/> instance that represents the specified <paramref name="statusChannelByte"/>.
        /// </summary>
        /// <param name="statusChannelByte">The status channel byte of the <see cref="IStatusChannel"/> to create.</param>
        /// <returns></returns>
        public static IStatusChannel? Create(byte statusChannelByte) => statusChannelByte switch
        {
            0 => new StatusChannel0(),
            1 => new StatusChannel1(),
            2 => new StatusChannel2(),
            3 => new StatusChannel3(),
            4 => new StatusChannel4(),
            5 => new StatusChannel5(),
            6 => new StatusChannel6(),
            7 => new StatusChannel7(),
            8 => new StatusChannel8(),
            9 => new StatusChannel9(),

            // [96...255] reserved
            _ => null
        };
    }
}
