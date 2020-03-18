﻿using Ncom.Enumerations;
using Ncom.StatusChannels;
using Ncom.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncom
{
    /// <summary>
    /// Represents a single OxTS NCOM structure-A packet.
    /// </summary>
    /// <remarks>
    /// For an NCOM packet to be classified as a structure-A NCOM packet, the 
    /// <see cref="NcomPacket.NavigationStatus"/> must be equal to one of the following:
    /// <list type="bullet">
    /// <item><see cref="NavigationStatus.Invalid" /></item>
    /// <item><see cref="NavigationStatus.RawIMUMeasurements" /></item>
    /// <item><see cref="NavigationStatus.Initialising" /></item>
    /// <item><see cref="NavigationStatus.Locking" /></item>
    /// <item><see cref="NavigationStatus.Locked" /></item>
    /// <item><see cref="NavigationStatus.ReservedForUnlocked" /></item>
    /// <item><see cref="NavigationStatus.ExpiredFirmware" /></item>
    /// <item><see cref="NavigationStatus.StatusOnly" /></item>
    /// <item><see cref="NavigationStatus.TriggerPacketWhileInitialising" /></item>
    /// <item><see cref="NavigationStatus.TriggerPacketWhileLocking" /></item>
    /// <item><see cref="NavigationStatus.TriggerPacketWhileLocked" /></item>
    /// </list>
    /// All other values of <see cref="NcomPacket.NavigationStatus"/> must be decoded as 
    /// <see cref="NcomPacketB" />.
    /// </remarks>
    /// <seealso cref="Ncom.NcomPacket" />
    /// <seealso cref="Ncom.NcomPacketB" />
    public class NcomPacketA : NcomPacket
    {

        /* ---------- Constants ---------------------------------------------------------------/**/

        internal const float ACCELERATION_SCALING   = 1e-4f;
        internal const float ANGULAR_RATE_SCALING   = 1e-5f;
        internal const float VELOCITY_SCALING       = 1e-4f;
        internal const float ORIENTATION_SCALING    = 1e-6f;

        internal const int CHECKSUM_1_INDEX = 22;
        internal const int CHECKSUM_2_INDEX = 61;

        internal const int STATUS_CHANNEL_LENGTH = 8;

        private const ushort MAX_TIME_VALUE = 59999;
        
        private static readonly NavigationStatus[] ALLOWED_NAV_STATUSES = new NavigationStatus[]
        {
            NavigationStatus.Invalid,                           // 0
            NavigationStatus.RawIMUMeasurements,                // 1
            NavigationStatus.Initialising,                      // 2
            NavigationStatus.Locking,                           // 3
            NavigationStatus.Locked,                            // 4
            NavigationStatus.ReservedForUnlocked,               // 5
            NavigationStatus.ExpiredFirmware,                   // 6
            NavigationStatus.BlockedFirmware,                   // 7
            NavigationStatus.StatusOnly,                        // 10
            NavigationStatus.TriggerPacketWhileInitialising,    // 20
            NavigationStatus.TriggerPacketWhileLocking,         // 21
            NavigationStatus.TriggerPacketWhileLocked           // 22
        };


        /* ---------- Private variables -------------------------------------------------------/**/


        /* ---------- Constructors ------------------------------------------------------------/**/

        /// <summary>
        /// Initializes a new instance of the <see cref="NcomPacketA"/> class.
        /// </summary>
        public NcomPacketA() : this(null) { }

        /// <summary>
        /// Copy constructor. Initializes a new instance of the <see cref="NcomPacketA"/> class,
        /// logically equivalent to to the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="NcomPacketA"/> with which to initialise this instance.
        /// </param>
        public NcomPacketA(NcomPacketA source) : base(source)
        {
            if (source != null)
            {
                this.Time = source.Time;

                this.AccelerationX = source.AccelerationX;
                this.AccelerationY = source.AccelerationY;
                this.AccelerationZ = source.AccelerationZ;

                this.AngularRateX = source.AngularRateX;
                this.AngularRateY = source.AngularRateY;
                this.AngularRateZ = source.AngularRateZ;

                this.Checksum1 = source.Checksum1;

                this.Latitude = source.Latitude;
                this.Longitude = source.Longitude;
                this.Altitude = source.Altitude;

                this.DownVelocity = source.DownVelocity;
                this.EastVelocity = source.EastVelocity;
                this.NorthVelocity = source.NorthVelocity;

                this.Heading = source.Heading;
                this.Pitch = source.Pitch;
                this.Roll = source.Roll;

                this.Checksum2 = source.Checksum2;

                this.StatusChannel = StatusChannelFactory.Copy(source.StatusChannel);
            }
        }


        /* ---------- Properties --------------------------------------------------------------/**/

        /// <summary>
        /// Acceleration <i>X</i> is the host object's acceleration in the <i>X</i>-direction (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// ms^-2.
        /// </summary>
        public float AccelerationX { get; set; } = 0;

        /// <summary>
        /// Acceleration <i>Y</i> is the host object's acceleration in the <i>Y</i>-direction (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// ms^-2.
        /// </summary>
        public float AccelerationY { get; set; } = 0;

        /// <summary>
        /// Acceleration <i>Z</i> is the host object's acceleration in the <i>Z</i>-direction (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// ms^-2.
        /// </summary>
        public float AccelerationZ { get; set; } = 0;

        /// <summary>
        /// The altitude of the INS. Expressed in units of radians.
        /// </summary>
        public float Altitude { get; set; } = 0;

        /// <summary>
        /// Angular rate <i>X</i> is the host object's angular rate about its <i>X</i>-axis (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// radians per second
        /// </summary>
        public float AngularRateX { get; set; } = 0;

        /// <summary>
        /// Angular rate <i>Y</i> is the host object's angular rate about its <i>Y</i>-axis (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// radians per second
        /// </summary>
        public float AngularRateY { get; set; } = 0;

        /// <summary>
        /// Angular rate <i>Z</i> is the host object's angular rate about its <i>Z</i>-axis (i.e. 
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of 
        /// radians per second
        /// </summary>
        public float AngularRateZ { get; set; } = 0;

        /// <summary>
        /// Checksum 1 allows the software to verify the integrity of bytes 1-21. The sync byte if 
        /// ignored. In low-latency applications the inertial measurements in Batch A can be used 
        /// to update a previous solution without waiting for the rest of the packet to be 
        /// received.
        /// </summary>
        public bool Checksum1 { get; set; } = false;

        /// <summary>
        /// Checksum 2 allows the software to verify the integrity of bytes 1-60. The sync byte if 
        /// ignored. For medium-latency output, the full navigation solution is now available 
        /// without waiting for the status updated in the rest of the packet.
        /// </summary>
        public bool Checksum2 { get; set; } = false;

        /// <summary>
        /// Down velocity in units m/s
        /// </summary>
        public float DownVelocity { get; set; } = 0;

        /// <summary>
        /// East velocity in units m/s
        /// </summary>
        public float EastVelocity { get; set; } = 0;

        /// <summary>
        /// Heading in units of radians. Range +-PI
        /// </summary>
        public float Heading { get; set; } = 0;

        /// <summary>
        /// The latitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Latitude { get; set; } = 0;

        /// <summary>
        /// The longitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Longitude { get; set; } = 0;

        /// <summary>
        /// North velocity in units m/s
        /// </summary>
        public float NorthVelocity { get; set; } = 0;

        /// <summary>
        /// Pitch in units of radians. Range +-PI/2
        /// </summary>
        public float Pitch { get; set; } = 0;

        /// <summary>
        /// Roll in units of radians. Range +-PI
        /// </summary>
        public float Roll { get; set; } = 0;

        /// <summary>
        /// Bytes 63 to 70 of an NCOM structure-A packet are collectively called Batch S. Batch S 
        /// contains status channel information from the INS. The information transmitted in Batch 
        /// S is defined by the value of the status channel byte, which defines the structure of 
        /// each status channel and the information it contains.
        /// </summary>
        public StatusChannel StatusChannel { get; set; } = new StatusChannel0();

        /// <summary>
        /// Time is transmitted as milliseconds into the current GPS minute. Range = [0 - 59,999]. 
        /// </summary>
        public ushort Time { get; set; } = 0;


        /* ---------- Public methods ----------------------------------------------------------/**/

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        /// <remarks>
        /// The hash code generation process ignores properties <see cref="Checksum1" />, 
        /// <see cref="Checksum2" /> and <see cref="NcomPacket.Checksum3" />, since they are not 
        /// used when testing for equality.
        /// </remarks>
        public override int GetHashCode()
        {
            int hash = 13;
            int mul = 7;

            hash = (hash * mul) + base.GetHashCode();

            hash = (hash * mul) + Time.GetHashCode();

            hash = (hash * mul) + AccelerationX.GetHashCode();
            hash = (hash * mul) + AccelerationY.GetHashCode();
            hash = (hash * mul) + AccelerationZ.GetHashCode();

            hash = (hash * mul) + AngularRateX.GetHashCode();
            hash = (hash * mul) + AngularRateY.GetHashCode();
            hash = (hash * mul) + AngularRateZ.GetHashCode();

            hash = (hash * mul) + Latitude.GetHashCode();
            hash = (hash * mul) + Longitude.GetHashCode();
            hash = (hash * mul) + Altitude.GetHashCode();

            hash = (hash * mul) + NorthVelocity.GetHashCode();
            hash = (hash * mul) + EastVelocity.GetHashCode();
            hash = (hash * mul) + DownVelocity.GetHashCode();

            hash = (hash * mul) + Heading.GetHashCode();
            hash = (hash * mul) + Pitch.GetHashCode();
            hash = (hash * mul) + Roll.GetHashCode();

            hash = (hash * mul) + (StatusChannel != null ? StatusChannel.GetHashCode() : 0);

            return hash;
        }

        /// <summary>
        /// Marshals this <see cref="NcomPacketA"/> into a byte array of length 
        /// <see cref="NcomPacket.PacketLength"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="byte"/> array of length equal to <see cref="NcomPacket.PacketLength"/>.
        /// </returns>
        public override byte[] Marshal()
        {
            // Call base method to get byte array to marshal into
            byte[] buffer = base.Marshal();
            int p = 1;


            // Batch A
            // --------

            // Insert Time
            Array.Copy(BitConverter.GetBytes(Time % MAX_TIME_VALUE), 0, buffer, p, 2);
            p += 2;

            // Insert acceleration X
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AccelerationX / ACCELERATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert acceleration Y
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AccelerationY / ACCELERATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert acceleration Z
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AccelerationZ / ACCELERATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Angular Rate X
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AngularRateX / ANGULAR_RATE_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Angular Rate Y
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AngularRateY / ANGULAR_RATE_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Angular Rate Z
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(AngularRateZ / ANGULAR_RATE_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Skip over nav. status
            p++;

            // Calculate and insert Checksum 1
            buffer[p] = CalculateChecksum(buffer, 1, p - 2);
            p++;


            // Batch B
            // --------

            // Insert Latitude
            Array.Copy(BitConverter.GetBytes(Latitude), 0, buffer, p, 8);
            p += 8;

            // Insert Longitude
            Array.Copy(BitConverter.GetBytes(Longitude), 0, buffer, p, 8);
            p += 8;

            // Insert Altitude
            Array.Copy(BitConverter.GetBytes(Altitude), 0, buffer, p, 4);
            p += 4;

            // Insert North Velocity
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(NorthVelocity / VELOCITY_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert East Velocity
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(EastVelocity / VELOCITY_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Down Velocity
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(DownVelocity / VELOCITY_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Heading
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(Heading / ORIENTATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Pitch
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(Pitch / ORIENTATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Insert Roll
            Array.Copy(ByteHandling.CastInt32To3Bytes((int)(Roll / ORIENTATION_SCALING)), 0, buffer, p, 3);
            p += 3;

            // Calculate and insert Checksum 2
            buffer[p] = CalculateChecksum(buffer, 1, p - 2);
            p++;


            // Batch S
            // --------

            // Insert Status Channel
            Array.Copy(
                StatusChannel != null ? StatusChannel.Marshal() : new byte[StatusChannel.StatusChannelLength], 
                0, 
                buffer, 
                p, 
                StatusChannel.StatusChannelLength
            );
            p += StatusChannel.StatusChannelLength;

            // Calculate and insert Checksum 3
            buffer[p] = CalculateChecksum(buffer, 1, p - 2);

            // Return the marshalled NCOM packet
            return buffer;
        }

        /// <summary>
        /// Unmarshals the data stored in the specified <paramref name="buffer" /> into this
        /// <see cref="NcomPacket" />. If no marshalled NCOM packet can be found,
        /// <see langword="false" /> is returned.
        /// </summary>
        /// <param name="buffer">The byte array containing the marshalled NCOM packet.</param>
        /// <param name="offset">The zero-based index indicating the location in the buffer to start looking for a sync
        /// byte from.</param>
        /// <returns>
        /// <see langword="false" /> if no marshalled NCOM packet can be found, otherwise
        /// <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the first byte of the buffer is not <see cref="NcomPacket.SyncByte" /> then the method will look
        /// for the first occurance of the sync byte.
        /// </para>
        /// <para>
        /// The <paramref name="offset" /> can be greater than the length of the
        /// <paramref name="buffer" /> without throwing an exception, returning
        /// <see langword="false" />. This allows for easy unmarshalling of multiple consecutive
        /// packets, e.g. <see langword="while" /> loops.
        /// </para>
        /// </remarks>
        public override bool Unmarshal(byte[] buffer, int offset)
        {
            // Check that the buffer is not null
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            // Check that the offset is in the expected range
            if (offset < 0) throw new IndexOutOfRangeException("Offset in buffer can not be less than zero");

            // Seek the sync byte
            while (offset <= buffer.Length - PacketLength)
            {
                // Have we found the sync byte?
                if (buffer[offset++] == SyncByte)
                {
                    int pkt_start = offset - 1;

                    // offset points to pkt[1]

                    // Call base method
                    if (!base.Unmarshal(buffer, offset - 1)) return false;

                    // Check the navigation status byte
                    if (!ALLOWED_NAV_STATUSES.Contains(NavigationStatus)) return false;


                    // Batch A
                    // --------

                    // Extract the time
                    Time = BitConverter.ToUInt16(buffer, offset);
                    offset += 2;

                    // Extract the Acceleration X
                    AccelerationX = ByteHandling.Cast3BytesToInt32(buffer, offset) * ACCELERATION_SCALING;
                    offset += 3;

                    // Extract the Acceleration Y
                    AccelerationY = ByteHandling.Cast3BytesToInt32(buffer, offset) * ACCELERATION_SCALING;
                    offset += 3;

                    // Extract the Acceleration Z
                    AccelerationZ = ByteHandling.Cast3BytesToInt32(buffer, offset) * ACCELERATION_SCALING;
                    offset += 3;

                    // Extract the Angular Rate X
                    AngularRateX = ByteHandling.Cast3BytesToInt32(buffer, offset) * ANGULAR_RATE_SCALING;
                    offset += 3;

                    // Extract the Angular Rate Y
                    AngularRateY = ByteHandling.Cast3BytesToInt32(buffer, offset) * ANGULAR_RATE_SCALING;
                    offset += 3;

                    // Extract the Angular Rate Z
                    AngularRateZ = ByteHandling.Cast3BytesToInt32(buffer, offset) * ANGULAR_RATE_SCALING;
                    offset += 3;

                    // Skip a bit for the nav status byte
                    offset++;

                    // Extract checksum 1
                    Checksum1 = CalculateChecksum(buffer, pkt_start + 1, 21) == buffer[offset];
                    offset++;


                    // Batch B
                    // --------

                    // Extract Latitude
                    Latitude = BitConverter.ToDouble(buffer, offset);
                    offset += 8;

                    // Extract Longitude
                    Longitude = BitConverter.ToDouble(buffer, offset);
                    offset += 8;

                    // Extract altitude
                    Altitude = BitConverter.ToSingle(buffer, offset);
                    offset += 4;

                    // Extract North velocity
                    NorthVelocity = ByteHandling.Cast3BytesToInt32(buffer, offset) * VELOCITY_SCALING;
                    offset += 3;

                    // Extract East velocity
                    EastVelocity = ByteHandling.Cast3BytesToInt32(buffer, offset) * VELOCITY_SCALING;
                    offset += 3;

                    // Extract Down velocity
                    DownVelocity = ByteHandling.Cast3BytesToInt32(buffer, offset) * VELOCITY_SCALING;
                    offset += 3;

                    // Extract Heading
                    Heading = ByteHandling.Cast3BytesToInt32(buffer, offset) * ORIENTATION_SCALING;
                    offset += 3;

                    // Extract Pitch
                    Pitch = ByteHandling.Cast3BytesToInt32(buffer, offset) * ORIENTATION_SCALING;
                    offset += 3;

                    // Extract Roll
                    Roll = ByteHandling.Cast3BytesToInt32(buffer, offset) * ORIENTATION_SCALING;
                    offset += 3;

                    // Extract checksum 2
                    Checksum2 = CalculateChecksum(buffer, pkt_start + 1, 60) == buffer[offset];
                    offset++;


                    // Batch S
                    // --------

                    // Extract status channel (and status channel byte)
                    StatusChannel = StatusChannelFactory.ProcessStatusChannel(buffer, offset);
                    
                    // Unmarshalled OK, return true
                    return true;
                }
            }

            // Couldn't find packet, return false;
            return false;
        }


        /* ---------- Internal methods ---------------------------------------------------------/**/

        /// <inheritdoc />
        internal override bool IsEqual(NcomPacket _pkt)
        {
            NcomPacketA pkt = _pkt as NcomPacketA;

            return base.IsEqual(pkt)
                && this.Time == pkt.Time
                && this.AccelerationX == pkt.AccelerationX
                && this.AccelerationY == pkt.AccelerationY
                && this.AccelerationZ == pkt.AccelerationZ
                && this.AngularRateX == pkt.AngularRateX
                && this.AngularRateY == pkt.AngularRateY
                && this.AngularRateZ == pkt.AngularRateZ
                && this.Latitude == pkt.Latitude
                && this.Longitude == pkt.Longitude
                && this.Altitude == pkt.Altitude
                && this.NorthVelocity == pkt.NorthVelocity
                && this.EastVelocity == pkt.EastVelocity
                && this.DownVelocity == pkt.DownVelocity
                && this.Heading == pkt.Heading
                && this.Pitch == pkt.Pitch
                && this.Roll == pkt.Roll
                && (this.StatusChannel == null ? pkt.StatusChannel == null : this.StatusChannel.Equals(pkt.StatusChannel));
        }

    }
}
