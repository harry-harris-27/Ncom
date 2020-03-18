using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom
{
    /// <summary>
    /// Ncom structure-B packets are not intended to be used by customers; they are used internally 
    /// by OxTS. They can be disabled over Ethernet by entering the following advanced command: 
    /// <i>-udp_Ncomx_0</i>.
    /// </summary>
    public class NcomPacketB : NcomPacket
    {

        /* ---------- Constructors ------------------------------------------------------------/**/

        /// <summary>
        /// Initializes a new instance of the <see cref="NcomPacketB"/> class.
        /// </summary>
        public NcomPacketB() : this(null) { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="pkt"></param>
        public NcomPacketB(NcomPacketB pkt) : base(pkt)
        {
            if (pkt != null)
            {

            }
        }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        /// <remarks>
        /// The hash code generation process ignores the <see cref="NcomPacket.Checksum3" /> property, 
        /// since it is not used when testing for equality.
        /// </remarks>
        public override int GetHashCode() => base.GetHashCode();


        /* ---------- Protected Methods -------------------------------------------------------/**/

        /// <inheritdoc />
        protected internal bool IsEqual(NcomPacket pkt)
        {
            return base.IsEqual(pkt);
        }
    }
}
