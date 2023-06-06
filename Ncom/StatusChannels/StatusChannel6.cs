using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Gyro Biases
    /// </summary>
    [StatusChannel(6)]
    public partial class StatusChannel6 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MeasurementsMask = 0b00001111;


        private short m_GyroBiasX = 0;
        private short m_GyroBiasY = 0;
        private short m_GyroBiasZ = 0;


        /// <summary>
        /// Gets or sets the gyro X bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasX { get => m_GyroBiasX; set => m_GyroBiasX = value; }

        /// <summary>
        /// Gets or sets the gyro Y bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasY { get => m_GyroBiasY; set => m_GyroBiasY = value; }

        /// <summary>
        /// Gets or sets the gyro Z bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasZ { get => m_GyroBiasZ; set => m_GyroBiasZ = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the number of L1 GPS measurements decoded by the primary receiver.
        /// </summary>
        public byte L1Measurements { get; set; }

        /// <summary>
        /// Gets or sets the number of L2 GPS measurements decoded by the primary receiver.
        /// </summary>
        public byte L2Measurements { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="GyroBiasX"/>, <see cref="GyroBiasY"/>
        /// and <see cref="GyroBiasZ"/> values are valid.
        /// </summary>
        public bool IsValid => Age < AgeValidThreshold;


        /// <summary>
        /// Sets this <see cref="StatusChannel6"/> instance logically equal to the specified
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel6"/> instance to copy.</param>
        public void Copy(StatusChannel6 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.GyroBiasX = source.GyroBiasX;
            this.GyroBiasY = source.GyroBiasY;
            this.GyroBiasZ = source.GyroBiasZ;
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

            hash = (hash * mul) + GyroBiasX.GetHashCode();
            hash = (hash * mul) + GyroBiasY.GetHashCode();
            hash = (hash * mul) + GyroBiasZ.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + L1Measurements.GetHashCode();
            hash = (hash * mul) + L2Measurements.GetHashCode();

            return hash;
        }

        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer, GyroBiasX);
            ByteHandling.Marshal(buffer, GyroBiasY);
            ByteHandling.Marshal(buffer, GyroBiasZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer, out m_GyroBiasX);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasY);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel6 other
                && this.GyroBiasX == other.GyroBiasX
                && this.GyroBiasY == other.GyroBiasY
                && this.GyroBiasZ == other.GyroBiasZ
                && this.Age == other.Age
                && this.L1Measurements == other.L1Measurements
                && this.L2Measurements == other.L2Measurements;
        }

    }
}
