using System;

namespace OxTS.RCOM
{
    public enum RCOMPacketType : byte
    {
        RangePacket = 0x00,

        LanePacket = 0x01,

        ExtendedRangePacket = 0x02,

        WrappedNCOMPacket = 0x03,

        TriggerTimePacket = 0x04,

        PolygonPacket = 0x05,

        MultiSensorPointPacket = 0x06
    }
}
