using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.Utilities
{
    public class DataStream : IDisposable
    {

        /* ---------- Constants ---------------------------------------------------------------/**/


        /* ---------- Private Variables -------------------------------------------------------/**/

        private readonly Endian m_machineEndian;
        private readonly Endian m_streamEndian;

        private readonly MemoryStream m_stream = new MemoryStream();


        /* ---------- Constructors ------------------------------------------------------------/**/

        public DataStream(Endian streamEndian = Endian.Little)
        {
            // Get the endian of the machine the program is running on
            m_machineEndian = BitConverter.IsLittleEndian ? Endian.Little : Endian.Big;

            // Set the endian of the stream
            m_streamEndian = streamEndian;
        }

        public DataStream(byte[] data, int offset, int length, Endian streamEndian = Endian.Little)
            : this(streamEndian)
        {
            // Write the specified data to the underlying stream
            Write(data, offset, length);
        }


        /* ---------- Properties --------------------------------------------------------------/**/
        
        /// <summary>
        /// Gets the Endian of the stream.
        /// </summary>
        public Endian Endian { get { return m_streamEndian; } }

        /// <summary>
        /// Gets the length of the stream in bytes.
        /// </summary>
        public long Length { get { return m_stream.Length; } }


        /* ---------- Public Methods ----------------------------------------------------------/**/

        public void Dispose()
        {
            // Dispose of the underlying stream
            m_stream.Dispose();
        }

        public void Write(byte val)
        {
            Write(new byte[] { val });
        }

        public void Write(ushort val) { Write((short)val); }
        public void Write(short val)
        {

        }

        public void Write(uint val) { Write((int)val); }
        public void Write(int val)
        {

        }
        
        public void Write(float val)
        {

        }

        public void Write(double val)
        {

        }

        public void Write(byte[] data)
        {
            Write(data, 0, data.Length);
        }

        public void Write(byte[] data, int offset, int length)
        {
            m_stream.Write(data, offset, length);
        }
        

        /* ---------- Private Methods ---------------------------------------------------------/**/
        
    }
}
