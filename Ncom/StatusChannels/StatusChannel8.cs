using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Gyro scale factors
    /// </summary>
    [StatusChannel(8)]
    public partial class StatusChannel8 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MeasurementsMask = 0b00001111;


        private short m_GyroScaleFactorX = 0;
        private short m_GyroScaleFactorY = 0;
        private short m_GyroScaleFactorZ = 0;


        /// <summary>
        /// Gets or sets the X Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the y Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorY { get => m_GyroScaleFactorY; set => m_GyroScaleFactorY = value; }

        /// <summary>
        /// Gets or sets the Z Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorZ { get => m_GyroScaleFactorZ; set => m_GyroScaleFactorZ = value; }

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
        /// Gets a value indicating whether the  <see cref="GyroScaleFactorX"/>, <see cref="GyroScaleFactorY"/>
        /// and <see cref="GyroScaleFactorZ"/> values are valid.
        /// </summary>
        public bool IsValid => Age < AgeValidThreshold;


        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer, GyroScaleFactorX);
            ByteHandling.Marshal(buffer, GyroScaleFactorY);
            ByteHandling.Marshal(buffer, GyroScaleFactorZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer, out m_GyroScaleFactorX);
            ByteHandling.Unmarshal(buffer, out m_GyroScaleFactorY);
            ByteHandling.Unmarshal(buffer, out m_GyroScaleFactorZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }

    }
}
