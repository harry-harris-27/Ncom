using NCOM.Enumerations;
using NCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    public class StatusChannel0 : StatusChannel
    {

        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel0() : base(0) { }


        /* ---------- Properties --------------------------------------------------------------/**/

        /// <summary>
        /// Gets or sets the time in minutes since GPS began (midnight, 6th January 1980)
        /// </summary>
        public int FullTime { get; set; }

        /// <summary>
        /// Gets or sets the number of GPS satellites tracked by the main GNSS receiver
        /// </summary>
        public byte NumberOfSatellites { get; set; }


        public PositionVelocityOrientationMode MainGNSSPositionMode { get; set; }

        public PositionVelocityOrientationMode MainGNSSVelocityMode { get; set; }

        public PositionVelocityOrientationMode DualAntennaSystemsOrientationMode { get; set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 1;

            Array.Copy(BitConverter.GetBytes(FullTime), 0, buffer, p, 4);
            p += 4;

            buffer[p++] = NumberOfSatellites;

            buffer[p++] = (byte)MainGNSSPositionMode;
            buffer[p++] = (byte)MainGNSSVelocityMode;
            buffer[p++] = (byte)DualAntennaSystemsOrientationMode;

            return buffer;
        }

        public override bool Unmarshal(byte[] buffer, int offset)
        {
            if (!base.Unmarshal(buffer, offset)) return false;

            // Time
            FullTime = BitConverter.ToInt32(buffer, offset);
            offset += 4;

            // Satellites
            NumberOfSatellites = buffer[offset];
            offset += 1;

            MainGNSSPositionMode = EnumParser.ParseByte(buffer[offset++], PositionVelocityOrientationMode.None);
            MainGNSSVelocityMode = EnumParser.ParseByte(buffer[offset++], PositionVelocityOrientationMode.None);
            DualAntennaSystemsOrientationMode = EnumParser.ParseByte(buffer[offset++], PositionVelocityOrientationMode.None);

            return true;
        }

    }
}
