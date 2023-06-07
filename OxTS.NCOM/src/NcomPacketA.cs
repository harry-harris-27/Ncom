using OxTS.NCOM.Enumerations;
using OxTS.NCOM.StatusChannels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OxTS.NCOM
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
        /// Gets or sets the time transmitted, expressed as milliseconds into the current GPS minute [0 - 59,999].
        /// </summary>
        public ushort Time { get => m_Time; set => m_Time = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>X</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationX { get => m_AccelerationX; set => m_AccelerationX = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>Y</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationY { get => m_AccelerationY; set => m_AccelerationY = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>Z</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationZ { get => m_AccelerationZ; set => m_AccelerationZ = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>X</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateX { get => m_AngularRateX; set => m_AngularRateX = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>Y</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateY { get => m_AngularRateY; set => m_AngularRateY = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>Z</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateZ { get => m_AngularRateZ; set => m_AngularRateZ = value; }

        /// <inheritdoc/>
        /// <exception cref="NcomException">
        /// When set to an invalid <see cref="NavigationStatus"/>. See <see cref="NcomPacketA"/> for
        /// permissable values.
        /// </exception>
        public override NavigationStatus NavigationStatus
        {
            get => base.NavigationStatus;
            set
            {
                if (!AllowedNavigationStatus.Contains(value))
                {
                    throw new NcomException("NavigationStatus value not allowed for NCOM Packet A.");
                }

                base.NavigationStatus = value;
            }
        }

        /// <summary>
        /// Indicates whether the first checksum was correct for the most recent
        /// <see cref="Unmarshal(ReadOnlySpan{byte})"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is only set during <see cref="Unmarshal(ReadOnlySpan{byte})"/>; otherwise
        /// defaults to <c>false</c>.
        /// </para>
        /// <para>
        /// This checksum validates:
        /// <list type="=bullet">
        /// <item><see cref="Time"/></item>
        /// <item><see cref="AccelerationX"/></item>
        /// <item><see cref="AccelerationY"/></item>
        /// <item><see cref="AccelerationZ"/></item>
        /// <item><see cref="AngularRateX"/></item>
        /// <item><see cref="AngularRateY"/></item>
        /// <item><see cref="AngularRateZ"/></item>
        /// <item><see cref="NavigationStatus"/></item>
        /// </list>
        /// </para>
        /// </remarks>
        public bool Checksum1 { get; private set; } = false;

        /// <summary>
        /// Gets or sets the latitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Latitude { get => m_Latitude; set => m_Latitude = value; }

        /// <summary>
        /// Gets or sets the longitude of the INS. Expressed in units of radians.
        /// </summary>
        public double Longitude { get => m_Longitude; set => m_Longitude = value; }

        /// <summary>
        /// Gets or sets the altitude of the INS. Expressed in units of metres.
        /// </summary>
        public float Altitude { get => m_Altitude; set => m_Altitude = value; }

        /// <summary>
        /// North velocity in units m/s
        /// </summary>
        public float NorthVelocity { get => m_NorthVelocity; set => m_NorthVelocity = value; }

        /// <summary>
        /// Gets or sets the east velocity in units m/s
        /// </summary>
        public float EastVelocity { get => m_EastVelocity; set => m_EastVelocity = value; }

        /// <summary>
        /// Gets or sets the down velocity in units m/s
        /// </summary>
        public float DownVelocity { get => m_DownVelocity; set => m_DownVelocity = value; }

        /// <summary>
        /// Gets or sets the heading in units of radians. Range +-PI
        /// </summary>
        public float Heading { get => m_Heading; set => m_Heading = value; }

        /// <summary>
        /// Gets or sets the pitch in units of radians. Range +-PI/2
        /// </summary>
        public float Pitch { get => m_Pitch; set => m_Pitch = value; }

        /// <summary>
        /// Gets or sets the roll in units of radians. Range +-PI
        /// </summary>
        public float Roll { get => m_Roll; set => m_Roll = value; }

        /// <summary>
        /// Indicates whether the second checksum was correct for the most recent
        /// <see cref="Unmarshal(ReadOnlySpan{byte})"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is only set during <see cref="Unmarshal(ReadOnlySpan{byte})"/>; otherwise
        /// defaults to <c>false</c>.
        /// </para>
        /// <para>
        /// This checksum validates all values validated by <see cref="Checksum1"/> in addition to:
        /// <list type="=bullet">
        /// <item><see cref="Latitude"/></item>
        /// <item><see cref="Longitude"/></item>
        /// <item><see cref="Altitude"/></item>
        /// <item><see cref="NorthVelocity"/></item>
        /// <item><see cref="EastVelocity"/></item>
        /// <item><see cref="DownVelocity"/></item>
        /// <item><see cref="Heading"/></item>
        /// <item><see cref="Pitch"/></item>
        /// <item><see cref="Roll"/></item>
        /// </list>
        /// </para>
        /// </remarks>
        public bool Checksum2 { get; set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="IStatusChannel"/> information.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Setting to null with assume an invalid status channel of 255.
        /// </para>
        /// </remarks>
        public IStatusChannel? StatusChannel { get; set; } = null;


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

            // Skip over nav. status and checksum 1
            offset += 2;


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

            // Skip over checksum 2
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
                buffer.Slice(offset, StatusChannels.StatusChannel.StatusChannelLength).Fill(0);
            }

            // Calculate and insert checksums
            buffer[Checksum1Index] = CalculateChecksum(buffer.Slice(1, Checksum1Index - 1));
            buffer[Checksum2Index] = CalculateChecksum(buffer.Slice(1, Checksum2Index - 1));
            buffer[Checksum3Index] = CalculateChecksum(buffer.Slice(1, Checksum3Index - 1));
        }

        /// <inheritdoc/>
        public override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            base.Unmarshal(buffer);

            // offset points to pkt[1] to skip over the Sync byte.
            int offset = 1;

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

            // Skip a over the nav status byte. Handled in base implementation
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

            byte statusChannelByte = buffer[offset++];
            StatusChannel = StatusChannelFactory.Create(statusChannelByte);
            StatusChannel?.Unmarshal(buffer.Slice(offset));
        }

    }
}
