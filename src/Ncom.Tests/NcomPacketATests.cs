using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ncom.StatusChannels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom.Tests
{
    [TestClass()]
    public class NcomPacketATests
    {

        private static readonly byte[] blankNcomBytes = new byte[]
        {
            0xE7,                                               // Sync
            0x00, 0x00,                                         // Time
            0x00, 0x00, 0x00,                                   // Acceleration X
            0x00, 0x00, 0x00,                                   // Acceleration Y
            0x00, 0x00, 0x00,                                   // Acceleration Z
            0x00, 0x00, 0x00,                                   // Angular Rate X
            0x00, 0x00, 0x00,                                   // Angular Rate Y
            0x00, 0x00, 0x00,                                   // Angular Rate Z
            0x00,                                               // Nav status
            0x00,                                               // Checksum 1
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,     // Latitude
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,     // Longitude
            0x00, 0x00, 0x00, 0x00,                             // Altitude
            0x00, 0x00, 0x00,                                   // North velocity
            0x00, 0x00, 0x00,                                   // East Velocity
            0x00, 0x00, 0x00,                                   // Down velocity
            0x00, 0x00, 0x00,                                   // Heading
            0x00, 0x00, 0x00,                                   // Pitch
            0x00, 0x00, 0x00,                                   // Roll
            0x00,                                               // Checksum 2
            0x00,                                               // Status channel byte
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,     // Status channel
            0x00                                                // Checksum 3
        };

        [TestMethod()]
        public void TestEquals()
        {
            // Create ncom packets
            NcomPacketA[] pkts = new NcomPacketA[2];
            for (int i = 0; i < pkts.Length; i++)
            {
                pkts[i] = new NcomPacketA()
                {
                    Time = 1,
                    AccelerationX = 2,
                    AccelerationY = 3,
                    AccelerationZ = 4,
                    AngularRateX = 5,
                    AngularRateY = 6,
                    AngularRateZ = 7,
                    NavigationStatus = Enumerations.NavigationStatus.Locked,
                    Latitude = 8,
                    Longitude = 9,
                    Altitude = 10,
                    NorthVelocity = 11,
                    EastVelocity = 12,
                    DownVelocity = 13,
                    Heading = 14,
                    Pitch = 15,
                    Roll = 16,
                    StatusChannel = new StatusChannel0()
                    {
                        FullTime = 17,
                        NumberOfSatellites = 18,
                        MainGNSSPositionMode = Enumerations.PositionVelocityOrientationMode.CDGPS,
                        MainGNSSVelocityMode = Enumerations.PositionVelocityOrientationMode.Blanked,
                        DualAntennaSystemsOrientationMode = Enumerations.PositionVelocityOrientationMode.Doppler
                    }
                };
            }

            // Test not equal to null reference
            Assert.AreNotEqual(pkts[0], null);

            // Test same object
            Assert.AreEqual(pkts[0], pkts[0]);

            // Test 2 different objects
            Assert.AreEqual(pkts[0], pkts[1]);
            Assert.AreEqual(pkts[1], pkts[0]);     // Transitive

            // Change one packet
            pkts[1].Time = 0;
            Assert.AreNotEqual(pkts[0], pkts[1]);
            Assert.AreNotEqual(pkts[1], pkts[0]);     // Transitive
        }


        [TestMethod()]
        public void BlankMarshalTest()
        {
            // Create a blank NCOM structure A packet
            NcomPacketA ncom = new NcomPacketA()
            {
                Time = 0,
                AccelerationX = 0,
                AccelerationY = 0,
                AccelerationZ = 0,
                AngularRateX = 0,
                AngularRateY = 0,
                AngularRateZ = 0,
                NavigationStatus = Enumerations.NavigationStatus.Invalid,
                Latitude = 0,
                Longitude = 0,
                Altitude = 0,
                NorthVelocity = 0,
                EastVelocity = 0,
                DownVelocity = 0,
                Heading = 0,
                Pitch = 0,
                Roll = 0,
                StatusChannel = new StatusChannel0()
                {
                    FullTime = 0,
                    NumberOfSatellites = 0,
                    MainGNSSPositionMode = Enumerations.PositionVelocityOrientationMode.None,
                    MainGNSSVelocityMode = Enumerations.PositionVelocityOrientationMode.None,
                    DualAntennaSystemsOrientationMode = Enumerations.PositionVelocityOrientationMode.None
                }
            };

            // Marshal
            byte[] marshalled = ncom.Marshal();

            // Check 2 arrays are equal
            for (int i = 0; i < marshalled.Length; i++)
            {
                Assert.AreEqual(blankNcomBytes[i], marshalled[i]);
            }
        }

        [TestMethod()]
        public void BlankUnmarshalTest()
        {
            // Unmarshal NCOM data
            NcomPacketA pkt = new NcomPacketA();
            pkt.Unmarshal(blankNcomBytes, 0);

            // Everything should be zero
            Assert.AreEqual(0, pkt.Time);
            Assert.AreEqual(0, pkt.AccelerationX);
            Assert.AreEqual(0, pkt.AccelerationY);
            Assert.AreEqual(0, pkt.AccelerationZ);
            Assert.AreEqual(0, pkt.AngularRateX);
            Assert.AreEqual(0, pkt.AngularRateY);
            Assert.AreEqual(0, pkt.AngularRateZ);
            Assert.AreEqual(0, (byte)pkt.NavigationStatus);
            Assert.AreEqual(0, pkt.Latitude);
            Assert.AreEqual(0, pkt.Longitude);
            Assert.AreEqual(0, pkt.Altitude);
            Assert.AreEqual(0, pkt.NorthVelocity);
            Assert.AreEqual(0, pkt.EastVelocity);
            Assert.AreEqual(0, pkt.DownVelocity);
            Assert.AreEqual(0, pkt.Heading);
            Assert.AreEqual(0, pkt.Pitch);
            Assert.AreEqual(0, pkt.Roll);
            Assert.AreEqual(0, pkt.StatusChannel.StatusChannelByte);

            StatusChannel0 chan = pkt.StatusChannel as StatusChannel0;
            Assert.AreEqual(0, chan.FullTime);
            Assert.AreEqual(0, chan.NumberOfSatellites);
            Assert.AreEqual(0, (byte)chan.MainGNSSPositionMode);
            Assert.AreEqual(0, (byte)chan.MainGNSSVelocityMode);
            Assert.AreEqual(0, (byte)chan.DualAntennaSystemsOrientationMode);
        }

        [TestMethod()]
        public void CopyConstructorTest()
        {
            // Create an NCOM structure A packet
            NcomPacketA pkt1 = new NcomPacketA()
            {
                Time = 1,
                AccelerationX = 2,
                AccelerationY = 3,
                AccelerationZ = 4,
                AngularRateX = 5,
                AngularRateY = 6,
                AngularRateZ = 7,
                NavigationStatus = Enumerations.NavigationStatus.Locked,
                Latitude = 8,
                Longitude = 9,
                Altitude = 10,
                NorthVelocity = 11,
                EastVelocity = 12,
                DownVelocity = 13,
                Heading = 14,
                Pitch = 15,
                Roll = 16,
                StatusChannel = new StatusChannel0()
                {
                    FullTime = 17,
                    NumberOfSatellites = 18,
                    MainGNSSPositionMode = Enumerations.PositionVelocityOrientationMode.CDGPS,
                    MainGNSSVelocityMode = Enumerations.PositionVelocityOrientationMode.Blanked,
                    DualAntennaSystemsOrientationMode = Enumerations.PositionVelocityOrientationMode.Doppler
                }
            };

            // Copy it
            NcomPacketA pkt2 = new NcomPacketA(pkt1);

            // Are they equal?
            Assert.AreEqual(pkt1, pkt2);
        }

    }
}