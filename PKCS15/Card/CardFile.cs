using System;
using System.IO;
using System.Linq;
namespace smartcardLib.Card
{
    /// <summary>
    /// Mocked card as file system card
    /// </summary>
    public class MockedCardFile : ICardFile
    {
        private ICardController cardController;
        private string selectedFilePath;
        private byte[] AllFileData;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="cardController">Card controller</param>
        /// <param name="filePath">path to file location</param>
        public MockedCardFile(ICardController cardController, string filePath)
        {
            this.cardController = cardController;
            this.selectedFilePath = filePath.Replace(":",@"\");
            if(selectedFilePath.StartsWith(@"\"))
            {
                selectedFilePath.Remove(0, 1);
            }
            AllFileData = File.ReadAllBytes(selectedFilePath);
            Length = (uint) AllFileData.Length;
           
        }
        /// <summary>
        /// Length of file
        /// </summary>
        public uint Length { get; internal set; }
        /// <summary>
        /// always transparent
        /// </summary>
        /// <returns>true</returns>
        public bool IsTransparent()
        {
            return true;
        }
        /// <summary>
        /// Read all data in file
        /// </summary>
        /// <returns>File data</returns>
        public byte[] ReadBinary()
        {
            return AllFileData;
        }
        /// <summary>
        /// Read a specific length from the EF
        /// </summary>
        /// <param name="length">size of data to be read</param>
        /// <returns>data read</returns>
        public byte[] ReadBinary(uint length)
        {
            return AllFileData.Take((int)length).ToArray();
        }
        /// <summary>
        /// Read a record for non transparent file
        /// </summary>
        /// <param name="length">length to be read</param>
        /// <returns>data read</returns>
        public byte[] ReadRecord(int length)
        {
            throw new NotImplementedException();
        }
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
        public byte[] ReadBinary(uint? offset, uint? length)
        {
            int off = (int)( offset ?? 0);
            int len = (int)(length ?? Length);
            return AllFileData.Skip(off).Take(len).ToArray();
        }
    }
}