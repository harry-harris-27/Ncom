using System;

namespace OxTS.RCOM
{
    /// <summary>
    /// The <see cref="RCOMLanePacket"/> contains measurements from the vehicle to the lane markings.
    /// The lane markings are defined in the map file currently loaded into the RT-Range.
    /// </summary>
    public class RCOMLanePacket : RCOMPacket
    {

        internal const float DistanceScaling        = 1e-3f;
        internal const float VelocityScaling        = 1e-2f;
        internal const float AccelerationScaling    = 1e-2f;
        internal const float CurvatureScaling       = 1e-4f;
        internal const float HeadingScaling         = 1e-2f;


        // Property backing stores
        private ushort m_Time = 0;
        private byte m_LineNumberToLeftOfA = 0;
        private byte m_LineNumberToRightOfA = 0;
        private float m_DistanceAlongLane1 = 0;
        private float m_LateralDistanceToLeftOfA = 0;
        private float m_LateralVelocityToLeftOfA = 0;
        private float m_LateralAccelerationToLeftOfA = 0;
        private float m_LateralDistanceFromPointBToLineOnLeftOfA = 0;
        private float m_LateralDistanceFromPointCToLineOnRightOfA = 0;
        private byte m_LineOnLeftOfPointB = 0;
        private byte m_LineOnRightOfPointB = 0;
        private byte m_LineOnLeftOfPointC = 0;
        private byte m_LineOnRightOfPointC = 0;
        private float m_HeadingWithRespectToLineOnLeftOfA = 0;
        private float m_HeadingWithRespectToLineOnRightOfA = 0;



        public RCOMLanePacket()
            : base(RCOMPacketType.LanePacket)
        { }



        public EightLineVector LateralDistanceFromPointATo { get; } = new EightLineVector(DistanceScaling);
        public EightLineVector LateralVelocityFromPointATo { get; } = new EightLineVector(VelocityScaling);
        public EightLineVector LateralDistanceFromPointBTo { get; } = new EightLineVector(DistanceScaling);
        public EightLineVector LateralDistanceFromPointCTo { get; } = new EightLineVector(DistanceScaling);
        public EightLineVector CurvatureOf { get; } = new EightLineVector(1e-3f);


        /// <inheritdoc/>
        public override int GetMarshalledSize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool TryMarshal(Span<byte> buffer, out int bytes)
        {
            if (!base.TryMarshal(buffer, out bytes))
            {
                return false;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes)
        {
            if (!base.TryUnmarshal(buffer, out bytes))
            {
                return false;
            }

            return false;
        }

    }
}
