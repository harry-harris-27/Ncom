using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM
{
    public interface IMarshal
    {

        int MarshalLength { get; }


        byte[] Marshal();

        bool Unmarshal(byte[] buffer, int offset);

    }
}
