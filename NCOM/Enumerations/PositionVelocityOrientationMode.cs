using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Enumerations
{
    public enum PositionVelocityOrientationMode
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
        RTK_Float = 5,

        /// <summary>
        /// The GPS measurement used L1/L2 carrier-phase differential corrections to give an 
        /// integer ambiguity solution.
        /// </summary>
        RTK_Integer = 6,

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
        OmniSTAR_HP = 9,

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
        Doppler_PP = 12,

        /// <summary>
        /// SPS GPS measurement post-processed.
        /// </summary>
        SPS_PP = 13,

        /// <summary>
        /// Differential GPS measurement post-processed.
        /// </summary>
        Differential_PP = 14,

        /// <summary>
        /// RTK Float GPS measurement post-processed.
        /// </summary>
        RTK_Float_PP = 15,

        /// <summary>
        /// RTK Integer GPS measurement post-processed.
        /// </summary>
        RTK_Integer_PP = 16,

        /// <summary>
        /// The GPS mesasurement used OmniSTAR XP corrections.
        /// </summary>
        OmniSTAR_XP = 17,

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
        GX_Doppler = 20,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements.
        /// </summary>
        GX_SPS = 21,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and differential corrections.
        /// </summary>
        GX_Differential = 22,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and L1 carrier-phase measurements 
        /// and differential corrections.
        /// </summary>
        GX_Float = 23,

        /// <summary>
        /// Computed by combining raw pseudo-range measurements and L1/L2 carrier-phase 
        /// measurements and differential corrections.
        /// </summary>
        GX_Integer = 24,

        /// <summary>
        /// Single-satellite updates from raw Doppler measurements.
        /// </summary>
        IX_Doppler = 25,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements.
        /// </summary>
        IX_SPS = 26,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and differential 
        /// corrections.
        /// </summary>
        IX_Differential = 27,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and L1 carrier-phase 
        /// measurements and differential corrections.
        /// </summary>
        IX_Float = 28,

        /// <summary>
        /// Single-satellite updates from raw pseudo-range measurements and L1/L2 carrier-phase 
        /// measurements and differential corrections.
        /// </summary>
        IX_Integer = 29,

        /// <summary>
        /// Converging PPP (Precise Point Positioning) from global PPP corrections.
        /// </summary>
        PPP_Converging = 30,

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
