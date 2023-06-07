using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace OxTS.NCOM.Tests
{
    public class NcomPacketFactoryTests
    {

        private static readonly byte[] sampleNcom = GetSampleNcom();
        private static readonly byte[] sampleNcomSingle = sampleNcom.Take(NcomPacket.PacketLength).ToArray();

        private readonly Random random = new Random();
        private readonly NcomPacketFactory factory = new NcomPacketFactory();


        [Fact]
        public void TestProcessNcomLength()
        {
            // Parse the sample ncom
            List<NcomPacket> ncomPackets = factory.ProcessNcom(sampleNcom);

            ncomPackets.Count.ShouldBe(sampleNcom.Length / NcomPacket.PacketLength);
        }

        [Fact]
        public void TestProcessNcomSeek()
        {
            // Hide the NCOM byte data in some random data
            byte[] preamble = new byte[random.Next(100, 200)];
            byte[] postamble = new byte[random.Next(100, 200)];

            // Populate the pre and post amble
            for (int i = 0; i < preamble.Length; i++)
            {
                preamble[i] = (byte)random.Next(0, 0xFF);
                if (preamble[i] == NcomPacket.SyncByte) i--;
            }
            for (int i = 0; i < postamble.Length; i++)
            {
                postamble[i] = (byte)random.Next(0, 0xFF);
                if (postamble[i] == NcomPacket.SyncByte) i--;
            }

            // Create array with a hidden NCOM packet
            byte[] data = new byte[preamble.Length + sampleNcomSingle.Length + postamble.Length];
            Array.Copy(preamble, 0, data, 0, preamble.Length);
            Array.Copy(sampleNcomSingle, 0, data, preamble.Length, sampleNcomSingle.Length);
            Array.Copy(postamble, 0, data, preamble.Length + sampleNcomSingle.Length, postamble.Length);

            // Parse data as NCOM
            List<NcomPacket> packets = factory.ProcessNcom(data);
            packets.Count.ShouldBe(1);
        }


        private static byte[] GetSampleNcom()
        {
            var assembly = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly;
            Stream resource = assembly.GetManifestResourceStream("Ncom.Tests.Resources.sample.ncom");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                resource.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}