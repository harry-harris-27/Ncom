using Ncom.Enumerations;
using Ncom.StatusChannels;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Ncom.Tests
{
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

        [Fact]
        public void Test_Equals()
        {
            NcomPacketA packet1 = new NcomPacketA();
            NcomPacketA packet2 = new NcomPacketA();

            NcomPacketA[] pkts = new [] { packet1, packet2 };
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
                        PositionMode = Enumerations.PositionVelocityOrientationMode.CDGPS,
                        VelocityMode = Enumerations.PositionVelocityOrientationMode.Blanked,
                        OrientationMode = Enumerations.PositionVelocityOrientationMode.Doppler
                    }
                };
            }

            packet1.ShouldBeEquivalentTo(packet2);

            // Test 2 different objects
            packet1.ShouldNotBeSameAs(packet2);
            packet2.ShouldNotBeSameAs(packet1);     // Transitive

            // Change one packet
            pkts[1].Time = 0;
            packet1.ShouldBeEquivalentTo(packet2);
            packet2.ShouldBeEquivalentTo(packet1);  // Transitive
        }

        [Fact]
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
                    PositionMode = Enumerations.PositionVelocityOrientationMode.None,
                    VelocityMode = Enumerations.PositionVelocityOrientationMode.None,
                    OrientationMode = Enumerations.PositionVelocityOrientationMode.None
                }
            };

            // Marshal
            byte[] marshalled = ncom.Marshal();

            // Assert
            Enumerable.SequenceEqual(blankNcomBytes, marshalled);
        }

        [Fact]
        public void BlankUnmarshalTest()
        {
            // Unmarshal NCOM data
            NcomPacketA pkt = new NcomPacketA();
            pkt.Unmarshal(blankNcomBytes, 0);

            // Everything should be zero
            pkt.Time.ShouldBe((ushort)0);
            pkt.AccelerationX.ShouldBe(0);
            pkt.AccelerationY.ShouldBe(0);
            pkt.AccelerationZ.ShouldBe(0);
            pkt.AngularRateX.ShouldBe(0);
            pkt.AngularRateY.ShouldBe(0);
            pkt.AngularRateZ.ShouldBe(0);
            pkt.NavigationStatus.ShouldBe((NavigationStatus)0);
            pkt.Latitude.ShouldBe(0);
            pkt.Longitude.ShouldBe(0);
            pkt.Altitude.ShouldBe(0);
            pkt.NorthVelocity.ShouldBe(0);
            pkt.EastVelocity.ShouldBe(0);
            pkt.DownVelocity.ShouldBe(0);
            pkt.Heading.ShouldBe(0);
            pkt.Pitch.ShouldBe(0);
            pkt.Roll.ShouldBe(0);
            pkt.StatusChannel.StatusChannelByte.ShouldBe((byte)0);

            StatusChannel0 chan = pkt.StatusChannel as StatusChannel0;
            chan.FullTime.ShouldBe(0);
            chan.NumberOfSatellites.ShouldBe((byte)0);
            chan.PositionMode.ShouldBe((PositionVelocityOrientationMode)0);
            chan.VelocityMode.ShouldBe((PositionVelocityOrientationMode)0);
            chan.OrientationMode.ShouldBe((PositionVelocityOrientationMode)0);
        }

        [Fact]
        public void CopyConstructorTest()
        {
            // Create an NCOM structure A packet
            NcomPacketA packet1 = new NcomPacketA()
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
                    PositionMode = Enumerations.PositionVelocityOrientationMode.CDGPS,
                    VelocityMode = Enumerations.PositionVelocityOrientationMode.Blanked,
                    OrientationMode = Enumerations.PositionVelocityOrientationMode.Doppler
                }
            };

            // Copy it
            NcomPacketA packet2 = new NcomPacketA(packet1);

            // Are they equal?
            packet1.ShouldBeEquivalentTo(packet2);
        }

    }
}