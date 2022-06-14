using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Internal information about primary GNSS receiver. 
    /// </summary>
    /// <remarks>
    /// These counters are cyclic and will wrap when they exceed the limit of the format used.
    /// </remarks>
    public class StatusChannel2 : StatusChannel
    {

        /// <summary>
        /// Initializes a new <see cref="StatusChannel2"/> instance.
        /// </summary>
        public StatusChannel2() { }

        /// <summary>
        /// Initializes a new <see cref="StatusChannel2"/> instance that is logically equal to 
        /// specifed <paramref name="source"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel2"/> instance to copy.</param>
        public StatusChannel2(StatusChannel2 source)
        {
            Copy(source);
        }


        /// <inheritdoc/>
        public override byte StatusChannelByte { get; } = 2;

        /// <summary>
        /// Gets or sets the characters received from the primary GNSS receiver by the navigation 
        /// computer. 
        /// </summary>
        public ushort PrimaryGNSSReceiverCharacters { get; set; }

        /// <summary>
        /// Gets or sets the number of packets received from the primary GNSS receiver by the 
        /// navigation computer.
        /// </summary>
        public ushort PrimaryGNSSReceiverPackets { get; set; }

        /// <summary>
        /// Gets or sets the characters received from the primary GNSS receiver by the navigation 
        /// computer, but not understood by the decoder.
        /// </summary>
        public ushort PrimaryGNSSReceiverCharactersNotUnderstood { get; set; }

        /// <summary>
        /// Gets or sets the number of packets received from the primary GNSS receiver by the 
        /// navigation computer that could not be used to update the Kalman filter (e.g. too old).
        /// </summary>
        public ushort PrimaryGNSSReceiverPacketsNotUsed { get; set; }


        /// <inheritdoc/>
        public override IStatusChannel Clone() => new StatusChannel2(this);

        /// <summary>
        /// Sets this <see cref="StatusChannel2"/> instance logically equal to the specified 
        /// <paramref name="source"/> instance
        /// </summary>
        /// <param name="source">The source <see cref="StatusChannel2"/> instance to copy.</param>
        public void Copy(StatusChannel2 source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.PrimaryGNSSReceiverCharacters = source.PrimaryGNSSReceiverCharacters;
            this.PrimaryGNSSReceiverPackets = source.PrimaryGNSSReceiverPackets;
            this.PrimaryGNSSReceiverCharactersNotUnderstood = source.PrimaryGNSSReceiverCharactersNotUnderstood;
            this.PrimaryGNSSReceiverPacketsNotUsed = source.PrimaryGNSSReceiverPacketsNotUsed;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + PrimaryGNSSReceiverCharacters.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverPackets.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverCharactersNotUnderstood.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverPacketsNotUsed.GetHashCode();

            return hash;
        }


        /// <inheritdoc/>
        protected override bool EqualsIntl(IStatusChannel data)
        {
            return data is StatusChannel2 other
                && this.PrimaryGNSSReceiverCharacters == other.PrimaryGNSSReceiverCharacters
                && this.PrimaryGNSSReceiverPackets == other.PrimaryGNSSReceiverPackets
                && this.PrimaryGNSSReceiverCharactersNotUnderstood == other.PrimaryGNSSReceiverCharactersNotUnderstood
                && this.PrimaryGNSSReceiverPacketsNotUsed == other.PrimaryGNSSReceiverPacketsNotUsed;
        }

        /// <inheritdoc/>
        protected override void MarshalIntl(byte[] buffer, int offset)
        {
            // Characters received
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverCharacters), 0, buffer, offset, 2);
            offset += 2;

            // Packets received
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverPackets), 0, buffer, offset, 2);
            offset += 2;

            // Characters received, not used
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverCharactersNotUnderstood), 0, buffer, offset, 2);
            offset += 2;

            // Packets received, not understood
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverPacketsNotUsed), 0, buffer, offset, 2);
            offset += 2;
        }

        /// <inheritdoc/>
        protected override void UnmarshalIntl(byte[] buffer, int offset)
        {
            // Characters received
            PrimaryGNSSReceiverCharacters = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Packets received
            PrimaryGNSSReceiverPackets = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Characters received, not used
            PrimaryGNSSReceiverCharactersNotUnderstood = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Packets received, not used
            PrimaryGNSSReceiverPacketsNotUsed = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
        }

    }
}
