using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Kalman filter innovations set 1 (position, velocity, attitude). 
    /// </summary>
    /// <remarks>
    /// <para>
    /// The innovations are always expressed as a proportion of the current accuracy; units 0.1. As 
    /// a general rule, innovations below 1.0 are good; innovations above 1.0 are poor. Usually it 
    /// is best to filter the square of the innovations and display the square root of the filtered 
    /// value.
    /// </para>
    /// <para>
    /// If the <see cref="OrientationPitchInnovation"/> and/or the <see cref="OrientationHeadingInnovation"/>
    /// are always much higher than 1.0 then it is likely that the system or the antennas have 
    /// changed orientation in the vehicle (Or the enironment is too poor to use dual antenna 
    /// systems).
    /// </para>
    /// </remarks>
    public class StatusChannel1 : StatusChannel
    {

        private const byte INNOVATION_VALIDITY_MASK = 0x01;

        private byte _positionXInnovation = 0;
        private byte _positionYInnovation = 0;
        private byte _positionZInnovation = 0;
        private byte _velocityXInnovation = 0;
        private byte _velocityYInnovation = 0;
        private byte _velocityZInnovation = 0;
        private byte _orientationPitchInnovation = 0;
        private byte _orientationHeadingInnovation = 0;
        private bool _isPositionXInnovationValid = false;
        private bool _isPositionYInnovationValid = false;
        private bool _isPositionZInnovationValid = false;
        private bool _isVelocityXInnovationValid = false;
        private bool _isVelocityYInnovationValid = false;
        private bool _isVelocityZInnovationValid = false;
        private bool _isOrientationPitchInnovationValid = false;
        private bool _isOrientationHeadingInnovationValid = false;


        /// <summary>
        /// Initializes a new <see cref="StatusChannel1"/> instance.
        /// </summary>
        public StatusChannel1() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel1"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel1"/> instance to copy.</param>
        public StatusChannel1(StatusChannel1 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 1;

        /// <summary>
        /// Gets or sets the Position X innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionXInnovation { get => _positionXInnovation; set => _positionXInnovation = value; }

        /// <summary>
        /// Gets or sets the Position Y innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionYInnovation { get => _positionYInnovation; set => _positionYInnovation = value; }

        /// <summary>
        /// Gets or sets the Position Z innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionZInnovation { get => _positionZInnovation; set => _positionZInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity X innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityXInnovation { get => _velocityXInnovation; set => _velocityXInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity Y innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityYInnovation { get => _velocityYInnovation; set => _velocityYInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity Z innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityZInnovation { get => _velocityZInnovation; set => _velocityZInnovation = value; }

        /// <summary>
        /// Gets or sets the orientation pitch innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte OrientationPitchInnovation { get => _orientationPitchInnovation; set => _orientationPitchInnovation = value; }

        /// <summary>
        /// Gets or sets the orientation heading innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte OrientationHeadingInnovation { get => _orientationHeadingInnovation; set => _orientationHeadingInnovation = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionXInnovation"/> is valid.
        /// </summary>
        public bool IsPositionXInnovationValid { get => _isPositionXInnovationValid; set => _isPositionXInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionYInnovation"/> is valid.
        /// </summary>
        public bool IsPositionYInnovationValid { get => _isPositionYInnovationValid; set => _isPositionYInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionZInnovation"/> is valid.
        /// </summary>
        public bool IsPositionZInnovationValid { get => _isPositionZInnovationValid; set => _isPositionZInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityXInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityXInnovationValid { get => _isVelocityXInnovationValid; set => _isVelocityXInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityYInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityYInnovationValid { get => _isVelocityYInnovationValid; set => _isVelocityYInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityZInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityZInnovationValid { get => _isVelocityZInnovationValid; set => _isVelocityZInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OrientationPitchInnovation"/> is valid.
        /// </summary>
        public bool IsOrientationPitchInnovationValid { get => _isOrientationPitchInnovationValid; set => _isOrientationPitchInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OrientationHeadingInnovation"/> is valid.
        /// </summary>
        public bool IsOrientationHeadingInnovationValid { get => _isOrientationHeadingInnovationValid; set => _isOrientationHeadingInnovationValid = value; }


        /// <inheritdoc/>
        public override IStatusChannel Clone() => new StatusChannel1(this);

        /// <summary>
        /// Sets this <see cref="StatusChannel1"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel1"/> instance to copy.</param>
        public void Copy(StatusChannel1 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            PositionXInnovation = source.PositionXInnovation;
            PositionYInnovation = source.PositionYInnovation;
            PositionZInnovation = source.PositionZInnovation;
            VelocityXInnovation = source.VelocityXInnovation;
            VelocityYInnovation = source.VelocityYInnovation;
            VelocityZInnovation = source.VelocityZInnovation;
            OrientationPitchInnovation = source.OrientationPitchInnovation;
            OrientationHeadingInnovation = source.OrientationHeadingInnovation;
            IsPositionXInnovationValid = source.IsPositionXInnovationValid;
            IsPositionYInnovationValid = source.IsPositionYInnovationValid;
            IsPositionZInnovationValid = source.IsPositionZInnovationValid;
            IsVelocityXInnovationValid = source.IsVelocityXInnovationValid;
            IsVelocityYInnovationValid = source.IsVelocityYInnovationValid;
            IsVelocityZInnovationValid = source.IsVelocityZInnovationValid;
            IsOrientationPitchInnovationValid = source.IsOrientationPitchInnovationValid;
            IsOrientationHeadingInnovationValid = source.IsOrientationHeadingInnovationValid;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + PositionXInnovation;
            hash = (hash * mul) + PositionYInnovation;
            hash = (hash * mul) + PositionZInnovation;
            hash = (hash * mul) + VelocityXInnovation;
            hash = (hash * mul) + VelocityYInnovation;
            hash = (hash * mul) + VelocityZInnovation;
            hash = (hash * mul) + OrientationHeadingInnovation;
            hash = (hash * mul) + OrientationPitchInnovation;
            hash = (hash * mul) + (IsPositionXInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsPositionYInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsPositionZInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsVelocityXInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsVelocityYInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsVelocityZInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsOrientationHeadingInnovationValid ? 1 : 0);
            hash = (hash * mul) + (IsOrientationPitchInnovationValid ? 1 : 0);

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel1 other
                && this.PositionXInnovation == other.PositionXInnovation
                && this.PositionYInnovation == other.PositionYInnovation
                && this.PositionZInnovation == other.PositionZInnovation
                && this.VelocityXInnovation == other.VelocityXInnovation
                && this.VelocityYInnovation == other.VelocityYInnovation
                && this.VelocityZInnovation == other.VelocityZInnovation
                && this.OrientationHeadingInnovation == other.OrientationHeadingInnovation
                && this.OrientationPitchInnovation == other.OrientationPitchInnovation
                && this.IsPositionXInnovationValid == other.IsPositionXInnovationValid
                && this.IsPositionYInnovationValid == other.IsPositionYInnovationValid
                && this.IsPositionZInnovationValid == other.IsPositionZInnovationValid
                && this.IsVelocityXInnovationValid == other.IsVelocityXInnovationValid
                && this.IsVelocityYInnovationValid == other.IsVelocityYInnovationValid
                && this.IsVelocityZInnovationValid == other.IsVelocityZInnovationValid
                && this.IsOrientationHeadingInnovationValid == other.IsOrientationHeadingInnovationValid
                && this.IsOrientationPitchInnovationValid == other.IsOrientationPitchInnovationValid;

        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base class StatusChannel guarentees that method arguments are non-null")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            buffer[offset++] = MarshalInnonvation(PositionXInnovation, IsPositionXInnovationValid);
            buffer[offset++] = MarshalInnonvation(PositionYInnovation, IsPositionYInnovationValid);
            buffer[offset++] = MarshalInnonvation(PositionZInnovation, IsPositionZInnovationValid);
            buffer[offset++] = MarshalInnonvation(VelocityXInnovation, IsVelocityXInnovationValid);
            buffer[offset++] = MarshalInnonvation(VelocityYInnovation, IsVelocityYInnovationValid);
            buffer[offset++] = MarshalInnonvation(VelocityZInnovation, IsVelocityZInnovationValid);
            buffer[offset++] = MarshalInnonvation(OrientationPitchInnovation, IsOrientationPitchInnovationValid);
            buffer[offset++] = MarshalInnonvation(OrientationHeadingInnovation, IsOrientationHeadingInnovationValid);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base class StatusChannel guarentees that method arguments are non-null")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            UnmarshalInnovation(buffer[offset++], ref _positionXInnovation, ref _isPositionXInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _positionYInnovation, ref _isPositionYInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _positionZInnovation, ref _isPositionZInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _velocityXInnovation, ref _isVelocityXInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _velocityYInnovation, ref _isVelocityYInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _velocityZInnovation, ref _isVelocityZInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _orientationPitchInnovation, ref _isOrientationPitchInnovationValid);
            UnmarshalInnovation(buffer[offset++], ref _orientationHeadingInnovation, ref _isOrientationHeadingInnovationValid);
        }


        private static byte MarshalInnonvation(byte innovation, bool isInnovationValid)
        {
            return (byte)((innovation << 1) | (isInnovationValid ? 0x01 : 0x00));
        }

        private static void UnmarshalInnovation(byte value, ref byte innovationStorage, ref bool validityStorage)
        {
            innovationStorage = (byte)((value & ~INNOVATION_VALIDITY_MASK) >> 1);
            validityStorage = (value & INNOVATION_VALIDITY_MASK) == INNOVATION_VALIDITY_MASK;
        }
    }
}
