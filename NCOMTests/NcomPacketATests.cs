using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCOM;
using NCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Tests
{
    [TestClass()]
    public class NcomPacketATests
    {

        private string ncomHex = "E7EC04494600EEFAFF5C7FFEA4F1FF08F9FF71F8FF043F50DBDD2DCB0EED3F79C3624E7FCCA5BF986BC7422B9BFED410FE11F6FF54B3DE792E0086AFFFEB253000B5000C0B0000F7";


        [TestInitialize]
        public void Init()
        {
            
        }


        [TestMethod()]
        public void MarshalTest()
        {
            // Get byte data
            byte[] data = HexStringToBytes(ncomHex);

            // Create an NCOM packet and marshal the hex string
            List<NcomPacket> listNcom = new NcomPacketFactory().ProcessNcom(data);

            // Check that length != 1 then failed
            if (listNcom.Count != 1)
            {
                Assert.Fail("Unexcepted length. Expected 1, got " + listNcom.Count);
                return;
            }

            // Check decoded is packet A
            NcomPacketA pkt = listNcom[0] as NcomPacketA;
            if (pkt == null)
            {
                Assert.Fail("Decoded NCOM packet is not of type NcomPacketA");
                return;
            }

            // Todo: Check values
            Assert.IsTrue(true);
        }



        private static byte[] HexStringToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
        }
    }
}