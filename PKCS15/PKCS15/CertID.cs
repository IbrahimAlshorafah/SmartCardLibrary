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
    /// Encodes 
    /// CertId ::= SEQUENCE {                 
    /// issuer           GeneralName,                 
    /// serialNumber     INTEGER } 
    /// </summary>
    class CertID
    {
        public string generalName
        { get; private set; }
        public string serialNumber
        { get; private set; }
        public CertID(ASN1 tlv)
        {
            Debug.Assert(tlv.elements == 2);
            generalName = new GeneralName(tlv.get(0)).Name;
            serialNumber = tlv.get(1).Value.ToHex();
        }

        public override string ToString()
        {
            return $@" CertId ={{\n
                         issuer={generalName},\n
                         serialNumber={serialNumber}\n
                       }}";
        }
    }
}
