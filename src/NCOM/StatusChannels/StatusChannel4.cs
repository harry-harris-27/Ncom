using Ncom.Enumerations;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Velocity accuracies.
    /// </summary>
    public class StatusChannel4 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid. 
        /// </summary>
        public const byte AgeValidThreshold = 150;


        private ushort _northVelocityAccuracy = ushort.MaxValue;
        private ushort _eastVelocityAccuracy = ushort.MaxValue;
        private ushort _downVelocityAccuracy = ushort.MaxValue;


        /// <summary>
        /// Initializes a new <see cref="StatusChannel4"/> instance.
        /// </summary>
        public StatusChannel4() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel4"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel4"/> instance to copy.</param>
        public StatusChannel4(StatusChannel4 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 4;

        /// <summary>
        /// Gets or sets the north velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort NorthVelocityAccuracy { get => _northVelocityAccuracy; set => _northVelocityAccuracy = value; }

        /// <summary>
        /// Gets or sets the east velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort EastVelocityAccuracy { get => _eastVelocityAccuracy; set => _eastVelocityAccuracy = value; }

        /// <summary>
        /// Gets or sets the down velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort DownVelocityAccuracy { get => _downVelocityAccuracy; set => _downVelocityAccuracy = value; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BlendedProcessingMethod"/> used by Blended.
        /// </summary>
        public BlendedProcessingMethod ProcessingMethod { get; set; }

        /// <summary>
        /// Gets a value indicating whether the  <see cref="NorthVelocityAccuracy"/>, 
        /// <see cref="EastVelocityAccuracy"/> and <see cref="DownVelocityAccuracy"/> values are 
        /// valid.
        /// </summary>
        public bool IsAccuracyValid => Age < AgeValidThreshold;


        /// <inheritdoc/>
        public override IStatusChannel Clone() => new StatusChannel4(this);

        /// <summary>
        /// Sets this <see cref="StatusChannel4"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel4"/> instance to copy.</param>
        public void Copy(StatusChannel4 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.NorthVelocityAccuracy = source.NorthVelocityAccuracy;
            this.EastVelocityAccuracy = source.EastVelocityAccuracy;
            this.DownVelocityAccuracy = source.DownVelocityAccuracy;
            this.Age = source.Age;
            this.ProcessingMethod = source.ProcessingMethod;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + NorthVelocityAccuracy.GetHashCode();
            hash = (hash * mul) + EastVelocityAccuracy.GetHashCode();
            hash = (hash * mul) + DownVelocityAccuracy.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + (byte)ProcessingMethod;

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel4 other
                && this.NorthVelocityAccuracy == other.NorthVelocityAccuracy
                && this.EastVelocityAccuracy == other.EastVelocityAccuracy
                && this.DownVelocityAccuracy == other.DownVelocityAccuracy
                && this.Age == other.Age
                && this.ProcessingMethod == other.ProcessingMethod;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.Marshal(buffer, ref offset, NorthVelocityAccuracy);
            ByteHandling.Marshal(buffer, ref offset, EastVelocityAccuracy);
            ByteHandling.Marshal(buffer, ref offset, DownVelocityAccuracy);

            buffer[offset++] = Age;
            buffer[offset++] = (byte)ProcessingMethod;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Base method implements non-null check")]
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _northVelocityAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _eastVelocityAccuracy);
            ByteHandling.UnmarshalUInt16(buffer, ref offset, ref _downVelocityAccuracy);

            Age = buffer[offset++];
            ProcessingMethod = ByteHandling.ParseEnum(buffer[offset++], BlendedProcessingMethod.Unknown);
        }

    }
}
