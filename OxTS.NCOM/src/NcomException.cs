using System;

namespace OxTS.NCOM
{
    public class NCOMException : OxTSException
    {
        public NCOMException(string message)
            : base(message)
        { }
    }
}
