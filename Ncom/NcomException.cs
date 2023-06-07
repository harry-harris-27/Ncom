using System;

namespace Ncom
{
    public class NcomException : Exception
    {
        public NcomException(string message)
            : base(message)
        { }
    }
}
