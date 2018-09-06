using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NCOM.StatusChannels
{
    public static class StatusChannelFactory
    {

        private const int STATUS_CHANNEL_LENGTH = 8;


        public static StatusChannel ProcessStatusChannel(byte[] buffer, int offset)
        {
            // Check the length of the available part of the buffer
            if (buffer.Length - offset < STATUS_CHANNEL_LENGTH + 1)
            {
                throw new ArgumentException("Buffer is smaller than required");
            }

            // Create a variable to store the status channel in
            StatusChannel channel;

            // Create the status channel
            switch (buffer[offset])
            {
                case 0:
                    channel = new StatusChannel0();
                    break;
                case 1:
                    channel = new StatusChannel1();
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 91:
                case 92:
                default:
                    channel = null;
                    break;
            }

            // If not null, marshal the bytes into the status channel obj
            channel?.Unmarshal(buffer, offset + 1);

            // Return the status channel object
            return channel;
        }
        
    }
}
