using System;

namespace OxTS
{
    public static class Constants
    {
        /// <summary>
        /// GPS time epoch (6th January 1980).
        /// </summary>
        /// <remarks>
        /// Often used with an offset to determine an exact date/time.
        /// </remarks>
        public static readonly DateTime GPSTimeEpoch = new DateTime(1980, 1, 6);
    }
}
