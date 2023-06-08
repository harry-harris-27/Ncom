using System;
using System.Linq;

namespace OxTS.NCOM
{
    /// <summary>
    /// Represents a single OxTS NCOM structure-A packet.
    /// </summary>
    /// <remarks>
    /// For an NCOM packet to be classified as a structure-A NCOM packet, the
    /// <see cref="NCOMPacket.NavigationStatus"/> must be equal to one of the following:
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
    /// All other values of <see cref="NCOMPacket.NavigationStatus"/> must be decoded as
    /// <see cref="NCOMPacketB" />.
    /// </remarks>
    /// <seealso cref="NCOM.NCOMPacket" />
    /// <seealso cref="NCOM.NCOMPacketB" />
    public class NCOMPacketA : NCOMPacket
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
        private readonly Vector3 m_Acceleration = new Vector3(AccelerationScaling);
        private readonly Vector3 m_AngularRate = new Vector3(AngularRateScaling);
        private readonly Vector3 m_Velocity = new Vector3(VelocityScaling);
        private readonly Vector3 m_Orientation = new Vector3(OrientationScaling);

        private ushort m_Time = 0;
        private double m_Latitude = 0;
        private double m_Longitude = 0;
        private float m_Altitude = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="NCOMPacketA"/> class.
        /// </summary>
        public NCOMPacketA() { }


