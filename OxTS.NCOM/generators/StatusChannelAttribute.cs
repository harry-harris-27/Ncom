using System;

namespace OxTS.NCOM.Generators
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class StatusChannelAttribute : Attribute
    {
        public StatusChannelAttribute(byte statusChannel)
        {
            StatusChannel = statusChannel;
        }

        public byte StatusChannel { get; }
    }
}
