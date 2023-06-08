using OxTS.NCOM.Generators;
using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// Gyro Biases
    /// </summary>
    [StatusChannel(6)]
    public partial class NCOMStatusChannel6 : StatusChannel
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


        /// <inheritdoc/>
        protected override void Marshal(Span<byte> buffer)
        {
            ByteHandling.Marshal(buffer, GyroBiasX);
            ByteHandling.Marshal(buffer, GyroBiasY);
            ByteHandling.Marshal(buffer, GyroBiasZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        protected override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            ByteHandling.Unmarshal(buffer, out m_GyroBiasX);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasY);
            ByteHandling.Unmarshal(buffer, out m_GyroBiasZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }

    }
}
