using OxTS.NCOM.Generators;
using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// Full time, number of satellites, position mode, velocity mode, dual antenna mode.
    /// </summary>
    [StatusChannel(0)]
    public partial class NCOMStatusChannel0 : StatusChannel
    {

        private int m_FullTime = 0;


        /// <summary>
        /// Gets or sets the time in minutes since GPS began (midnight, 6th January 1980)
        /// </summary>
        public int FullTime { get => m_FullTime; set => m_FullTime = value; }

        /// <summary>
        /// Gets or sets the number of GPS satellites tracked by the main GNSS receiver
        /// </summary>
        public byte NumberOfSatellites { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Position mode of the main GNSS.
        /// </summary>
        public PositionVelocityOrientationMode PositionMode { get; set; } = PositionVelocityOrientationMode.None;

        /// <summary>
        /// Gets or sets the Velocity mode of the main GNSS.
        /// </summary>
        public PositionVelocityOrientationMode VelocityMode { get; set; } = PositionVelocityOrientationMode.None;

        /// <summary>
        /// Gets or sets the Orientation mode of dual antenna systems.
        /// </summary>
        public PositionVelocityOrientationMode OrientationMode { get; set; } = PositionVelocityOrientationMode.None;


        /// <inheritdoc/>
        protected override void Marshal(Span<byte> buffer)
        {
            ByteHandling.Marshal(buffer, m_FullTime);
            buffer[4] = NumberOfSatellites;
            buffer[5] = (byte)PositionMode;
            buffer[6] = (byte)VelocityMode;
            buffer[7] = (byte)OrientationMode;
        }

        /// <inheritdoc/>
        protected override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            ByteHandling.Unmarshal(buffer, out m_FullTime);
            NumberOfSatellites = buffer[4];
            PositionMode = ByteHandling.ParseEnum(buffer[5], PositionVelocityOrientationMode.Unknown);
            VelocityMode = ByteHandling.ParseEnum(buffer[6], PositionVelocityOrientationMode.Unknown);
            OrientationMode = ByteHandling.ParseEnum(buffer[7], PositionVelocityOrientationMode.Unknown);
        }

    }
}