        /// <summary>
        /// Gets or sets the time transmitted, expressed as milliseconds into the current GPS minute [0 - 59,999].
        /// </summary>
        public ushort Time { get => m_Time; set => m_Time = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>X</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationX { get => m_Acceleration.X; set => m_Acceleration.X = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>Y</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationY { get => m_Acceleration.Y; set => m_Acceleration.Y = value; }

        /// <summary>
        /// Gets or sets the component of the host object's acceleration in the <i>Z</i>-direction (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// ms^-2.
        /// </summary>
        public float AccelerationZ { get => m_Acceleration.Z; set => m_Acceleration.Z = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>X</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateX { get => m_AngularRate.X; set => m_AngularRate.X = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>Y</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateY { get => m_AngularRate.Y; set => m_AngularRate.Y = value; }

        /// <summary>
        /// Gets or sets the component of the host object's angular rate about its <i>Z</i>-axis (i.e.
        /// after the IMU to host attitude matrix has been applied). It is expressed in units of
        /// radians per second
        /// </summary>
        public float AngularRateZ { get => m_AngularRate.Z; set => m_AngularRate.Z = value; }

        /// <inheritdoc/>
        /// <exception cref="NCOMException">
        /// When set to an invalid <see cref="NavigationStatus"/>. See <see cref="NCOMPacketA"/> for
        /// permissable values.
        /// </exception>
        public override NavigationStatus NavigationStatus
        {
            get => base.NavigationStatus;
            set
            {
                if (!AllowedNavigationStatus.Contains(value))
                {
                    throw new NCOMException("NavigationStatus value not allowed for NCOM Packet A.");
                }

                base.NavigationStatus = value;
            }
        }

        /// <summary>
        /// Indicates whether the first checksum was correct for the most recent
        /// <see cref="TryUnmarshal(ReadOnlySpan{byte}, out int)"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is only set during <see cref="TryUnmarshal(ReadOnlySpan{byte}, out int)"/>; otherwise
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
        public float NorthVelocity { get => m_Velocity.X; set => m_Velocity.X = value; }

        /// <summary>
        /// Gets or sets the east velocity in units m/s
        /// </summary>
        public float EastVelocity { get => m_Velocity.Y; set => m_Velocity.Y = value; }

        /// <summary>
        /// Gets or sets the down velocity in units m/s
        /// </summary>
        public float DownVelocity { get => m_Velocity.Z; set => m_Velocity.Z = value; }

        /// <summary>
        /// Gets or sets the heading in units of radians. Range +-PI
        /// </summary>
        public float Heading { get => m_Orientation.X; set => m_Orientation.X = value; }

        /// <summary>
        /// Gets or sets the pitch in units of radians. Range +-PI/2
        /// </summary>
        public float Pitch { get => m_Orientation.Y; set => m_Orientation.Y = value; }

        /// <summary>
        /// Gets or sets the roll in units of radians. Range +-PI
        /// </summary>
        public float Roll { get => m_Orientation.Z; set => m_Orientation.Z = value; }

        /// <summary>
        /// Indicates whether the second checksum was correct for the most recent
        /// <see cref="TryUnmarshal(ReadOnlySpan{byte}, out int)"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is only set during <see cref="TryUnmarshal(ReadOnlySpan{byte}, out int)"/>; otherwise
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
        protected override void Marshal(Span<byte> buffer)
        {
            int offset = 1;

            // Insert Time
            ByteHandling.Marshal(buffer, ref offset, (ushort)(Time % MaxTimeValue));

            m_Acceleration.TryMarshal(buffer, ref offset, out _);
            m_AngularRate.TryMarshal(buffer, ref offset, out _);

            // Skip over nav. status and checksum 1
            offset += 2;

            // Insert Latitude, Longitude and altitude
            ByteHandling.Marshal(buffer, ref offset, Latitude);
            ByteHandling.Marshal(buffer, ref offset, Longitude);
            ByteHandling.Marshal(buffer, ref offset, Altitude);

            m_Velocity.TryMarshal(buffer, ref offset, out _);
            m_Orientation.TryMarshal(buffer, ref offset, out _);

            // Skip over checksum 2
            offset++;

            // Insert Status Channel
            if (StatusChannel is not null && StatusChannel.TryMarshal(buffer.Slice(offset + 1)))
            {
                buffer[offset++] = StatusChannel.StatusChannelByte;
            }
            else
            {
                buffer[offset++] = 0xFF;
                buffer.Slice(offset, OxTS.StatusChannel.StatusChannelLength).Fill(0);
            }

            // Calculate and insert checksums
            buffer[Checksum1Index] = ByteHandling.CalculateChecksum(buffer.Slice(1, Checksum1Index - 1));
            buffer[Checksum2Index] = ByteHandling.CalculateChecksum(buffer.Slice(1, Checksum2Index - 1));
        }

        /// <inheritdoc/>
        protected override void Unmarshal(ReadOnlySpan<byte> buffer)
        {
            // offset points to pkt[1] to skip over the Sync byte.
            int offset = 1;

            // Extract the time
            ByteHandling.Unmarshal(buffer, ref offset, out m_Time);

            m_Acceleration.TryUnmarshal(buffer, ref offset, out _);
            m_AngularRate.TryUnmarshal(buffer, ref offset, out _);

            // Skip a over the nav status byte. Handled in base implementation
            offset++;

            // Extract checksum 1
            Checksum1 = ByteHandling.CalculateChecksum(buffer.Slice(1, 21)) == buffer[offset++];

            // Extract Latitude, Longitude and altitude
            ByteHandling.Unmarshal(buffer, ref offset, out m_Latitude);
            ByteHandling.Unmarshal(buffer, ref offset, out m_Longitude);
            ByteHandling.Unmarshal(buffer, ref offset, out m_Altitude);

            m_Velocity.TryUnmarshal(buffer, ref offset, out _);
            m_Orientation.TryUnmarshal(buffer, ref offset, out _);

            // Extract checksum 2
            Checksum2 = ByteHandling.CalculateChecksum(buffer.Slice(1, 60)) == buffer[offset++];

            // Batch S
            byte statusChannelByte = buffer[offset++];
            StatusChannel = NCOMStatusChannelFactory.Create(statusChannelByte);
            if (StatusChannel is not null && !StatusChannel.TryUnmarshal(buffer, ref offset, out _))
            {
                StatusChannel = null;
            }
        }

    }
}
