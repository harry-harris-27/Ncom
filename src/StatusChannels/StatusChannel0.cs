using Ncom.Enumerations;
using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Full time, number of satellites, position mode, velocity mode, dual antenna mode. 
    /// </summary>
    [StatusChannel(0)]
    public partial class StatusChannel0 : StatusChannel
    {
        /// <summary>
        /// Gets or sets the time in minutes since GPS began (midnight, 6th January 1980)
        /// </summary>
        public int FullTime { get; set; } = 0;

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


        /// <summary>
        /// Sets this <see cref="StatusChannel0"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel0"/> instance to copy.</param>
        public void Copy(StatusChannel0 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            FullTime = source.FullTime;
            NumberOfSatellites = source.NumberOfSatellites;
            PositionMode = source.PositionMode;
            VelocityMode = source.VelocityMode;
            OrientationMode = source.OrientationMode;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + FullTime.GetHashCode();
            hash = (hash * mul) + NumberOfSatellites;
            hash = (hash * mul) + (byte)PositionMode;
            hash = (hash * mul) + (byte)VelocityMode;
            hash = (hash * mul) + (byte)OrientationMode;

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel0 other
                && this.FullTime == other.FullTime
                && this.NumberOfSatellites == other.NumberOfSatellites
                && this.PositionMode == other.PositionMode
                && this.VelocityMode == other.VelocityMode
                && this.OrientationMode == other.OrientationMode;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base class StatusChannel guarentees that method arguments are non-null")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            Array.Copy(BitConverter.GetBytes(FullTime), 0, buffer, offset, 4);
            offset += 4;

            buffer[offset++] = NumberOfSatellites;
            buffer[offset++] = (byte)PositionMode;
            buffer[offset++] = (byte)VelocityMode;
            buffer[offset++] = (byte)OrientationMode;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base class StatusChannel guarentees that method arguments are non-null")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            FullTime = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            NumberOfSatellites = buffer[offset++];
            PositionMode = ByteHandling.ParseEnum(buffer[offset++], PositionVelocityOrientationMode.Unknown);
            VelocityMode = ByteHandling.ParseEnum(buffer[offset++], PositionVelocityOrientationMode.Unknown);
            OrientationMode = ByteHandling.ParseEnum(buffer[offset++], PositionVelocityOrientationMode.Unknown);
        }

    }
}
