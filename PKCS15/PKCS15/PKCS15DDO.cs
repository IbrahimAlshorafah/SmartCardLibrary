using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// Shall encode PKCS 15 DDO ASN1 object:
    /// 
    /// DDO ::= SEQUENCE {
    ///     oid OBJECT IDENTIFIER,
    ///     odfPath Path OPTIONAL,
    ///     tokenInfoPath[0] Path OPTIONAL,
    ///     unusedPath[1] Path OPTIONAL,
    ///     ... -- For future extensions
    ///     }
    /// </summary>
    public class PKCS15DDO
    {
        /// <summary>
        /// Provider ID
        /// </summary>
        public string providerId { get; private set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tlv">ASN1 object that contains DDO</param>
        public PKCS15DDO(ASN1 tlv)
        {
            Debug.Assert(tlv.Tag == 0x73);
            var i = 0;
            if (i < tlv.elements)
            {
                var t = tlv.get(i);
                if (t.Tag == ASN1.OBJECT_IDENTIFIER)
                {
                    this.providerId = t.GetValue<string>();
                    i++;
                }
            }
            if (i < tlv.elements)
            {
                var t = tlv.get(i);
                if ((t.Tag & ASN1.SEQUENCE) == ASN1.SEQUENCE)
                {
                    this.odfPath = new PKCS15Path(t);
                    i++;
                }
            }
            if (i < tlv.elements)
            {
                var t = tlv.get(i);
                if (t.Tag == 0xA0)
                {
                    this.ciaInfoPath = new PKCS15Path(t);
                    i++;
                }
            }
        }

        /// <summary>
        /// Converts the DDO object to a readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"{{
            {((this.providerId != null)?$"providerId={ this.providerId },\n":"")}
            {((this.odfPath != null)?$"odfPath={this.odfPath }\n":"")}
            {((this.ciaInfoPath != null)?$"ciaInfoPath={this.ciaInfoPath},\n":"")}
            }}";
        }
        /// <summary>
        /// path to the CIA INFO
        /// </summary>
        public PKCS15Path ciaInfoPath { get; private set; }
        /// <summary>
        /// Path to the PKCS15 ODF 
        /// </summary>
        public PKCS15Path odfPath { get; private set; }
    }
}