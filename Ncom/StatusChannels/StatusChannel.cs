using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Abstract base class for <see cref="IStatusChannel"/> implementations.
    /// </summary>
    public abstract class StatusChannel : IStatusChannel
    {

        /// <summary>
        /// The length, in bytes, of the marshalled Batch S (Status Channels).
        /// </summary>
        internal const int StatusChannelLength = 8;


        /// <inheritdoc/>
        public abstract byte StatusChannelByte { get; }

        /// <inheritdoc/>
        public int MarshalledSize => StatusChannelLength;


        /// <inheritdoc/>
        public virtual void Marshal(Span<byte> buffer)
        {
            this.CheckedBufferSize(buffer);
        }

        /// <inheritdoc/>
        public virtual void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            this.CheckedBufferSize(buffer);
        }

    }
}
