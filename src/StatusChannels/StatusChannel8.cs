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

        private const byte MEASUREMENTS_MASK = 0b00001111;


        private short _gyroScaleFactorX = 0;
        private short _gyroScaleFactorY = 0;
        private short _gyroScaleFactorZ = 0;


        /// <summary>
        /// Gets or sets the X Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorX { get => _gyroScaleFactorX; set => _gyroScaleFactorX = value; }

        /// <summary>
        /// Gets or sets the y Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorY { get => _gyroScaleFactorY; set => _gyroScaleFactorY = value; }

        /// <summary>
        /// Gets or sets the Z Gyro scale factor, units 1 ppm (0.0001%).
        /// </summary>
        /// <seealso cref="IsValid"/>
        public short GyroScaleFactorZ { get => _gyroScaleFactorZ; set => _gyroScaleFactorZ = value; }

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


        /// <summary>
        /// Sets this <see cref="StatusChannel8"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel8"/> instance to copy.</param>
        public void Copy(StatusChannel8 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.GyroScaleFactorX = source.GyroScaleFactorX;
            this.GyroScaleFactorY = source.GyroScaleFactorY;
            this.GyroScaleFactorZ = source.GyroScaleFactorZ;
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

            hash = (hash * mul) + GyroScaleFactorX.GetHashCode();
            hash = (hash * mul) + GyroScaleFactorY.GetHashCode();
            hash = (hash * mul) + GyroScaleFactorZ.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + L1Measurements.GetHashCode();
            hash = (hash * mul) + L2Measurements.GetHashCode();

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel8 other
                && this.GyroScaleFactorX == other.GyroScaleFactorX
                && this.GyroScaleFactorY == other.GyroScaleFactorY
                && this.GyroScaleFactorZ == other.GyroScaleFactorZ
                && this.Age == other.Age
                && this.L1Measurements == other.L1Measurements
                && this.L2Measurements == other.L2Measurements;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, GyroScaleFactorX);
            ByteHandling.Marshal(buffer, ref offset, GyroScaleFactorY);
            ByteHandling.Marshal(buffer, ref offset, GyroScaleFactorZ);

            buffer[offset++] = Age;
            buffer[offset] = (byte)(L1Measurements & MEASUREMENTS_MASK);
            buffer[offset++] |= (byte)((L2Measurements & MEASUREMENTS_MASK) << 4);
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroScaleFactorX);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroScaleFactorY);
            ByteHandling.UnmarshalInt16(buffer, ref offset, ref _gyroScaleFactorZ);

            Age = buffer[offset++];
            L1Measurements = (byte)(buffer[offset] & MEASUREMENTS_MASK);
            L2Measurements = (byte)((buffer[offset] >> 4) & MEASUREMENTS_MASK);
        }

    }
}
