using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Utilities.Tests
{
    [TestClass()]
    public class NcomPacketFactoryTests
    {

        private static readonly Random random = new Random();
        private static readonly NcomPacketFactory factory = new NcomPacketFactory();
        private static readonly byte[] sampleNcom = NCOMTests.Properties.Resources.sample;
        private static readonly byte[] sampleNcomSingle = sampleNcom.Take(NcomPacket.PACKET_LENGTH).ToArray();
        

        [TestMethod()]
        public void TestProcessNcomLength()
        {
            // Parse the sample ncom
            List<NcomPacket> ncomPackets = factory.ProcessNcom(sampleNcom);

            Assert.AreEqual(sampleNcom.Length / NcomPacket.PACKET_LENGTH, ncomPackets.Count);
        }

        [TestMethod()]
        public void TestProcessNcomSeek()
        {
            // Hide the NCOM byte data in some random data
            byte[] preamble = new byte[random.Next(100, 200)];
            byte[] postamble = new byte[random.Next(100, 200)];

            // Populate the pre and post amble
            for (int i = 0; i < preamble.Length; i++)
            {
                preamble[i] = (byte)random.Next(0, 0xFF);
                if (preamble[i] == NcomPacket.SYNC_BYTE) i--;
            }
            for (int i = 0; i < postamble.Length; i++)
            {
                postamble[i] = (byte)random.Next(0, 0xFF);
                if (postamble[i] == NcomPacket.SYNC_BYTE) i--;
            }

            // Create array with a hidden NCOM packet
            byte[] data = new byte[preamble.Length + sampleNcomSingle.Length + postamble.Length];
            Array.Copy(preamble, 0, data, 0, preamble.Length);
            Array.Copy(sampleNcomSingle, 0, data, preamble.Length, sampleNcomSingle.Length);
            Array.Copy(postamble, 0, data, preamble.Length + sampleNcomSingle.Length, postamble.Length);

            // Parse data as NCOM
            List<NcomPacket> pkt = factory.ProcessNcom(data);
            Assert.AreEqual(1, pkt.Count);
        }

    }
}