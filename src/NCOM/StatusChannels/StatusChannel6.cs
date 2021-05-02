using Ncom.Enumerations;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Gyro Biases
    /// </summary>
    public class StatusChannel6 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid. 
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte MEASUREMENTS_MASK = 0b00001111;


        private short _gyroBiasX = 0;
        private short _gyroBiasY = 0;
        private short _gyroBiasZ = 0;


        /// <summary>
        /// Initializes a new <see cref="StatusChannel6"/> instance.
        /// </summary>
        public StatusChannel6() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel6"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel6"/> instance to copy.</param>
        public StatusChannel6(StatusChannel6 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 6;

        /// <summary>
        /// Gets or sets the gyro X bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasX { get => _gyroBiasX; set => _gyroBiasX = value; }

        /// <summary>
        /// Gets or sets the gyro Y bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasY { get => _gyroBiasY; set => _gyroBiasY = value; }

        /// <summary>
        /// Gets or sets the gyro Z bias, expressed in 1e-6 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroBiasZ { get => _gyroBiasZ; set => _gyroBiasZ = value; }

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
        public override IStatusChannel Clone() => new StatusChannel6(this);

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

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, GyroBiasX);
            ByteHandling.Marshal(buffer, ref offset, GyroBiasY);
            ByteHandling.Marshal(buffer, ref offset, GyroBiasZ);

            buffer[offset++] = Age;
            buffer[offset] = (byte)(L1Measurements & MEASUREMENTS_MASK);
            buffer[offset++] |= (byte)((L2Measurements & MEASUREMENTS_MASK) << 4);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroBiasX);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroBiasY);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroBiasZ);

            Age = buffer[offset++];
            L1Measurements = (byte)(buffer[offset] & MEASUREMENTS_MASK);
            L2Measurements = (byte)((buffer[offset] >> 4) & MEASUREMENTS_MASK);
        }

    }
}
