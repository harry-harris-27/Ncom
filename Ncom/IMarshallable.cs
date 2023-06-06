using System;

namespace Ncom
{
    /// <summary>
    /// An interface that represents a data structure that can be marshalled/unmarshalled.
    /// </summary>
    public interface IMarshallable
    {
        /// <summary>
        /// Gets the marshalled size of this data structure, expressed in number of bytes.
        /// </summary>
        /// <seealso cref="Marshal(Span{byte})"/>
        /// <seealso cref="Unmarshal(ReadOnlySpan{byte})"/>
        int MarshalledSize { get; }


        /// <summary>
        /// Marshalls the data structure in the specified position in the <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to writed the marshalled bytes to.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// When the <paramref name="buffer"/> is not large enough to contain the marshalled data structure.
        /// </exception>
        /// <seealso cref="MarshalledSize"/>
        /// <seealso cref="Unmarshal(ReadOnlySpan{byte})"/>
        void Marshal(Span<byte> buffer);

        /// <summary>
        /// Unmarshalled the data structure from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer to unmarhsal the data structure from.</param>
        /// <seealso cref="MarshalledSize"/>
        /// <seealso cref="Marshal(Span{byte})"/>
        void Unmarshal(ReadOnlySpan<byte> buffer);
    }
}
