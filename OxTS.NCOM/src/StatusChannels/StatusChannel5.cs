using OxTS.NCOM.Generators;
using System;

namespace OxTS.NCOM.StatusChannels
{
    /// <summary>
    /// Orientation accuracies.
    /// </summary>
    [StatusChannel(5)]
    public partial class StatusChannel5 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;


        private ushort m_HeadingAccuracy = ushort.MaxValue;
        private ushort m_PitchAccuracy = ushort.MaxValue;
        private ushort m_RollAccuracy = ushort.MaxValue;


        /// <summary>
        /// Gets or sets the north velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort HeadingAccuracy { get => m_HeadingAccuracy; set => m_HeadingAccuracy = value; }

        /// <summary>
        /// Gets or sets the east velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort PitchAccuracy { get => m_PitchAccuracy; set => m_PitchAccuracy = value; }

        /// <summary>
        /// Gets or sets the down velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort RollAccuracy { get => m_RollAccuracy; set => m_RollAccuracy = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="HeadingAccuracy"/>,
        /// <see cref="PitchAccuracy"/> and <see cref="RollAccuracy"/> values are valid.
        /// </summary>
        public bool IsValid => Age < AgeValidThreshold;


        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer.Slice(0), HeadingAccuracy);
            ByteHandling.Marshal(buffer.Slice(2), PitchAccuracy);
            ByteHandling.Marshal(buffer.Slice(3), RollAccuracy);

            buffer[6] = Age;
            buffer[7] = 0;       // Reserved
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer.Slice(0), out m_HeadingAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(2), out m_PitchAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(3), out m_RollAccuracy);

            Age = buffer[6];
            // buffer[7] is reserved
        }

    }
}
