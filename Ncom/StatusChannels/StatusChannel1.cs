﻿using System;
using Ncom.Generators;

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
    [StatusChannel(1)]
    public partial class StatusChannel1 : StatusChannel
    {

        private const byte INNOVATION_VALIDITY_MASK = 0x01;

        private byte m_PositionXInnovation = 0;
        private byte m_PositionYInnovation = 0;
        private byte m_PositionZInnovation = 0;
        private byte m_VelocityXInnovation = 0;
        private byte m_VelocityYInnovation = 0;
        private byte m_VelocityZInnovation = 0;
        private byte m_OrientationPitchInnovation = 0;
        private byte m_OrientationHeadingInnovation = 0;
        private bool m_IsPositionXInnovationValid = false;
        private bool m_IsPositionYInnovationValid = false;
        private bool m_IsPositionZInnovationValid = false;
        private bool m_IsVelocityXInnovationValid = false;
        private bool m_IsVelocityYInnovationValid = false;
        private bool m_IsVelocityZInnovationValid = false;
        private bool m_IsOrientationPitchInnovationValid = false;
        private bool m_IsOrientationHeadingInnovationValid = false;


        /// <summary>
        /// Gets or sets the Position X innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionXInnovation { get => m_PositionXInnovation; set => m_PositionXInnovation = value; }

        /// <summary>
        /// Gets or sets the Position Y innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionYInnovation { get => m_PositionYInnovation; set => m_PositionYInnovation = value; }

        /// <summary>
        /// Gets or sets the Position Z innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte PositionZInnovation { get => m_PositionZInnovation; set => m_PositionZInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity X innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityXInnovation { get => m_VelocityXInnovation; set => m_VelocityXInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity Y innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityYInnovation { get => m_VelocityYInnovation; set => m_VelocityYInnovation = value; }

        /// <summary>
        /// Gets or sets the Velocity Z innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte VelocityZInnovation { get => m_VelocityZInnovation; set => m_VelocityZInnovation = value; }

        /// <summary>
        /// Gets or sets the orientation pitch innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte OrientationPitchInnovation { get => m_OrientationPitchInnovation; set => m_OrientationPitchInnovation = value; }

        /// <summary>
        /// Gets or sets the orientation heading innovation.
        /// </summary>
        /// <seealso cref="StatusChannel1"/>
        public byte OrientationHeadingInnovation { get => m_OrientationHeadingInnovation; set => m_OrientationHeadingInnovation = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionXInnovation"/> is valid.
        /// </summary>
        public bool IsPositionXInnovationValid { get => m_IsPositionXInnovationValid; set => m_IsPositionXInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionYInnovation"/> is valid.
        /// </summary>
        public bool IsPositionYInnovationValid { get => m_IsPositionYInnovationValid; set => m_IsPositionYInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PositionZInnovation"/> is valid.
        /// </summary>
        public bool IsPositionZInnovationValid { get => m_IsPositionZInnovationValid; set => m_IsPositionZInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityXInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityXInnovationValid { get => m_IsVelocityXInnovationValid; set => m_IsVelocityXInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityYInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityYInnovationValid { get => m_IsVelocityYInnovationValid; set => m_IsVelocityYInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VelocityZInnovation"/> is valid.
        /// </summary>
        public bool IsVelocityZInnovationValid { get => m_IsVelocityZInnovationValid; set => m_IsVelocityZInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OrientationPitchInnovation"/> is valid.
        /// </summary>
        public bool IsOrientationPitchInnovationValid { get => m_IsOrientationPitchInnovationValid; set => m_IsOrientationPitchInnovationValid = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OrientationHeadingInnovation"/> is valid.
        /// </summary>
        public bool IsOrientationHeadingInnovationValid { get => m_IsOrientationHeadingInnovationValid; set => m_IsOrientationHeadingInnovationValid = value; }


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
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            buffer[0] = MarshalInnonvation(PositionXInnovation, IsPositionXInnovationValid);
            buffer[1] = MarshalInnonvation(PositionYInnovation, IsPositionYInnovationValid);
            buffer[2] = MarshalInnonvation(PositionZInnovation, IsPositionZInnovationValid);
            buffer[3] = MarshalInnonvation(VelocityXInnovation, IsVelocityXInnovationValid);
            buffer[4] = MarshalInnonvation(VelocityYInnovation, IsVelocityYInnovationValid);
            buffer[5] = MarshalInnonvation(VelocityZInnovation, IsVelocityZInnovationValid);
            buffer[6] = MarshalInnonvation(OrientationPitchInnovation, IsOrientationPitchInnovationValid);
            buffer[7] = MarshalInnonvation(OrientationHeadingInnovation, IsOrientationHeadingInnovationValid);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            UnmarshalInnovation(buffer[0], ref m_PositionXInnovation, ref m_IsPositionXInnovationValid);
            UnmarshalInnovation(buffer[1], ref m_PositionYInnovation, ref m_IsPositionYInnovationValid);
            UnmarshalInnovation(buffer[2], ref m_PositionZInnovation, ref m_IsPositionZInnovationValid);
            UnmarshalInnovation(buffer[3], ref m_VelocityXInnovation, ref m_IsVelocityXInnovationValid);
            UnmarshalInnovation(buffer[4], ref m_VelocityYInnovation, ref m_IsVelocityYInnovationValid);
            UnmarshalInnovation(buffer[5], ref m_VelocityZInnovation, ref m_IsVelocityZInnovationValid);
            UnmarshalInnovation(buffer[6], ref m_OrientationPitchInnovation, ref m_IsOrientationPitchInnovationValid);
            UnmarshalInnovation(buffer[7], ref m_OrientationHeadingInnovation, ref m_IsOrientationHeadingInnovationValid);
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