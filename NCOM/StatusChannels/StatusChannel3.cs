using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    /// <summary>
    /// Position accuracy.
    /// </summary>
    public class StatusChannel3 : StatusChannel
    {

        /* ---------- Constants ---------------------------------------------------------------/**/

        public byte MAX_VALID_AGE = 150;


        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel3() : base(3)
        {

        }


        /* ---------- Properties --------------------------------------------------------------/**/

        /// <summary>
        /// North position accuracy. Valid when <see cref="Age"/> < <see cref="MAX_VALID_AGE"/>.
        /// </summary>
        public ushort NorthPositionAccuracy { get; set; }

        /// <summary>
        /// East position accuracy. Valid when <see cref="Age"/> < <see cref="MAX_VALID_AGE"/>.
        /// </summary>
        public ushort EastPositionAccuracy { get; set; }

        /// <summary>
        /// Down position accuracy. Valid when <see cref="Age"/> < <see cref="MAX_VALID_AGE"/>.
        /// </summary>
        public ushort DownPositionAccuracy { get; set; }

        /// <summary>
        /// Age.
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// ABD robot UMAC interface status byte.
        /// </summary>
        public byte ABDRobotUMACStatus { get; set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + NorthPositionAccuracy.GetHashCode();
            hash = (hash * mul) + EastPositionAccuracy.GetHashCode();
            hash = (hash * mul) + DownPositionAccuracy.GetHashCode();
            hash = (hash * mul) + Age;
            hash = (hash * mul) + ABDRobotUMACStatus;

            return hash;
        }

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 1;

            // North position accuracy
            Array.Copy(BitConverter.GetBytes(NorthPositionAccuracy), 0, buffer, p, 2);
            p += 2;

            // East position accuracy
            Array.Copy(BitConverter.GetBytes(EastPositionAccuracy), 0, buffer, p, 2);
            p += 2;

            // Down position accuracy
            Array.Copy(BitConverter.GetBytes(DownPositionAccuracy), 0, buffer, p, 2);
            p += 2;

            // Age
            buffer[p++] = Age;

            // ABD robot UMAC insterface status byte
            buffer[p++] = ABDRobotUMACStatus;

            return buffer;
        }

        public override bool Unmarshal(byte[] buffer, int offset)
        {
            if (!base.Unmarshal(buffer, offset)) return false;

            // North position accuracy
            NorthPositionAccuracy = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // East position accuracy
            EastPositionAccuracy = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Down position accuracy
            DownPositionAccuracy = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Age
            Age = buffer[offset++];

            // ABD robot UMAX interface status byte
            ABDRobotUMACStatus = buffer[offset++];

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
            StatusChannel3 chan = data as StatusChannel3;

            return base.IsEqual(chan)
                && this.NorthPositionAccuracy == chan.NorthPositionAccuracy
                && this.EastPositionAccuracy == chan.EastPositionAccuracy
                && this.DownPositionAccuracy == chan.DownPositionAccuracy
                && this.Age == chan.Age
                && this.ABDRobotUMACStatus == chan.ABDRobotUMACStatus;
        }

    }
}
