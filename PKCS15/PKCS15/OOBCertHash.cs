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
    /// OOBCertHash ::= SEQUENCE {                      
    ///     hashAlg     [0] AlgorithmIdentifier     OPTIONAL,                      
    ///     certId      [1] CertId                  OPTIONAL,                      
    ///     hashVal         BIT STRING                      -- hashVal is calculated over the DER encoding of the                      
    ///     -- self-signed certificate with the identifier certID.       
    ///     } 
    /// </summary>
    class OOBCertHash
    {
        /// <summary>
        /// Algorithm Identifier
        /// </summary>
        public AlgorithmIdentifier hashAlg { get; private set; }
        /// <summary>
        /// certificate Id
        /// </summary>
        public CertID certId { get; private set; }
        /// <summary>
        ///   hashVal is calculated over the DER encoding of the
        ///  self-signed certificate with the identifier certID.      
        /// </summary>
        public string hashVal { get; private set; }
        /// <summary>
        /// constructor from asn
        /// </summary>
        /// <param name="tlv"></param>
        public OOBCertHash(ASN1 tlv)
        {
            Debug.Assert(tlv.elements >= 1);
            if(tlv.elements > 1)
            {
                foreach(var elm in tlv.Children)
                {
                    if(tlv.TagNumber == 0)
                    {
                        hashAlg = new AlgorithmIdentifier(elm);

                    }
                    else if(tlv.TagNumber == 1)
                    {
                        certId = new CertID(elm);
                    }

                }
                hashVal = tlv.get(tlv.elements - 1).GetValue<string>();
            }
            else
            {
                hashVal = tlv.get(0).GetValue<string>();
            }
        }

        /// <summary>
        /// Converts the CertHash to readable string 
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@" CertHash = {{
                        {((hashAlg!=null)?$"hashAlg={hashAlg.ToString()},\n":"")}
                        {((certId!=null)?$"certId={certId.ToString()},\n":"")}
                        hasVal={hashVal}\n
                       }}
                    ";
        }
    }
}
