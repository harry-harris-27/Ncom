using System;

namespace OxTS
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
        public int GetMarshalledSize()
        {
            return StatusChannelLength;
        }

        /// <inheritdoc/>
        public bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (buffer.Length < StatusChannelLength)
            {
                bytes = -1;
                return false;
            }

            Marshal(buffer);
            bytes = StatusChannelLength;

            return true;
        }

        /// <inheritdoc/>
        public bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (buffer.Length < StatusChannelLength)
            {
                bytes = -1;
                return false;
            }

            Unmarshal(buffer);
            bytes = StatusChannelLength;

            return true;
        }


        /// <summary>
        /// Marshals this data structure into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to write marshalled data to.</param>
        /// <remarks>
        /// The called to this method guarentees that the <paramref name="buffer"/> is large enough
        /// to store the marshalled data.
        /// </remarks>
        protected abstract void Marshal(Span<byte> buffer);

        /// <summary>
        /// Unmsarshals this data structure from the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to read marshalled data from.</param>
        /// <remarks>
        /// The called to this method guarentees that the <paramref name="buffer"/> is large enough
        /// to store the marshalled data.
        /// </remarks>
        protected abstract void Unmarshal(ReadOnlySpan<byte> buffer);

    }
}
