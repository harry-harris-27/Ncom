using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace OxTS.NCOM.Tests
{
    public class NCOMPacketFactoryTests
    {

        private static readonly byte[] sampleNCOM = GetSampleNCOM();
        private static readonly byte[] sampleNCOMSingle = sampleNCOM.Take(NCOMPacket.PacketLength).ToArray();

        private readonly Random random = new Random();
        private readonly NCOMPacketFactory factory = new NCOMPacketFactory();


        [Fact]
        public void TestProcessNCOMLength()
        {
            // Parse the sample ncom
            List<NCOMPacket> ncomPackets = factory.ProcessNCOM(sampleNCOM);

            ncomPackets.Count.ShouldBe(sampleNCOM.Length / NCOMPacket.PacketLength);
        }

        [Fact]
        public void TestProcessNCOMSeek()
        {
            // Hide the NCOM byte data in some random data
            byte[] preamble = new byte[random.Next(100, 200)];
            byte[] postamble = new byte[random.Next(100, 200)];

            // Populate the pre and post amble
            for (int i = 0; i < preamble.Length; i++)
            {
                preamble[i] = (byte)random.Next(0, 0xFF);
                if (preamble[i] == NCOMPacket.SyncByte) i--;
            }
            for (int i = 0; i < postamble.Length; i++)
            {
                postamble[i] = (byte)random.Next(0, 0xFF);
                if (postamble[i] == NCOMPacket.SyncByte) i--;
            }

            // Create array with a hidden NCOM packet
            byte[] data = new byte[preamble.Length + sampleNCOMSingle.Length + postamble.Length];
            Array.Copy(preamble, 0, data, 0, preamble.Length);
            Array.Copy(sampleNCOMSingle, 0, data, preamble.Length, sampleNCOMSingle.Length);
            Array.Copy(postamble, 0, data, preamble.Length + sampleNCOMSingle.Length, postamble.Length);

            // Parse data as NCOM
            List<NCOMPacket> packets = factory.ProcessNCOM(data);
            packets.Count.ShouldBe(1);
        }


        private static byte[] GetSampleNCOM()
        {
            var assembly = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly;
            Stream resource = assembly.GetManifestResourceStream("OxTS.NCOM.Tests.Resources.sample.ncom");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                resource.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}