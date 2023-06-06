using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Gyro bias accuracy
    /// </summary>
    [StatusChannel(9)]
    public partial class StatusChannel9 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MeasurementsMask = 0b00001111;


        private short m_GyroBiasAccuracyX = 0;
        private short m_GyroBiasAccuracyY = 0;
        private short m_GyroBiasAccuracyZ = 0;


        /// <summary>
        /// Gets or sets the X Gyro bias accuracy factor, units 1e-6 radians/s.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasAccuracyX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Y Gyro bias accuracy factor, units 1e-6 radians/s.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasAccuracyY { get => m_GyroBiasAccuracyY; set => m_GyroBiasAccuracyY = value; }

        /// <summary>
        /// Gets or sets the Z Gyro bias accuracy factor, units 1e-6 radians/s.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasAccuracyZ { get => m_GyroBiasAccuracyZ; set => m_GyroBiasAccuracyZ = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the number of L1 GPS measurements decoded by the external receiver.
        /// </summary>
        public byte L1Measurements { get; set; }

        /// <summary>
        /// Gets or sets the number of L2 GPS measurements decoded by the external receiver.
        /// </summary>
        public byte L2Measurements { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="GyroBiasAccuracyX"/>, <see cref="GyroBiasAccuracyY"/>
        /// and <see cref="GyroBiasAccuracyZ"/> values are valid.
        /// </summary>
        public bool IsValid => Age < AgeValidThreshold;


        /// <summary>
        /// Sets this <see cref="StatusChannel8"/> instance logically equal to the specified
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel8"/> instance to copy.</param>
        public void Copy(StatusChannel9 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.GyroBiasAccuracyX = source.GyroBiasAccuracyX;
            this.GyroBiasAccuracyY = source.GyroBiasAccuracyY;
            this.GyroBiasAccuracyZ = source.GyroBiasAccuracyZ;
            this.Age = source.Age;
            this.L1Measurements = source.L1Measurements;
            this.L2Measurements = source.L2Measurements;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + GyroBiasAccuracyX.GetHashCode();
            hash = (hash * mul) + GyroBiasAccuracyY.GetHashCode();
            hash = (hash * mul) + GyroBiasAccuracyZ.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + L1Measurements.GetHashCode();
            hash = (hash * mul) + L2Measurements.GetHashCode();

            return hash;
        }

        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer, GyroBiasAccuracyX);
            ByteHandling.Marshal(buffer, GyroBiasAccuracyY);
            ByteHandling.Marshal(buffer, GyroBiasAccuracyZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer, out m_GyroBiasAccuracyX);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasAccuracyY);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasAccuracyZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel9 other
                && this.GyroBiasAccuracyX == other.GyroBiasAccuracyX
                && this.GyroBiasAccuracyY == other.GyroBiasAccuracyY
                && this.GyroBiasAccuracyZ == other.GyroBiasAccuracyZ
                && this.Age == other.Age
                && this.L1Measurements == other.L1Measurements
                && this.L2Measurements == other.L2Measurements;
        }

    }
}
