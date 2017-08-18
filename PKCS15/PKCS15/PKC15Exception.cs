using System;
using System.Runtime.Serialization;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// PKCS15 exception
    /// </summary>
    [Serializable]
    internal class PKC15Exception : Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        public PKC15Exception()
        {
        }
        /// <summary>
        /// constructor with a message
        /// </summary>
        /// <param name="message">message</param>
        public PKC15Exception(string message) : base(message)
        {
        }
        /// <summary>
        /// constructor with a message and inner exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public PKC15Exception(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// De-serialize constructor
        /// </summary>
        /// <param name="info">serialization info</param>
        /// <param name="context">context streaming</param>
        protected PKC15Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}