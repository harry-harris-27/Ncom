using Ncom.Enumerations;
using Ncom.Generators;
using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Velocity accuracies.
    /// </summary>
    [StatusChannel(4)]
    public partial class StatusChannel4 : StatusChannel
    {

        /// <summary>
        /// The value where if <see cref="Age"/> exceeds, the accuracies become invalid.
        /// </summary>
        public const byte AgeValidThreshold = 150;


        private ushort m_NorthVelocityAccuracy = ushort.MaxValue;
        private ushort m_EastVelocityAccuracy = ushort.MaxValue;
        private ushort m_DownVelocityAccuracy = ushort.MaxValue;


        /// <summary>
        /// Gets or sets the north velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort NorthVelocityAccuracy { get => m_NorthVelocityAccuracy; set => m_NorthVelocityAccuracy = value; }

        /// <summary>
        /// Gets or sets the east velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort EastVelocityAccuracy { get => m_EastVelocityAccuracy; set => m_EastVelocityAccuracy = value; }

        /// <summary>
        /// Gets or sets the down velocity accuracy, expressed in mm.
        /// </summary>
        /// <seealso cref="IsAccuracyValid"/>
        public ushort DownVelocityAccuracy { get => m_DownVelocityAccuracy; set => m_DownVelocityAccuracy = value; }

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
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer.Slice(0), NorthVelocityAccuracy);
            ByteHandling.Marshal(buffer.Slice(2), EastVelocityAccuracy);
            ByteHandling.Marshal(buffer.Slice(4), DownVelocityAccuracy);

            buffer[6] = Age;
            buffer[7] = (byte)ProcessingMethod;
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer.Slice(0), out m_NorthVelocityAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(2), out m_EastVelocityAccuracy);
            ByteHandling.Unmarshal(buffer.Slice(4), out m_DownVelocityAccuracy);

            Age = buffer[6];
            ProcessingMethod = ByteHandling.ParseEnum(buffer[7], BlendedProcessingMethod.Unknown);
        }

    }
}
