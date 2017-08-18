using System;
using System.Runtime.Serialization;
namespace smartcardLib.Card
{
    /// <summary>
    /// smartcard exception
    /// </summary>
    internal class SmartCardException : Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SmartCardException()
        {
        }
        /// <summary>
        /// constructor with a message
        /// </summary>
        /// <param name="message">message</param>
        public SmartCardException(string message) : base(message)
        {
        }
        /// <summary>
        /// constructor with a message and inner exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public SmartCardException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// De-serialize constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">context streaming</param>
        protected SmartCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}