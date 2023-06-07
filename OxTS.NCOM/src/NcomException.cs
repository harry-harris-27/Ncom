using System;

namespace OxTS.NCOM
{
    public class NcomException : OxTSException
    {
        public NcomException(string message)
            : base(message)
        { }
    }
}
