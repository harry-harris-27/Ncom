using Ncom.Enumerations;
using Ncom.StatusChannels;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ncom.Tests
{
    public class NcomPacketATests
    {
        // Values taken from wireshark NCOM decode
        byte[] marshalled = new byte[]
        {
            0xe7,                                               // Sync
            0x81, 0x59,                                         // Time                =  22913
            0xfe, 0x0b, 0x00,                                   // Acceleration X      =  0.3070
            0x22, 0x03, 0x00,                                   // Acceleration Y      =  0.0802
            0xa7, 0x82, 0xfe,                                   // Acceleration Z      = -9.7625
            0xd5, 0xfe, 0xff,                                   // Angular Rate X      = -0.00299
            0xa5, 0x00, 0x00,                                   // Angular Rate Y      =  0.00165
            0xf8, 0xff, 0xff,                                   // Angular Rate Z      = -0.00008
            0x02,                                               // Nav status          =  2
            0x9e,                                               // Checksum 1          =  158
            0x38, 0xbe, 0xcf, 0x2a, 0xbf, 0xef, 0xe4, 0x3f,     // Latitude            =  0.6542659603273426
            0xf6, 0xe7, 0x3a, 0x47, 0x9b, 0x10, 0x01, 0xc0,     // Longitude           = -2.1331086697437014
            0x81, 0xde, 0x93, 0x3f,                             // Altitude            =  1.1552278
            0x5b, 0x00, 0x00,                                   // North velocity      =  0.0091
            0x67, 0x00, 0x00,                                   // East Velocity       =  0.0103
            0x01, 0xff, 0xff,                                   // Down velocity       = -0.0255
            0x00, 0x00, 0x80,                                   // Heading             = -8.388608
            0x00, 0x00, 0x80,                                   // Pitch               = -8.388608
            0x00, 0x00, 0x80,                                   // Roll                = -8.388608
            0x38,                                               // Checksum 2          = 56
            0x00,                                               // Status channel byte = 0
            0x64, 0x3f, 0x2f, 0x01, 0x0f, 0x03, 0x02, 0x00,     // Status channel      = ?
            0x57                                                // Checksum 3          = 87
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
        public void Test_Unmarshal()
        {
            // Arrange
            var ncom = new NcomPacketA();

            // Act
            ncom.Unmarshal(marshalled.AsSpan());

            // Assert
            ncom.Time.ShouldBe((ushort)22913);
            ncom.AccelerationX.ShouldBe(0.3070f, NcomPacketA.AccelerationScaling);
            ncom.AccelerationY.ShouldBe(0.0802f, NcomPacketA.AccelerationScaling);
            ncom.AccelerationZ.ShouldBe(-9.7625f, NcomPacketA.AccelerationScaling);
            ncom.AngularRateX.ShouldBe(-0.00299f, NcomPacketA.AngularRateScaling);
            ncom.AngularRateY.ShouldBe(0.00165f, NcomPacketA.AngularRateScaling);
            ncom.AngularRateZ.ShouldBe(-0.00008f, NcomPacketA.AngularRateScaling);

            ncom.NavigationStatus.ShouldBe((NavigationStatus)2);
            ncom.Checksum1.ShouldBeTrue();

            ncom.Latitude.ShouldBe(0.6542659603273426);
            ncom.Longitude.ShouldBe(-2.1331086697437014);
            ncom.Altitude.ShouldBe(1.1552278f);
            ncom.NorthVelocity.ShouldBe(0.0091f, NcomPacketA.VelocityScaling);
            ncom.EastVelocity.ShouldBe(0.0103f, NcomPacketA.VelocityScaling);
            ncom.DownVelocity.ShouldBe(-0.0255f, NcomPacketA.VelocityScaling);
            ncom.Heading.ShouldBe(-8.388608f, NcomPacketA.OrientationScaling);
            ncom.Pitch.ShouldBe(-8.388608f, NcomPacketA.OrientationScaling);
            ncom.Roll.ShouldBe(-8.388608f, NcomPacketA.OrientationScaling);

            ncom.Checksum2.ShouldBeTrue();
            ncom.StatusChannel.ShouldNotBeNull();
            ncom.StatusChannel.StatusChannelByte.ShouldBe((byte)0);

            ncom.Checksum3.ShouldBeTrue();
        }

        [Fact]
        public void Test_Marshal()
        {
            // Arrange
            var ncom = new NcomPacketA
            {
                Time = 22913,
                AccelerationX = 0.3070f,
                AccelerationY = 0.0802f,
                AccelerationZ = -9.7625f,
                AngularRateX = -0.00299f,
                AngularRateY = 0.00165f,
                AngularRateZ = -0.00008f,
                NavigationStatus = (NavigationStatus)2,
                Latitude = 0.6542659603273426,
                Longitude = -2.1331086697437014,
                Altitude = 1.1552278f,
                NorthVelocity = 0.0091f,
                EastVelocity = 0.0103f,
                DownVelocity = -0.0255f,
                Heading = -8.388608f,
                Pitch = -8.388608f,
                Roll = -8.388608f,
                StatusChannel = new StatusChannel0
                {
                    FullTime = 19873636,
                    NumberOfSatellites = 15,
                    PositionMode = (PositionVelocityOrientationMode)3,
                    VelocityMode = (PositionVelocityOrientationMode)2,
                    OrientationMode = (PositionVelocityOrientationMode)0
                }
            };

            // Act
            var buffer = ncom.Marshal();

            // Assert
            Assert.AssertArraysAreEqual(buffer.AsSpan(), marshalled.AsSpan());
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