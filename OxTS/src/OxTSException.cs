using System;

namespace OxTS
{
    public abstract class OxTSException : Exception
    {
        public OxTSException(string message)
            : base(message)
        { }
    }
}
