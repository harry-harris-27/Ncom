using Ncom.Enumerations;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Accelerometer Biases
    /// </summary>
    public class StatusChannel7 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid. 
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MEASUREMENTS_MASK = 0b00001111;


        private short _accelerometerBiasX = 0;
        private short _accelerometerBiasY = 0;
        private short _accelerometerBiasZ = 0;


        /// <summary>
        /// Initializes a new <see cref="StatusChannel7"/> instance.
        /// </summary>
        public StatusChannel7() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel7"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel7"/> instance to copy.</param>
        public StatusChannel7(StatusChannel7 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 7;

        /// <summary>
        /// Gets or sets the accelerometer X bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasX { get => _accelerometerBiasX; set => _accelerometerBiasX = value; }

        /// <summary>
        /// Gets or sets the accelerometer Y bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasY { get => _accelerometerBiasY; set => _accelerometerBiasY = value; }

        /// <summary>
        /// Gets or sets the accelerometer Z bias, units 0.1 mm^-2.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short AccelerometerBiasZ { get => _accelerometerBiasZ; set => _accelerometerBiasZ = value; }

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
        public override IStatusChannel Clone() => new StatusChannel7(this);

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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, AccelerometerBiasX);
            ByteHandling.Marshal(buffer, ref offset, AccelerometerBiasY);
            ByteHandling.Marshal(buffer, ref offset, AccelerometerBiasZ);

            buffer[offset++] = Age;
            buffer[offset] = (byte)(L1Measurements & MEASUREMENTS_MASK);
            buffer[offset++] |= (byte)((L2Measurements & MEASUREMENTS_MASK) << 4);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _accelerometerBiasX);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _accelerometerBiasY);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _accelerometerBiasZ);

            Age = buffer[offset++];
            L1Measurements = (byte)(buffer[offset] & MEASUREMENTS_MASK);
            L2Measurements = (byte)((buffer[offset] >> 4) & MEASUREMENTS_MASK);
        }

    }
}
