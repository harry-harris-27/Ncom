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

        public override byte[] Marshal()
        {
            byte[] buffer = base.Marshal();
            int p = 0;

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
    }
}
