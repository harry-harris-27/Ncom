using System;

namespace OxTS
{
    /// <summary>
    /// An interface that represents a data structure that can be marshalled/unmarshalled.
    /// </summary>
    public interface IMarshallable
    {
        /// <summary>
        /// Returns the number bytes required to store this marshalled data structure.
        /// </summary>
        /// <returns>The number of bytes required to store this marshalled data structure.</returns>
        int GetMarshalledSize();


        /// <summary>
        /// Attempts to marshal the data structure into the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to write the marshalled data structure to.</param>
        /// <param name="bytes">
        /// If successful, set to the number of bytes written to the <paramref name="buffer"/>;
        /// otherwise <c>-1</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; otherwise <c>false</c>. If successful then
        /// <paramref name="bytes"/> is set to the number of bytes written to the
        /// <paramref name="buffer"/>.
        /// </returns>
        /// <seealso cref="Unmarshal(ReadOnlySpan{byte})"/>
        bool TryMarshal(Span<byte> buffer, out int bytes);

        /// <summary>
        /// Attempts to unmarshal the data structure from the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to read the marshalled data structure from.</param>
        /// <param name="bytes">
        /// If successful, set to the number of bytes read from the <paramref name="buffer"/>;
        /// otherwise <c>-1</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; otherwise <c>false</c>. If successful then
        /// <paramref name="bytes"/> is set to the number of bytes read from the
        /// <paramref name="buffer"/>.
        /// </returns>
        /// <seealso cref="Marshal(Span{byte})"/>
        bool TryUnmarshal(ReadOnlySpan<byte> buffer, out int bytes);
    }
}
