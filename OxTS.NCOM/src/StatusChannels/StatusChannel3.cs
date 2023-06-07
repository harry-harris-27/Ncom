using System;
using OxTS.NCOM.Generators;

namespace OxTS.NCOM.StatusChannels
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

        private ushort m_NorthPositionAccuracy = ushort.MaxValue;
        private ushort m_EastPositionAccuracy = ushort.MaxValue;
        private ushort m_DownPositionAccuracy = ushort.MaxValue;


        /// <summary>
        /// Gets or sets the north position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort NorthPositionAccuracy { get => m_NorthPositionAccuracy; set => m_NorthPositionAccuracy = value; }

        /// <summary>
        /// Gets or sets the east position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort EastPositionAccuracy { get => m_EastPositionAccuracy; set => m_EastPositionAccuracy = value; }

        /// <summary>
        /// Gets or sets the down position accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort DownPositionAccuracy { get => m_DownPositionAccuracy; set => m_DownPositionAccuracy = value; }

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


        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer.Slice(0), m_NorthPositionAccuracy);
            ByteHandling.Marshal(buffer.Slice(2), m_EastPositionAccuracy);
            ByteHandling.Marshal(buffer.Slice(4), m_DownPositionAccuracy);

            buffer[6] = Age;
            buffer[7] = ABDRobotUMACStatus;
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer.Slice(0), out m_NorthPositionAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(2), out m_EastPositionAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(4), out m_DownPositionAccuracy);

            Age = buffer[6];
            ABDRobotUMACStatus = buffer[7];
        }

    }
}
