using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// Class the holds AlgorithmIdentifier object
    ///    AlgorithmIdentifier ::= SEQUENCE {
    ///    algorithm OBJECT IDENTIFIER,
    ///    parameters ANY DEFINED BY algorithm OPTIONAL }
    ///</summary>
    class AlgorithmIdentifier
    {
        string algorithm;
        ASN1 parameters;

        /// <summary>
        /// Construct from ASN1 
        /// </summary>
        /// <param name="tlv">ASN1 instance of AlgorithmIdentifier </param>
        public AlgorithmIdentifier(ASN1 tlv)
        {
            Debug.Assert(tlv.Tag == ASN1.SEQUENCE);
            algorithm = tlv.get(0).GetValue<string>();
            if(tlv.elements >1)
            {
                parameters = tlv.get(1);
            }
        }
        /// <summary>
        /// Converts algorithm identifier to a readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"AlgorithmIdentifier {{\n
                   algorithm: {algorithm}\n
                   parameters:{parameters.ToString()}\n
                   }}";
        }
    }
}
