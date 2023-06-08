using OxTS.NCOM.Generators;
using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// Accelerometer Biases
    /// </summary>
    [StatusChannel(7)]
    public partial class NCOMStatusChannel7 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MeasurementsMask = 0b00001111;


        private short m_AccelerometerBiasX = 0;
        private short m_AccelerometerBiasY = 0;
        private short m_AccelerometerBiasZ = 0;


        /// <summary>
        /// Gets or sets the accelerometer X bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasX { get => m_AccelerometerBiasX; set => m_AccelerometerBiasX = value; }

        /// <summary>
        /// Gets or sets the accelerometer Y bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasY { get => m_AccelerometerBiasY; set => m_AccelerometerBiasY = value; }

        /// <summary>
        /// Gets or sets the accelerometer Z bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasZ { get => m_AccelerometerBiasZ; set => m_AccelerometerBiasZ = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the number of L1 GPS measurements decoded by the secondary receiver.
        /// </summary>
        public byte L1Measurements { get; set; }

        /// <summary>
        /// Gets or sets the number of L2 GPS measurements decoded by the secondary receiver.
        /// </summary>
        public byte L2Measurements { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="AccelerometerBiasX"/>, <see cref="AccelerometerBiasY"/>
        /// and <see cref="AccelerometerBiasZ"/> values are valid.
        /// </summary>
        public bool IsValid => Age < AgeValidThreshold;


        /// <inheritdoc/>
        protected override void Marshal(Span<byte> buffer)
        {
            ByteHandling.Marshal(buffer, AccelerometerBiasX);
            ByteHandling.Marshal(buffer, AccelerometerBiasY);
            ByteHandling.Marshal(buffer, AccelerometerBiasZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        protected override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasX);
            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasY);
            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }

    }
}
