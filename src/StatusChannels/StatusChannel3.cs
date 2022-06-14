using System;
using Ncom.Generators;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Position accuracies.
    /// </summary>
    [StatusChannel(3)]
    public partial class StatusChannel3 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid. 
        /// </summary>
        public const byte AgeValidThreshold = 150;

        private const byte InvalidABDRobotUMACInterfaceStatus = 0xFF;

        private ushort _northPositionAccuracy = ushort.MaxValue;
        private ushort _eastPositionAccuracy = ushort.MaxValue;
        private ushort _downPositionAccuracy = ushort.MaxValue;


        /// <summary>
        /// Gets or sets the north position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort NorthPositionAccuracy { get => _northPositionAccuracy; set => _northPositionAccuracy = value; }

        /// <summary>
        /// Gets or sets the east position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort EastPositionAccuracy { get => _eastPositionAccuracy; set => _eastPositionAccuracy = value; }

        /// <summary>
        /// Gets or sets the down position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort DownPositionAccuracy { get => _downPositionAccuracy; set => _downPositionAccuracy = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the ABD robot UMAC interface status byte.
        /// </summary>
        public byte ABDRobotUMACStatus { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="NorthPositionAccuracy"/>, 
        /// <see cref="EastPositionAccuracy"/> and <see cref="DownPositionAccuracy"/> values are 
        /// valid.
        /// </summary>
        public bool IsAccuracyValid => Age < AgeValidThreshold;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ABDRobotUMACStatus"/> value is valid.
        /// </summary>
        public bool IsABDRobotUMACStatusValid => ABDRobotUMACStatus != InvalidABDRobotUMACInterfaceStatus;


        /// <summary>
        /// Sets this <see cref="StatusChannel3"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel3"/> instance to copy.</param>
        public void Copy(StatusChannel3 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.NorthPositionAccuracy = source.NorthPositionAccuracy;
            this.EastPositionAccuracy = source.EastPositionAccuracy;
            this.DownPositionAccuracy = source.DownPositionAccuracy;
            this.Age = source.Age;
            this.ABDRobotUMACStatus = source.ABDRobotUMACStatus;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + NorthPositionAccuracy.GetHashCode();
            hash = (hash * mul) + EastPositionAccuracy.GetHashCode();
            hash = (hash * mul) + DownPositionAccuracy.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + ABDRobotUMACStatus;

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel3 other
                && this.NorthPositionAccuracy == other.NorthPositionAccuracy
                && this.EastPositionAccuracy == other.EastPositionAccuracy
                && this.DownPositionAccuracy == other.DownPositionAccuracy
                && this.Age == other.Age
                && this.ABDRobotUMACStatus == other.ABDRobotUMACStatus;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, NorthPositionAccuracy);
            ByteHandling.Marshal(buffer, ref offset, EastPositionAccuracy);
            ByteHandling.Marshal(buffer, ref offset, DownPositionAccuracy);

            buffer[offset++] = Age;
            buffer[offset++] = ABDRobotUMACStatus;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _northPositionAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _eastPositionAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _downPositionAccuracy);

            Age = buffer[offset++];
            ABDRobotUMACStatus = buffer[offset++];
        }

    }
}
