using System;

namespace OxTS.NCOM
{
    /// <summary>
    /// NCOM structure-B packets are not intended to be used by customers; they are used internally
    /// by OxTS. They can be disabled over Ethernet by entering the following advanced command:
    /// <i>-udp_NCOMx_0</i>.
    /// </summary>
    public class NCOMPacketB : NCOMPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCOMPacketB"/> class.
        /// </summary>
        public NCOMPacketB() : base() { }


        /// <inheritdoc/>
        protected override void Marshal(Span<byte> buffer) { }

        /// <inheritdoc/>
        protected override void Unmarshal(ReadOnlySpan<byte> buffer) { }
    }
}
