namespace OxTS.NCOM.Enumerations
{
    public enum BlendedProcessingMethod : byte
    {
        /// <summary>
        /// Invalid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Generate in real-time by firmware.
        /// </summary>
        RealTime = 1,

        /// <summary>
        /// Post-process simulation of real-time blending chronological constraints.
        /// </summary>
        Simulated = 2,

        /// <summary>
        /// Post-processed in forward time direction.
        /// </summary>
        PostProcessForward = 3,

        /// <summary>
        /// Post-processed in backward time direction.
        /// </summary>
        PostProcessBackward = 4,

        /// <summary>
        /// Post-processed combinating of forward and backward processing results.
        /// </summary>
        PostProcessCombined = 5,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 6
    }
}
