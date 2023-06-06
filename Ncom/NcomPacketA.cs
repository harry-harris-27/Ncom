using Ncom.Enumerations;
using Ncom.StatusChannels;
using System;
using System.Linq;

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

        internal const float AccelerationScaling    = 1e-4f;
        internal const float AngularRateScaling     = 1e-5f;
        internal const float VelocityScaling        = 1e-4f;
        internal const float OrientationScaling     = 1e-6f;

        internal const int Checksum1Index           = 22;
        internal const int Checksum2Index           = 61;

        private const ushort MaxTimeValue           = 59999;

        private static readonly NavigationStatus[] AllowedNavigationStatus = new NavigationStatus[]
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

        // Property backing stores
        private ushort m_Time = 0;
        private float m_AccelerationX = 0;
        private float m_AccelerationY = 0;
        private float m_AccelerationZ = 0;
        private float m_AngularRateX = 0;
        private float m_AngularRateY = 0;
        private float m_AngularRateZ = 0;
        private double m_Latitude = 0;
        private double m_Longitude = 0;
        private float m_Altitude = 0;
        private float m_NorthVelocity = 0;
        private float m_EastVelocity = 0;
        private float m_DownVelocity = 0;
        private float m_Heading = 0;
        private float m_Pitch = 0;
        private float m_Roll = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="NcomPacketA"/> class.
        /// </summary>
        public NcomPacketA() { }

        /// <summary>
        /// Copy constructor. Initializes a new instance of the <see cref="NcomPacketA"/> class,
        /// logically equivalent to to the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">
        /// The source <see cref="NcomPacketA"/> with which to initialise this instance.
        /// </param>
        public NcomPacketA(NcomPacketA source) : base(source)
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

            this.NorthVelocity = source.NorthVelocity;
            this.EastVelocity = source.EastVelocity;
            this.DownVelocity = source.DownVelocity;

            this.Heading = source.Heading;
            this.Pitch = source.Pitch;
            this.Roll = source.Roll;

            this.Checksum2 = source.Checksum2;

            this.StatusChannel = source.StatusChannel?.Clone();
        }


        /// <summary>
        /// Time is transmitted as milliseconds into the current GPS minute. Range = [0 - 59,999].
        /// </summary>
        public ushort Time { get => m_Time; set => m_Time = value; }

        /// <summary>
        /// Acceleration <i>X</i> is the host object's acceleration in the <i>X</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationX { get => m_AccelerationX; set => m_AccelerationX = value; }

        /// <summary>
        /// Acceleration <i>Y</i> is the host object's acceleration in the <i>Y</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationY { get => m_AccelerationY; set => m_AccelerationY = value; }

        /// <summary>
        /// Acceleration <i>Z</i> is the host object's acceleration in the <i>Z</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationZ { get => m_AccelerationZ; set => m_AccelerationZ = value; }

        /// <summary>
        /// Angular rate <i>X</i> is the host object's angular rate about its <i>X</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateX { get => m_AngularRateX; set => m_AngularRateX = value; }

        /// <summary>
        /// Angular rate <i>Y</i> is the host object's angular rate about its <i>Y</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateY { get => m_AngularRateY; set => m_AngularRateY = value; }

        /// <summary>
        /// Angular rate <i>Z</i> is the host object's angular rate about its <i>Z</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateZ { get => m_AngularRateZ; set => m_AngularRateZ = value; }

        /// <summary>
        /// Checksum 1 allows the software to verify the integrity of bytes 1-21. The sync byte if
        /// ignored. In low-latency applications the inertial measurements in Batch A can be used
        /// to update a previous solution without waiting for the rest of the packet to be
        /// received.
        /// </summary>
        public bool Checksum1 { get; set; } = false;

        /// <summary>
        /// The latitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Latitude { get => m_Latitude; set => m_Latitude = value; }

        /// <summary>
        /// The longitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Longitude { get => m_Longitude; set => m_Longitude = value; }

        /// <summary>
        /// The altitude of the INS. Expressed in units of radians.
        /// </summary>
        public float Altitude { get => m_Altitude; set => m_Altitude = value; }

        /// <summary>
        /// North velocity in units m/s
        /// </summary>
        public float NorthVelocity { get => m_NorthVelocity; set => m_NorthVelocity = value; }

        /// <summary>
        /// East velocity in units m/s
        /// </summary>
        public float EastVelocity { get => m_EastVelocity; set => m_EastVelocity = value; }

        /// <summary>
        /// Down velocity in units m/s
        /// </summary>
        public float DownVelocity { get => m_DownVelocity; set => m_DownVelocity = value; }

        /// <summary>
        /// Heading in units of radians. Range +-PI
        /// </summary>
        public float Heading { get => m_Heading; set => m_Heading = value; }

        /// <summary>
        /// Pitch in units of radians. Range +-PI/2
        /// </summary>
        public float Pitch { get => m_Pitch; set => m_Pitch = value; }

        /// <summary>
        /// Roll in units of radians. Range +-PI
        /// </summary>
        public float Roll { get => m_Roll; set => m_Roll = value; }

        /// <summary>
        /// Checksum 2 allows the software to verify the integrity of bytes 1-60. The sync byte if
        /// ignored. For medium-latency output, the full navigation solution is now available
        /// without waiting for the status updated in the rest of the packet.
        /// </summary>
        public bool Checksum2 { get; set; } = false;

        /// <summary>
        /// Bytes 63 to 70 of an NCOM structure-A packet are collectively called Batch S. Batch S
        /// contains status channel information from the INS. The information transmitted in Batch
        /// S is defined by the value of the status channel byte, which defines the structure of
        /// each status channel and the information it contains.
        /// </summary>
        public IStatusChannel StatusChannel { get; set; } = null;


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

        /// <inheritdoc/>
        public override void Marshal(Span<byte> buffer)
        {
            base.Marshal(buffer);

            int offset = 1;


            // Batch A
            // --------

            // Insert Time
            ByteHandling.Marshal(buffer, ref offset, (ushort)(Time % MaxTimeValue));

            // Insert accelerations
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AccelerationX / AccelerationScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AccelerationY / AccelerationScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AccelerationZ / AccelerationScaling));

            // Insert Angular Rates
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AngularRateX / AngularRateScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AngularRateY / AngularRateScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(AngularRateZ / AngularRateScaling));

            // Skip over nav. status
            offset++;

            // Calculate and insert Checksum 1
            buffer[offset] = CalculateChecksum(buffer.Slice(1, offset - 2));
            offset++;


            // Batch B
            // --------

            // Insert Latitude, Longitude and altitude
            ByteHandling.Marshal(buffer, ref offset, Latitude);
            ByteHandling.Marshal(buffer, ref offset, Longitude);
            ByteHandling.Marshal(buffer, ref offset, Altitude);

            // Insert Velocities
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(NorthVelocity / VelocityScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(EastVelocity / VelocityScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(DownVelocity / VelocityScaling));

            // Insert Heading, Pitch and Roll
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(Heading / OrientationScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(Pitch / OrientationScaling));
            ByteHandling.MarshalInt24(buffer, ref offset, (int)(Roll / OrientationScaling));

            // Calculate and insert Checksum 2
            buffer[offset] = CalculateChecksum(buffer.Slice(1, offset - 2));
            offset++;


            // Batch S
            // --------

            // Insert Status Channel
            if (StatusChannel != null)
            {
                buffer[offset++] = StatusChannel.StatusChannelByte;
                ByteHandling.Marshal(buffer, ref offset, StatusChannel);
            }
            else
            {
                buffer[offset++] = 0xFF;
                offset += StatusChannels.StatusChannel.StatusChannelLength;
            }
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            int offset = 1;

            // offset points to pkt[1]

            // Check the navigation status byte
            if (!AllowedNavigationStatus.Contains(NavigationStatus))
            {
                //throw new Exception("Invalid NavigationStatus value");
            }

            // Batch A
            // --------

            // Extract the time
            ByteHandling.Unmarshal(buffer, ref offset, out m_Time);

            // Extract Accelerations
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AccelerationX, AccelerationScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AccelerationY, AccelerationScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AccelerationZ, AccelerationScaling);

            // Extract Angular Rates
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AngularRateX, AngularRateScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AngularRateY, AngularRateScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_AngularRateZ, AngularRateScaling);

            // Skip a bit for the nav status byte
            offset++;

            // Extract checksum 1
            Checksum1 = CalculateChecksum(buffer.Slice(1, 21)) == buffer[offset++];


            // Batch B
            // --------

            // Extract Latitude, Longitude and altitude
            ByteHandling.Unmarshal(buffer, ref offset, out m_Latitude);
            ByteHandling.Unmarshal(buffer, ref offset, out m_Longitude);
            ByteHandling.Unmarshal(buffer, ref offset, out m_Altitude);

            // Extract velocities
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_NorthVelocity, VelocityScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_EastVelocity, VelocityScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_DownVelocity, VelocityScaling);

            // Extract Heading, Pitch and Roll
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_Heading, OrientationScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_Pitch, OrientationScaling);
            ByteHandling.UnmarshalInt24(buffer, ref offset, out m_Roll, OrientationScaling);

            // Extract checksum 2
            Checksum2 = CalculateChecksum(buffer.Slice(1, 60)) == buffer[offset++];


            // Batch S
            // --------

            // Extract status channel (and status channel byte)
            byte statusChannelByte = buffer[offset++];
            StatusChannel = StatusChannelFactory.Unmarshal(statusChannelByte, buffer.Slice(offset));
        }

        /// <inheritdoc/>
        protected override bool IsEqual(NcomPacket _pkt)
        {
            NcomPacketA pkt = (_pkt as NcomPacketA)!;

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
