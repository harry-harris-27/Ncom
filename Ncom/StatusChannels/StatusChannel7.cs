using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Accelerometer Biases
    /// </summary>
    [StatusChannel(7)]
    public partial class StatusChannel7 : StatusChannel
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


        /// <summary>
        /// Sets this <see cref="StatusChannel7"/> instance logically equal to the specified
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel7"/> instance to copy.</param>
        public void Copy(StatusChannel7 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.AccelerometerBiasX = source.AccelerometerBiasX;
            this.AccelerometerBiasY = source.AccelerometerBiasY;
            this.AccelerometerBiasZ = source.AccelerometerBiasZ;
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

            hash = (hash * mul) + AccelerometerBiasX.GetHashCode();
            hash = (hash * mul) + AccelerometerBiasY.GetHashCode();
            hash = (hash * mul) + AccelerometerBiasZ.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + L1Measurements.GetHashCode();
            hash = (hash * mul) + L2Measurements.GetHashCode();

            return hash;
        }

        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer, AccelerometerBiasX);
            ByteHandling.Marshal(buffer, AccelerometerBiasY);
            ByteHandling.Marshal(buffer, AccelerometerBiasZ);

            buffer[6] = Age;
            buffer[7] = (byte)(L1Measurements & MeasurementsMask);
            buffer[7] |= (byte)((L2Measurements & MeasurementsMask) << 4);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasX);
            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasY);
            ByteHandling.Unmarshal(buffer, out m_AccelerometerBiasZ);

            Age = buffer[6];
            L1Measurements = (byte)(buffer[7] & MeasurementsMask);
            L2Measurements = (byte)((buffer[7] >> 4) & MeasurementsMask);
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel7 other
                && this.AccelerometerBiasX == other.AccelerometerBiasX
                && this.AccelerometerBiasY == other.AccelerometerBiasY
                && this.AccelerometerBiasZ == other.AccelerometerBiasZ
                && this.Age == other.Age
                && this.L1Measurements == other.L1Measurements
                && this.L2Measurements == other.L2Measurements;
        }

    }
}
