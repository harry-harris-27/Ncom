namespace OxTS.NCOM
{
    public enum NavigationStatus : byte
    {

        /// <summary>
        /// All quantities in the packet are invalid
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Raw IMU measurements.
        /// <para>These are output at roughly 10 Hz before the system is initialised. They are useful for
        /// checking the communication link and verifying the operation of the accelerometers and angular rate sensors in
        /// the laboratory. In this mode only the accelerations and angular rates are valid, they are not calibrated or to
        /// any specification. The information in the other fields is invalid.
        /// </para>
        /// </summary>
        RawIMUMeasurements = 1,

        /// <summary>
        /// Initialising.
        /// <para>When GPS time becomes available the system starts the initialisation process. The strapdown navigator and
        /// Kalman filter are allocated, but do not yet run. Angular rates and accelerations during this time are output 1 s
        /// in arrears. There will be a 1 s pause at the start of initialisation where no output will be made (while the
        /// system fills the buffers). The system has to run 1 s in arrears at this time in order to synchronise the GNSS
        /// data with the inertial data and perform the initialisation checks.
        /// </para>
        /// <para>
        /// During the initialising mode the time, acceleration and angular rate fields will be valid. Approximate (very
        /// inaccurate) position, velocity and angles may be output.
        /// </para>
        /// </summary>
        Initialising = 2,

        /// <summary>
        /// Locking
        /// <para>
        /// The system will move to locking mode when the conditions for initialising are correct. To initialise, GPS time,
        /// position and velocity must be available; roll and pitch must be estimated (assumed approximately zero with the
        /// “vehicle level” option); heading must be estimated from forward velocity, dual antenna static initialisation or
        /// user command.
        /// </para>
        /// <para>
        /// In locking mode the system runs in arrears but catches up by 0.1 s every 1 s; locking mode lasts 10 s. During
        /// locking mode the outputs are not real-time, but all fields are valid.
        /// </para>
        /// </summary>
        Locking = 3,

        /// <summary>
        /// Locked
        /// <para>
        /// In Locked mode the system is outputting real-time data with the specified latency guaranteed. All fields are
        /// valid.
        /// </para>
        /// </summary>
        Locked = 4,

        /// <summary>
        /// Reserved for “unlocked” navigation output. Do not use any values from this message.
        /// </summary>
        ReservedForUnlocked = 5,

        /// <summary>
        /// Expired firmware: this is output if the firmware is time limited and the expiry time has passed.
        /// </summary>
        ExpiredFirmware = 6,

        /// <summary>
        /// Blocked firmware: this is output if the firmware has been blocked (by password protection).
        /// </summary>
        BlockedFirmware = 7,

        /// <summary>
        /// Status Only
        /// <para>
        /// Only the Batch S part of the message (Bytes 63–70) should be decoded. This is used at the start of some
        /// logged NCOM files in order to save a complete set of status messages before the real data begins.
        /// </para>
        /// </summary>
        StatusOnly = 10,

        /// <summary>
        /// Internal Use.
        /// <para>
        /// Do not use any values from this message.
        /// </para>
        /// </summary>
        InternalUse = 11,

        /// <summary>
        /// Trigger packet while "initialising" (see <see cref="NavigationStatus.Initialising"/> for more details).
        /// <para>
        /// The Status channel (byte 62) will have a value of 24 (falling trigger), 43 (rising trigger), 65 (output
        /// trigger), 79 (falling trigger 2), 80 (rising trigger 2) or 81 (output trigger 2), depending on what
        /// triggers the packet.
        /// </para>
        /// <para>
        /// This packet is generated following a short variable delay (less than 0.02 s) after the corresponding
        /// navigation data output. The Time output is that of the trigger event.
        /// </para>
        /// </summary>
        TriggerPacketWhileInitialising = 20,

        /// <summary>
        /// Trigger packet while "locking" (see <see cref="NavigationStatus.Locking"/> for more details).
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Status channel (byte 62) will have a value of 24 (falling trigger), 43 (rising trigger), 65 (output
        /// trigger), 79 (falling trigger 2), 80 (rising trigger 2) or 81 (output trigger 2), depending on what
        /// triggers the packet.
        /// </para>
        /// <para>
        /// This packet is generated following a short variable delay (less than 0.02 s) after the corresponding
        /// navigation data output. The Time output is that of the trigger event.
        /// </para>
        /// </remarks>
        TriggerPacketWhileLocking = 21,

        /// <summary>
        /// Trigger packet while "locked" (see <see cref="NavigationStatus.Locked"/> for more details).
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Status channel (byte 62) will have a value of 24 (falling trigger), 43 (rising trigger), 65 (output
        /// trigger), 79 (falling trigger 2), 80 (rising trigger 2) or 81 (output trigger 2), depending on what
        /// triggers the packet.
        /// </para>
        /// <para>
        /// This packet is generated following a short variable delay (less than 0.02 s) after the corresponding
        /// navigation data output. The Time output is that of the trigger event. The latency of the trigger output
        /// is variable (by up to 0.02 s) due to the short variable delay.
        /// </para>
        /// </remarks>
        TriggerPacketWhileLocked = 22,

        Unknown = 255
    }
}
