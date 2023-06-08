namespace OxTS.NCOM
{
    public enum PositionVelocityOrientationMode : byte
    {
        /// <summary>
        /// The GPX is not able to make this measurement.
        /// </summary>
        None = 0,

        /// <summary>
        /// The GPS system is solving ambiguities and searching for a valid solution.
        /// </summary>
        Search = 1,

        /// <summary>
        /// The GPS measurement is based on a Doppler measurement.
        /// </summary>
        Doppler = 2,

        /// <summary>
        /// Standard Positioning Service (SPS), the GPS measurement has no additional external
        /// corrections.
        /// </summary>
        SPS = 3,

        /// <summary>
        /// The GPS measurement used pseudo-range differential corrections.
        /// </summary>
        Differential = 4,

        /// <summary>
        /// The GPS measurement used L1 carrier-phase differential corrections to give a floating
        /// ambiguity solution.
        /// </summary>
        RTKFloat = 5,

        /// <summary>
        /// The GPS measurement used L1/L2 carrier-phase differential corrections to give an
        /// integer ambiguity solution.
        /// </summary>
        RTKInteger = 6,

        /// <summary>
        /// The GPS measurement used SBAS corrections.
        /// </summary>
        WASS = 7,

        /// <summary>
        /// The GPS measurement used OmniSTAR VBS corrections.
        /// </summary>
        OmniSTAR = 8,

        /// <summary>
        /// The GPS measurement used OmniSTAR HP corrections.
        /// </summary>
        OmniSTARHP = 9,

        /// <summary>
        /// No data.
        /// </summary>
        NoData = 10,

        /// <summary>
        /// Blanked.
        /// </summary>
        Blanked = 11,

        /// <summary>
        /// Doppler GPS measurement post-processed.
        /// </summary>
        DopplerPP = 12,

        /// <summary>
        /// SPS GPS measurement post-processed.
        /// </summary>
        SPSPP = 13,

        /// <summary>
        /// Differential GPS measurement post-processed.
        /// </summary>
        DifferentialPP = 14,

        /// <summary>
        /// RTK Float GPS measurement post-processed.
        /// </summary>
        RTKFloatPP = 15,

        /// <summary>
        /// RTK Integer GPS measurement post-processed.
        /// </summary>
        RTKIntegerPP = 16,

        /// <summary>
        /// The GPS mesasurement used OmniSTAR XP corrections.
        /// </summary>
        OmniSTARXP = 17,

        /// <summary>
        /// The GPS measurement used real time Canada wide DGPS service.
        /// </summary>
        CDGPS = 18,

        /// <summary>
        /// Not recognised.
        /// </summary>
        NotRecognised = 19,

        /// <summary>
        /// Computed by combining raw Doppler measurements.
        /// </summary>
        GXDoppler = 20,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements.
        /// </summary>
        GXSPS = 21,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and differential corrections.
        /// </summary>
        GXDifferential = 22,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and L1 carrier-phase measurements
        /// and differential corrections.
        /// </summary>
        GXFloat = 23,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and L1/L2 carrier-phase
        /// measurements and differential corrections.
        /// </summary>
        GXInteger = 24,

        /// <summary>
        /// Single-satellite updates from raw Doppler measurements.
        /// </summary>
        IXDoppler = 25,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements.
        /// </summary>
        IXSPS = 26,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and differential
        /// corrections.
        /// </summary>
        IXDifferential = 27,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and L1 carrier-phase
        /// measurements and differential corrections.
        /// </summary>
        IXFloat = 28,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and L1/L2 carrier-phase
        /// measurements and differential corrections.
        /// </summary>
        IXInteger = 29,

        /// <summary>
        /// Converging PPP (Precise Point Positioning) from global PPP corrections.
        /// </summary>
        PPPConverging = 30,

        /// <summary>
        /// Converged PPP (Precise Point Positioning) from global PPP corrections
        /// </summary>
        PPP = 31,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 32


        //
        // Bytes 33 - 255 are reserved.
        //
    }
}
