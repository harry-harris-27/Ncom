using NCOM.Enumerations;
using NCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    /// <summary>
    /// Full time, number of satellites, position mode, velocity mode, dual antenna mode. 
    /// </summary>
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
        
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + FullTime.GetHashCode();
            hash = (hash * mul) + NumberOfSatellites;
            hash = (hash * mul) + (byte)MainGNSSPositionMode;
            hash = (hash * mul) + (byte)MainGNSSVelocityMode;
            hash = (hash * mul) + (byte)DualAntennaSystemsOrientationMode;

            return hash;
        }

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 0;

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

            MainGNSSPositionMode = (PositionVelocityOrientationMode)buffer[offset++];
            MainGNSSVelocityMode = (PositionVelocityOrientationMode)buffer[offset++];
            DualAntennaSystemsOrientationMode = (PositionVelocityOrientationMode)buffer[offset++];

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
            StatusChannel0 chan = data as StatusChannel0;

            return base.IsEqual(chan)
                && this.FullTime == chan.FullTime
                && this.NumberOfSatellites == chan.NumberOfSatellites
                && this.MainGNSSPositionMode == chan.MainGNSSPositionMode
                && this.MainGNSSVelocityMode == chan.MainGNSSVelocityMode
                && this.DualAntennaSystemsOrientationMode == chan.DualAntennaSystemsOrientationMode;
        }

    }
}
