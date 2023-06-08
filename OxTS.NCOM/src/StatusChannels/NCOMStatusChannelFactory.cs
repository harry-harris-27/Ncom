using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// Utility class for handling the creation and unmarshalling of NCOM <see cref="IStatusChannel"/>s.
    /// </summary>
    public static class NCOMStatusChannelFactory
    {
        /// <summary>
        /// Creates a <see cref="IStatusChannel"/> instance that represents the specified <paramref name="statusChannelByte"/>.
        /// </summary>
        /// <param name="statusChannelByte">The status channel byte of the <see cref="IStatusChannel"/> to create.</param>
        /// <returns></returns>
        public static IStatusChannel? Create(byte statusChannelByte) => statusChannelByte switch
        {
            0 => new NCOMStatusChannel0(),
            1 => new NCOMStatusChannel1(),
            2 => new NCOMStatusChannel2(),
            3 => new NCOMStatusChannel3(),
            4 => new NCOMStatusChannel4(),
            5 => new NCOMStatusChannel5(),
            6 => new NCOMStatusChannel6(),
            7 => new NCOMStatusChannel7(),
            8 => new NCOMStatusChannel8(),
            9 => new NCOMStatusChannel9(),

            // [96...255] reserved
            _ => null
        };
    }
}
