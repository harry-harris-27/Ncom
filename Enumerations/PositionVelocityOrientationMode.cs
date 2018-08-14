using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Enumerations
{
    public enum PositionVelocityOrientationMode
    {
        None = 0,
        Search = 1,
        Doppler = 2,
        SPS = 3,
        Differential = 4,
        RTK_Float = 5,
        RTK_Integer = 6

        // ...
    }
}
