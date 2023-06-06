using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
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


        /// <summary>
        /// Sets this <see cref="StatusChannel5"/> instance logically equal to the specified
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel5"/> instance to copy.</param>
        public void Copy(StatusChannel5 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.HeadingAccuracy = source.HeadingAccuracy;
            this.PitchAccuracy = source.PitchAccuracy;
            this.RollAccuracy = source.RollAccuracy;
            this.Age = source.Age;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + HeadingAccuracy.GetHashCode();
            hash = (hash * mul) + PitchAccuracy.GetHashCode();
            hash = (hash * mul) + RollAccuracy.GetHashCode();
            hash = (hash * mul) + Age;

            return hash;
        }

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


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel5 other
                && this.HeadingAccuracy == other.HeadingAccuracy
                && this.PitchAccuracy == other.PitchAccuracy
                && this.RollAccuracy == other.RollAccuracy
                && this.Age == other.Age;
        }

    }
}
