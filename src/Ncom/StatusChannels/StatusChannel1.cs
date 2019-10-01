using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom.StatusChannels
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

        public StatusChannel1() : this(null) { }

        public StatusChannel1(StatusChannel1 source) : base(1)
        {
            if (source != null)
            {
                PositionXInnovation = source.PositionXInnovation;
                PositionXInnovationValid = source.PositionXInnovationValid;

                PositionYInnovation = source.PositionYInnovation;
                PositionYInnovationValid = source.PositionYInnovationValid;

                PositionZInnovation = source.PositionZInnovation;
                PositionZInnovationValid = source.PositionZInnovationValid;

                VelocityXInnovation = source.VelocityXInnovation;
                VelocityXInnovationValid = source.VelocityXInnovationValid;

                VelocityYInnovation = source.VelocityYInnovation;
                VelocityYInnovationValid = source.VelocityYInnovationValid;

                VelocityZInnovation = source.VelocityZInnovation;
                VelocityZInnovationValid = source.VelocityZInnovationValid;

                OrientationPitchInnovation = source.OrientationPitchInnovation;
                OrientationPitchInnovationValid = source.OrientationPitchInnovationValid;

                OrientationHeadingInnovation = source.OrientationHeadingInnovation;
                OrientationHeadingInnovationValid = source.OrientationHeadingInnovationValid;
            }
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        public byte PositionXInnovation { get; set; } = 0;
        public bool PositionXInnovationValid { get; set; } = false;

        public byte PositionYInnovation { get; set; } = 0;
        public bool PositionYInnovationValid { get; set; } = false;

        public byte PositionZInnovation { get; set; } = 0;
        public bool PositionZInnovationValid { get; set; } = false;

        public byte VelocityXInnovation { get; set; } = 0;
        public bool VelocityXInnovationValid { get; set; } = false;

        public byte VelocityYInnovation { get; set; } = 0;
        public bool VelocityYInnovationValid { get; set; } = false;

        public byte VelocityZInnovation { get; set; } = 0;
        public bool VelocityZInnovationValid { get; set; } = false;

        public byte OrientationPitchInnovation { get; set; } = 0;
        public bool OrientationPitchInnovationValid { get; set; } = false;

        public byte OrientationHeadingInnovation { get; set; } = 0;
        public bool OrientationHeadingInnovationValid { get; set; } = false;


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + PositionXInnovation;
            hash = (hash * mul) + PositionYInnovation;
            hash = (hash * mul) + PositionZInnovation;
            hash = (hash * mul) + (PositionXInnovationValid ? 1 : 0);
            hash = (hash * mul) + (PositionYInnovationValid ? 1 : 0);
            hash = (hash * mul) + (PositionZInnovationValid ? 1 : 0);
            hash = (hash * mul) + VelocityXInnovation;
            hash = (hash * mul) + VelocityYInnovation;
            hash = (hash * mul) + VelocityZInnovation;
            hash = (hash * mul) + (VelocityXInnovationValid ? 1 : 0);
            hash = (hash * mul) + (VelocityYInnovationValid ? 1 : 0);
            hash = (hash * mul) + (VelocityZInnovationValid ? 1 : 0);
            hash = (hash * mul) + OrientationHeadingInnovation;
            hash = (hash * mul) + OrientationPitchInnovation;
            hash = (hash * mul) + (OrientationHeadingInnovationValid ? 1 : 0);
            hash = (hash * mul) + (OrientationPitchInnovationValid ? 1 : 0);

            return hash;
        }

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 1;

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

        /* ---------- Protected methods -------------------------------------------------------/**/

        /// <summary>
        /// A pure implementation of value equality that avoids the routine checks in 
        /// <see cref="Equals(object)"/>.
        /// To override the default equals method, override this method instead.
        /// </summary>
        /// <param name="pkt"></param>
        /// <returns></returns>
        protected override bool IsEqual(StatusChannel data)
        {
            StatusChannel1 chan = data as StatusChannel1;

            return base.IsEqual(chan)
                && this.PositionXInnovation == chan.PositionXInnovation
                && this.PositionYInnovation == chan.PositionYInnovation
                && this.PositionZInnovation == chan.PositionZInnovation
                && this.PositionXInnovationValid == chan.PositionXInnovationValid
                && this.PositionYInnovationValid == chan.PositionYInnovationValid
                && this.PositionZInnovationValid == chan.PositionZInnovationValid
                && this.VelocityXInnovation == chan.VelocityXInnovation
                && this.VelocityYInnovation == chan.VelocityYInnovation
                && this.VelocityZInnovation == chan.VelocityZInnovation
                && this.VelocityXInnovationValid == chan.VelocityXInnovationValid
                && this.VelocityYInnovationValid == chan.VelocityYInnovationValid
                && this.VelocityZInnovationValid == chan.VelocityZInnovationValid
                && this.OrientationHeadingInnovation == chan.OrientationHeadingInnovation
                && this.OrientationPitchInnovation == chan.OrientationPitchInnovation
                && this.OrientationHeadingInnovationValid == chan.OrientationHeadingInnovationValid
                && this.OrientationPitchInnovationValid == chan.OrientationPitchInnovationValid;

        }

    }
}
