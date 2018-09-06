using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    /// <summary>
    /// Kalman filter innovations set 1 (position, velocity, attitude). 
    /// </summary>
    public class StatusChannel1 : StatusChannel
    {

        /* ---------- Constants ---------------------------------------------------------------/**/

        private const byte INNOVATION_MASK = 0xFE;
        private const byte INNOVATION_VALID_MASK = 0xFF - INNOVATION_MASK;


        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel1() : base(1) { }


        /* ---------- Properties --------------------------------------------------------------/**/

        public byte PositionXInnovation { get; set; }
        public bool PositionXInnovationValid { get; set; }

        public byte PositionYInnovation { get; set; }
        public bool PositionYInnovationValid { get; set; }

        public byte PositionZInnovation { get; set; }
        public bool PositionZInnovationValid { get; set; }

        public byte VelocityXInnovation { get; set; }
        public bool VelocityXInnovationValid { get; set; }

        public byte VelocityYInnovation { get; set; }
        public bool VelocityYInnovationValid { get; set; }

        public byte VelocityZInnovation { get; set; }
        public bool VelocityZInnovationValid { get; set; }

        public byte OrientationPitchInnovation { get; set; }
        public bool OrientationPitchInnovationValid { get; set; }

        public byte OrientationHeadingInnovation { get; set; }
        public bool OrientationHeadingInnovationValid { get; set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 0;

            // Position X Innovation
            buffer[p] = (byte)((PositionXInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(PositionXInnovationValid ? 0x01 : 0x00);

            // Position Y Innovation
            buffer[p] = (byte)((PositionYInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(PositionYInnovationValid ? 0x01 : 0x00);

            // Position Z Innovation
            buffer[p] = (byte)((PositionZInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(PositionZInnovationValid ? 0x01 : 0x00);

            // Velocity X Innovation
            buffer[p] = (byte)((VelocityXInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(VelocityXInnovationValid ? 0x01 : 0x00);

            // Velocity Y Innovation
            buffer[p] = (byte)((VelocityYInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(VelocityYInnovationValid ? 0x01 : 0x00);

            // Velocity Z Innovation
            buffer[p] = (byte)((VelocityZInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(VelocityZInnovationValid ? 0x01 : 0x00);

            // Orientation Pitch Innovation
            buffer[p] = (byte)((OrientationPitchInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(OrientationPitchInnovationValid ? 0x01 : 0x00);

            // Orientation Heading Innovation
            buffer[p] = (byte)((OrientationHeadingInnovation << 1) & INNOVATION_MASK);
            buffer[p++] |= (byte)(OrientationHeadingInnovationValid ? 0x01 : 0x00);


            return buffer;
        }

        public override bool Unmarshal(byte[] buffer, int offset)
        {
            if (!base.Unmarshal(buffer, offset)) return false;

            // Position X innovation
            PositionXInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            PositionXInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Position Y innovation
            PositionYInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            PositionYInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Position Z innovation
            PositionZInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            PositionZInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Velocity X innovation
            VelocityXInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            VelocityXInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Velocity Y innovation
            VelocityYInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            VelocityYInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Velocity Z innovation
            VelocityZInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            VelocityZInnovationValid = (buffer[offset++] & INNOVATION_VALID_MASK) == 0x01;

            // Orientation Pitch Innovation
            OrientationPitchInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            OrientationPitchInnovationValid = (buffer[offset] & INNOVATION_VALID_MASK) == 0x01;

            // Orientation Heading Innovation
            OrientationHeadingInnovation = (byte)((buffer[offset] & INNOVATION_MASK) >> 1);
            OrientationHeadingInnovationValid = (buffer[offset] & INNOVATION_VALID_MASK) == 0x01;

            return true;
        }

    }
}
