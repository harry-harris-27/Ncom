using OxTS.NCOM;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxTS.RCOM
{
    public class RCOMWrappedNCOMPacket : RCOMPacket
    {

        /// <summary>
        /// The number bytes occupied by a marshalled <see cref="RCOMTriggerTimePacket"/>.
        /// </summary>
        public const int PacketSize = PacketHeaderSize + 5 + NCOMPacket.PacketLength + 1;


        public RCOMWrappedNCOMPacket()
            : base(RCOMPacketType.WrappedNCOMPacket)
        { }


        /// <summary>
        /// Gets the IP Address of the RT sending the NCOM packet.
        /// </summary>
        public byte[] IPAddress { get; } = new byte[4];

        /// <summary>
        /// Gets or sets the where the NCOM packet originated from.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For NCOM data from a file (<see cref="NCOMProvenance.File"/>), the <see cref="IPAddress"/>
        /// if filled with the characters "hunt" for hunter NCOM data or "tgtx" where "x" is replaced
        /// by the target number for target NCOM.
        /// </para>
        /// <para>
        /// For NCOM that has been received on a serial port (COMx), all 4 bytes for
        /// <see cref="IPAddress"/> are set to zero.
        /// </para>
        /// </remarks>
        public NCOMProvenance Provenance { get; set; } = NCOMProvenance.Invalid;


        /// <inheritdoc/>
        public override int GetMarshalledSize() => PacketSize;

        /// <inheritdoc/>
        public override bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (!base.TryMarshal(buffer, out bytes))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (!base.TryUnmarshal(buffer, out bytes))
            {
                return false;
            }

            return true;
        }

    }
}
