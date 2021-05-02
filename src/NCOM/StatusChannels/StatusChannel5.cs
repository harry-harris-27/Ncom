using Ncom.Enumerations;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Orientation accuracies.
    /// </summary>
    public class StatusChannel5 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid. 
        /// </summary>
        public const byte AgeValidThreshold = 150;


        private ushort _headingAccuracy = ushort.MaxValue;
        private ushort _pitchAccuracy = ushort.MaxValue;
        private ushort _rollAccuracy = ushort.MaxValue;


        /// <summary>
        /// Initializes a new <see cref="StatusChannel5"/> instance.
        /// </summary>
        public StatusChannel5() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel5"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel5"/> instance to copy.</param>
        public StatusChannel5(StatusChannel5 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 5;

        /// <summary>
        /// Gets or sets the north velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort HeadingAccuracy { get => _headingAccuracy; set => _headingAccuracy = value; }

        /// <summary>
        /// Gets or sets the east velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort PitchAccuracy { get => _pitchAccuracy; set => _pitchAccuracy = value; }

        /// <summary>
        /// Gets or sets the down velocity accuracy, expressed in 1e-5 radians.
        /// </summary>
        /// <seealso cref="IsValid"/>
        public ushort RollAccuracy { get => _rollAccuracy; set => _rollAccuracy = value; }

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
        public override IStatusChannel Clone() => new StatusChannel5(this);

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
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel5 other
                && this.HeadingAccuracy == other.HeadingAccuracy
                && this.PitchAccuracy == other.PitchAccuracy
                && this.RollAccuracy == other.RollAccuracy
                && this.Age == other.Age;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, HeadingAccuracy);
            ByteHandling.Marshal(buffer, ref offset, PitchAccuracy);
            ByteHandling.Marshal(buffer, ref offset, RollAccuracy);

            buffer[offset++] = Age;
            buffer[offset++] = 0;       // Reserved
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _headingAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _pitchAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _rollAccuracy);

            Age = buffer[offset++];
            offset++;               // Reserved
        }

    }
}
