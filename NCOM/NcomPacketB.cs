using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM
{
    /// <summary>
    /// NCOM structure-B packets are not intended to be used by customers; they are used internally 
    /// by OxTS. They can be disabled over Ethernet by entering the following advanced command: 
    /// <i>-udp_ncomx_0</i>.
    /// </summary>
    public class NcomPacketB : NcomPacket
    {

        /* ---------- Constructors ------------------------------------------------------------/**/

        public NcomPacketB() : base() { }

        public NcomPacketB(NcomPacketB pkt) : base(pkt)
        {
            if (pkt != null)
            {

            }
            else
            {

            }
        }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public override int GetHashCode()
        {
            //int hash = 13;
            //int mul = 7;

            //hash = (hash * mul) + (byte)NavigationStatus;

            //return hash;
            return base.GetHashCode();
        }


        /* ---------- Protected Methods -------------------------------------------------------/**/

        /// <inheritdoc />
        protected override bool IsEqual(NcomPacket pkt)
        {
            return base.IsEqual(pkt);
        }
    }
}
