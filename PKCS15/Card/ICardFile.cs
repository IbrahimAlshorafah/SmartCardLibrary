namespace smartcardLib.Card
{
    /// <summary>
    /// Interface for card file
    /// </summary>
    public interface ICardFile
    {
        /// <summary>
        /// Length of EF file if EF
        /// </summary>
        uint Length { get; }
        /// <summary>
        /// Is transparent EF
        /// </summary>
        /// <returns>true if transparent EF false otherwise</returns>
        bool IsTransparent();
        /// <summary>
        /// Read the full EF
        /// </summary>
        /// <returns>data in EF </returns>
        byte[] ReadBinary();
        /// <summary>
        /// Read a specific length from the EF
        /// </summary>
        /// <param name="length">size of data to be read</param>
        /// <returns>data read</returns>
        byte[] ReadBinary(uint length);
        /// <summary>
        /// Read from a specific offset with a specific length
        /// </summary>
        /// <param name="offset">offset to read from</param>
        /// <param name="length">length to be read from that offset</param>
        /// <returns>data returned</returns>
        /// <remarks>
        /// if offset is null  it is assumed to be 0
        /// if length is null the full file length will be read
        /// </remarks>
        byte[] ReadBinary(uint? offset, uint? length);
        /// <summary>
        /// Read a record for non transparent file
        /// </summary>
        /// <param name="length">length to be read</param>
        /// <returns>data read</returns>
        byte[] ReadRecord(int length);
    }
}