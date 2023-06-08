using System;

namespace OxTS.RCOM
{
    /// <summary>
    /// Represents an RCOM Trigger Time packet. The trigger time packet is used in RCOM files (not
    /// over Ethernet) to identify the time of the trigger that started the logging to the file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// </para>
    /// It allows software to have "pre-trigger" data in the file. RT-Range Post-process will have
    /// the "TimeFromStart" field as zero at the time of the trigger. Data before the trigger will
    /// have a negative "TimeFromStart".
    /// <para>
    /// There is no status fgield in this packet.
    /// </para>
    /// </remarks>
    public class RCOMTriggerTimePacket : RCOMPacket
    {

        private const ushort MillisecondsIntoMinuteInvalid = 0xFFFF;
        private const byte MillisecondOffsetInvalid = 0x80;
        private const uint MinutesSinceGPSEpochInvalid = 0x80000000;


        /// <summary>
        /// The number bytes occupied by a marshalled <see cref="RCOMTriggerTimePacket"/>.
        /// </summary>
        public const int PacketLength = 12;


        public RCOMTriggerTimePacket()
            : base(RCOMPacketType.TriggerTimePacket)
        { }


        /// <summary>
        /// Gets or sets the time that caused this trigger. If <c>null</c> then time is invalid.
        /// </summary>
        public DateTime? TriggerTime { get; set; } = null;


        /// <inheritdoc/>
        public override int GetMarshalledSize() => PacketLength;

        /// <inheritdoc/>
        public override bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (!base.TryMarshal(buffer, out bytes))
            {
                return false;
            }

            ushort msIntoMinute = MillisecondsIntoMinuteInvalid;
            byte msOffset = MillisecondOffsetInvalid;
            uint minsSinceEpoch = MinutesSinceGPSEpochInvalid;

            if (TriggerTime is not null && TriggerTime > Constants.GPSTimeEpoch)
            {
                var sinceEpoch = TriggerTime.Value - Constants.GPSTimeEpoch;

                minsSinceEpoch = (uint)sinceEpoch.TotalMinutes;

                var intoMinute = sinceEpoch - TimeSpan.FromMinutes(minsSinceEpoch);
                msIntoMinute = (ushort)Math.Round(intoMinute.TotalMilliseconds);

                msOffset = (byte)(sbyte)((intoMinute - TimeSpan.FromMilliseconds(msIntoMinute)).TotalMilliseconds / 0.004);
            }

            ByteHandling.Marshal(buffer.Slice(4), msIntoMinute);
            buffer[6] = msOffset;
            ByteHandling.Marshal(buffer.Slice(7), minsSinceEpoch);

            return true;
        }

        /// <inheritdoc/>
        public override bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            TriggerTime = null;

            if (!base.TryUnmarshal(buffer, out bytes))
            {
                return false;
            }

            ByteHandling.Unmarshal(buffer.Slice(4), out ushort msIntoMinute);
            if (msIntoMinute == MillisecondsIntoMinuteInvalid)
            {
                return false;
            }

            byte millisecondOffset = buffer[6];
            if (millisecondOffset == MillisecondOffsetInvalid)
            {
                return false;
            }

            ByteHandling.Unmarshal(buffer.Slice(7), out uint minutesSinceGPSEpoch);
            if (minutesSinceGPSEpoch == MinutesSinceGPSEpochInvalid)
            {
                return false;
            }

            TriggerTime = Constants.GPSTimeEpoch
                + TimeSpan.FromMinutes(minutesSinceGPSEpoch)
                + TimeSpan.FromMilliseconds(msIntoMinute + ((sbyte)millisecondOffset) * 0.004);

            return true;
        }

    }
}
