using System;

namespace OxTS.RCOM
{
    public enum NCOMProvenance
    {
        UDP     = 0x00,
        COM1    = 0x01,
        COM2    = 0x02,
        COM3    = 0x03,
        COM4    = 0x04,
        COM5    = 0x05,
        COM6    = 0x06,
        COM7    = 0x07,
        COM8    = 0x08,
        COM9    = 0x09,
        COM10   = 0x0A,
        COM11   = 0x0B,
        COM12   = 0x0C,
        COM13   = 0x0D,
        COM14   = 0x0E,
        COM15   = 0x0F,
        File    = 0x10,

        Invalid = 0xFF
    }
}
