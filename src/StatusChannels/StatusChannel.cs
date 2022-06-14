using System;
using System.Collections;
using System.Collections.Generic;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Abstract base class for <see cref="IStatusChannel"/> implementations.
    /// </summary>
    public abstract class StatusChannel : IStatusChannel
    {

        private static readonly IDictionary<byte, Type> statusChannelTypeLookup = new Dictionary<byte, Type>();


        protected static void RegisterStatusChannel(byte statusChannel, Type type)
        {
            statusChannelTypeLookup[statusChannel] = type;
        }

        internal static bool TryGetStatusChannelType(byte statusChannelByte, out Type statusChannelType)
        {
            return statusChannelTypeLookup.TryGetValue(statusChannelByte, out statusChannelType);
        }


        /// <inheritdoc/>
        public abstract byte StatusChannelByte { get; }


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
        public void Marshal(byte[] buffer, int offset)
        {
            // Perform routine argument validation checks
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset + NcomPacketA.StatusChannelLength > buffer.Length)
                throw new IndexOutOfRangeException();

            // Marshal this status channel into the buffer
            MarshalIntl(buffer, offset);
        }

        /// <inheritdoc/>
        public void Unmarshal(byte[] buffer, int offset)
        {
            // Perform routine argument validation checks
            if (buffer == null) 
                throw new ArgumentNullException(nameof(buffer));

            if (offset + NcomPacketA.StatusChannelLength > buffer.Length)
                throw new IndexOutOfRangeException();

            UnmarshalIntl(buffer, offset);
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

        /// <summary>
        /// Performs the actually marshalling of this Status Channel message.
        /// </summary>
        /// <param name="buffer">The byte array to marshall this StatusChannel into.</param>
        /// <param name="offset">The zero-based index indicating the location in the buffer start writing from.</param>
        protected abstract void MarshalIntl(byte[] buffer, int offset);

        /// <summary>
        /// Performs the actual unmarshalling of this Status Channel message.
        /// </summary>
        /// <param name="buffer">The byte array containing the marshalled StatusChannel data.</param>
        /// <param name="offset">The zero-based index indicating the location in the buffer read from.</param>
        /// <remarks>
        /// This method is only called from <see cref="Unmarshal(byte[], int)"/>, which performs 
        /// argument validation checks, removing the need for any further checks.
        /// </remarks>
        protected abstract void UnmarshalIntl(byte[] buffer, int offset);
    }
}
