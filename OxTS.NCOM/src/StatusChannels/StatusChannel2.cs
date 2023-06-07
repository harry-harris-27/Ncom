using System;
using OxTS.NCOM.Generators;

namespace OxTS.NCOM.StatusChannels
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

    }
}
