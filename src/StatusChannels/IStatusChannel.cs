using System;

namespace Ncom.StatusChannels
{
    /// <summary>
    /// An interface representing a decoded Batch S (Status Channel) from an <see cref="NcomPacketA"/>.
    /// <para>
    /// Bytes 63 to 70 of an Ncom structure-A packet are collectively call Batch S. Batch S 
    /// contains status channel information from the INS. The information transmitted in Batch S 
    /// is defined by the value of the status byte, which defines the structure of each status 
    /// channel and the information it contains.
    /// </para>
    /// <para>
    /// There are so many status messages it is impossible to transmit them all in a single Ncom 
    /// packet. So instead, the status messages are split into a number of groups called channels,
    /// made up of 8 bytes each - and one channel is inserted into each Ncom packet.
    /// </para>
    /// <para>
    /// It is important to note that the value of the status channel byte does not increase 
    /// incrementally, This is because some status channels are more important than others, and 
    /// need to be transmitted more often. It is also important to note that the channel 
    /// transmission order may change between software versions. However, the channel transmission 
    /// list will repeat approximately once every 200 Ncom structure-A packets.
    /// </para>
    /// </summary>
    public interface IStatusChannel : IEquatable<IStatusChannel>
    {

        /// <summary>
        /// Gets the Status Channel bytes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Byte 62 of an NCOM structure-A packet is used to identify the status message contained 
        /// within Batch S.
        /// </para>
        /// <para>
        /// It is important to note that the value of the <see cref="StatusChannelByte"/> does not 
        /// increase incrementally. This is because some status channels are more important than 
        /// others, and need to be transmitted more often. It is also importants to note that the 
        /// channel transmission order may change between software versions. However, the channel 
        /// transmission list will repeat approximately once every 2-- NCOM structure-A packets.
        /// </para>
        /// </remarks>
        byte StatusChannelByte { get; }


        /// <summary>
        /// Creates a new instance of the <see cref="IStatusChannel"/> logically equal to this 
        /// instance.
        /// </summary>
        /// <returns>A <see cref="IStatusChannel"/> instance</returns>
        IStatusChannel Clone();

        /// <summary>
        /// Marshals this <see cref="IStatusChannel"/> into a <see cref="byte"/> array.
        /// </summary>
        /// <param name="buffer">The byte array to write the marshalled StatusChannel data to.</param>
        /// <param name="offset">The zero-based index indicating the location in the buffer write from.</param>
        /// <remarks>
        /// The <see cref="StatusChannelByte"/> is not included in the marshalled status channel.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="buffer"/> is <see langword="null"/>.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// When <paramref name="offset"/> does not point to an index in the <paramref name="buffer"/>, 
        /// or if the available number of bytes is less than <see cref="NcomPacketA.StatusChannelLength"/>.
        /// </exception>
        void Marshal(byte[] buffer, int offset);

        /// <summary>
        /// Unmarshals the data stored in the specified <paramref name="buffer" /> into this 
        /// <see cref="IStatusChannel" />.
        /// </summary>
        /// <param name="buffer">The byte array containing the marshalled StatusChannel data.</param>
        /// <param name="offset">The zero-based index indicating the location in the buffer read from.</param>
        /// <remarks>
        /// <para>
        /// All marshalled <see cref="IStatusChannel"/> messages have a length of 
        /// <see cref="NcomPacketA.StatusChannelLength"/>. If less than this number of bytes are 
        /// available, an exception will be thrown. This method does not expect the 
        /// <see cref="StatusChannelByte"/> to be included.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">When <paramref name="buffer"/> is <see langword="null"/>.</exception>
        /// <exception cref="IndexOutOfRangeException">
        /// When <paramref name="offset"/> does not point to an index in the <paramref name="buffer"/>, 
        /// or if the available number of bytes is less than <see cref="NcomPacketA.StatusChannelLength"/>.
        /// </exception>
        void Unmarshal(byte[] buffer, int offset);

    }
}
