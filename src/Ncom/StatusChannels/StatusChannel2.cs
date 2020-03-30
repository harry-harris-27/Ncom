using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    /// <summary>
    /// Internal information about primary GNSS receiver. 
    /// </summary>
    public class StatusChannel2 : StatusChannel
    {

        /* ---------- Constants ---------------------------------------------------------------/**/


        /* ---------- Constructors ------------------------------------------------------------/**/

        public StatusChannel2() : base(2)
        {

        }


        /* ---------- Properties --------------------------------------------------------------/**/

        /// <summary>
        /// Characters received from the primary GNSS receiver by the navigation computer. 
        /// </summary>
        public ushort PrimaryGNSSReceiverCharacters { get; set; }

        /// <summary>
        /// Packets received from the primary GNSS receiver by the navigation computer.
        /// </summary>
        public ushort PrimaryGNSSReceiverPackets { get; set; }

        /// <summary>
        /// Characters received from the primary GNSS receiver by the navigation computer, but not 
        /// understood by the decoder.
        /// </summary>
        public ushort PrimaryGNSSReceiverCharactersNotUnderstood { get; set; }

        /// <summary>
        /// Packets received from the primary GNSS receiver by the navigation computer that could 
        /// not be used to update the Kalman filter (e.g. too old).
        /// </summary>
        public ushort PrimaryGNSSReceiverPacketsNotUsed { get; set; }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + PrimaryGNSSReceiverCharacters.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverPackets.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverCharactersNotUnderstood.GetHashCode();
            hash = (hash * mul) + PrimaryGNSSReceiverPacketsNotUsed.GetHashCode();

            return hash;
        }

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 1;

            // Characters received
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverCharacters), 0, buffer, p, 2);
            p += 2;

            // Packets received
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverPackets), 0, buffer, p, 2);
            p += 2;

            // Characters received, not used
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverCharactersNotUnderstood), 0, buffer, p, 2);
            p += 2;

            // Packets received, not understood
            Array.Copy(BitConverter.GetBytes(PrimaryGNSSReceiverPacketsNotUsed), 0, buffer, p, 2);
            p += 2;

            return buffer;
        }

        public override bool Unmarshal(byte[] buffer, int offset)
        {
            if (!base.Unmarshal(buffer, offset)) return false;

            // Characters received
            PrimaryGNSSReceiverCharacters = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Packets received
            PrimaryGNSSReceiverPackets = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Characters received, not used
            PrimaryGNSSReceiverCharactersNotUnderstood = BitConverter.ToUInt16(buffer, offset);
            offset += 2;

            // Packets received, not used
            PrimaryGNSSReceiverPacketsNotUsed = BitConverter.ToUInt16(buffer, offset);
            offset += 2;
            
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
            StatusChannel2 chan = data as StatusChannel2;

            return base.IsEqual(chan)
                && this.PrimaryGNSSReceiverCharacters == chan.PrimaryGNSSReceiverCharacters
                && this.PrimaryGNSSReceiverPackets == chan.PrimaryGNSSReceiverPackets
                && this.PrimaryGNSSReceiverCharactersNotUnderstood == chan.PrimaryGNSSReceiverCharactersNotUnderstood
                && this.PrimaryGNSSReceiverPacketsNotUsed == chan.PrimaryGNSSReceiverPacketsNotUsed;
        }

    }
}
