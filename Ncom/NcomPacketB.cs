﻿using System;

namespace Ncom
{
    /// <summary>
    /// Ncom structure-B packets are not intended to be used by customers; they are used internally
    /// by OxTS. They can be disabled over Ethernet by entering the following advanced command:
    /// <i>-udp_Ncomx_0</i>.
    /// </summary>
    public class NcomPacketB : NcomPacket
    {

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


        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();

    }
}