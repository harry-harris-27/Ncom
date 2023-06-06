using System;
using Ncom.Generators;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// Internal information about primary GNSS receiver.
    /// </summary>
    /// <remarks>
    /// These counters are cyclic and will wrap when they exceed the limit of the format used.
    /// </remarks>
    [StatusChannel(2)]
    public partial class StatusChannel2 : StatusChannel
    {

        private ushort m_PrimaryGNSSReceiverCharacters = 0;
        private ushort m_PrimaryGNSSReceiverPackets = 0;
        private ushort m_PrimaryGNSSReceiverCharactersNotUnderstood = 0;
        private ushort m_PrimaryGNSSReceiverPacketsNotUsed = 0;


        /// <summary>
        /// Gets or sets the characters received from the primary GNSS receiver by the navigation
        /// computer.
        /// </summary>
        public ushort PrimaryGNSSReceiverCharacters
        {
            get => m_PrimaryGNSSReceiverCharacters;
            set => m_PrimaryGNSSReceiverCharacters = value;
        }

        /// <summary>
        /// Gets or sets the number of packets received from the primary GNSS receiver by the
        /// navigation computer.
        /// </summary>
        public ushort PrimaryGNSSReceiverPackets
        {
            get => m_PrimaryGNSSReceiverPackets;
            set => m_PrimaryGNSSReceiverPackets = value;
        }

        /// <summary>
        /// Gets or sets the characters received from the primary GNSS receiver by the navigation
        /// computer, but not understood by the decoder.
        /// </summary>
        public ushort PrimaryGNSSReceiverCharactersNotUnderstood
        {
            get => m_PrimaryGNSSReceiverCharactersNotUnderstood;
            set => m_PrimaryGNSSReceiverCharactersNotUnderstood = value;
        }

        /// <summary>
        /// Gets or sets the number of packets received from the primary GNSS receiver by the
        /// navigation computer that could not be used to update the Kalman filter (e.g. too old).
        /// </summary>
        public ushort PrimaryGNSSReceiverPacketsNotUsed
        {
            get => m_PrimaryGNSSReceiverPacketsNotUsed;
            set => m_PrimaryGNSSReceiverPacketsNotUsed = value;
        }


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
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            ByteHandling.Marshal(buffer.Slice(0), m_PrimaryGNSSReceiverCharacters);
            ByteHandling.Marshal(buffer.Slice(2), m_PrimaryGNSSReceiverPackets);
            ByteHandling.Marshal(buffer.Slice(4), m_PrimaryGNSSReceiverCharactersNotUnderstood);
            ByteHandling.Marshal(buffer.Slice(6), m_PrimaryGNSSReceiverPacketsNotUsed);
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            ByteHandling.Unmarshal(buffer.Slice(0), out m_PrimaryGNSSReceiverCharacters);
            ByteHandling.Unmarshal(buffer.Slice(2), out m_PrimaryGNSSReceiverPackets);
            ByteHandling.Unmarshal(buffer.Slice(4), out m_PrimaryGNSSReceiverCharactersNotUnderstood);
            ByteHandling.Unmarshal(buffer.Slice(6), out m_PrimaryGNSSReceiverPacketsNotUsed);
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

    }
}
