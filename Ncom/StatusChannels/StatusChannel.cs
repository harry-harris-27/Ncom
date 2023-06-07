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
        public abstract IStatusChannel Clone();

        /// <inheritdoc/>
        public bool Equals(IStatusChannel rhs)
        {
            return rhs != null
                && this.StatusChannelByte == rhs.StatusChannelByte
                && EqualsIntl(rhs);
        }

        /// <inheritdoc/>
        public override bool Equals(object value)
        {
            return value is IStatusChannel rhs && Equals(rhs);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + StatusChannelByte;

            return hash;
        }

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


        /// <summary>
        /// A pure implementation of value equality that avoids the routine checks in
        /// <see cref="Equals(object)"/> and <see cref="Equals(IStatusChannel)"/>.
        /// To override the default equals method, override this method instead.
        /// </summary>
        /// <param name="pkt"></param>
        /// <returns>
        /// <see langword="true"/> if the specified object is equal to this current object; otherwise
        /// <see langword="false"/>.
        /// </returns>
        protected abstract bool EqualsIntl(IStatusChannel pkt);

    }
}
