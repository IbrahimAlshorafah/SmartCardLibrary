
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// The class decodes the following ASN.1 syntax:
    /// CommonCertificateAttributes ::= SEQUENCE {
    /// iD Identifier,
    /// authority BOOLEAN DEFAULT FALSE,
    /// identifier CredentialIdentifier {{ KeyIdentifiers} OPTIONAL,
    /// certHash[0] CertHash OPTIONAL,
    /// trustedUsage[1] Usage OPTIONAL,
    /// identifiers[2] SEQUENCE OF CredentialIdentifier {{ KeyIdentifiers} }
    /// OPTIONAL,
    /// validity[4] Validity OPTIONAL,
    /// ...
    /// } -- Context tag[3] is reserved for historical reasons
    /// NOTE PKCS #15 uses context tag [3].
    /// Usage::= SEQUENCE {
    /// keyUsage KeyUsage OPTIONAL,
    /// extKeyUsage SEQUENCE SIZE(1..MAX) OF OBJECT IDENTIFIER OPTIONAL,
    /// ...
    /// } (WITH COMPONENTS {..., keyUsage PRESENT } | WITH COMPONENTS {..., extKeyUsage PRESENT }) 
    /// </summary>
    public class PKCS15CommonCertificateAttributes : PKCS15CommonObjectAttributes
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 object that contains the CommonCertificateAttributes</param>
        public PKCS15CommonCertificateAttributes(ASN1 tlv) : base(tlv)
        {
            tlv = tlv.get(1);
            tlv.setName("commonCertificateAttributes");
            var i = 0;
            ASN1 t=null;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.OCTET_STRING);
            t.setName("iD");
            this.iD = t.Value;
            i++;

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.BOOLEAN))
            {
                t.setName("authority");
                this.authority = (t.Value.ToSigned() > 0);
                i++;
            }

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.SEQUENCE))
            {
                t.setName("identifier");
                this.identifier = t.Value;
                i++;
            }
        }

        /// <summary>
        /// converts to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"{base.ToString()}
            \nCommonCertificateAttributes {{\n 
            iD={this.iD.ToHex()},\n
            {((this.authority) ? $"authority={this.authority },\n":"")}
            {((this.identifier != null)?$"identifier={this.identifier.ToHex()},\n":"")} }}";
            
        }
        /// <summary>
        /// Authority
        /// </summary>
        public bool authority { get; private set; }
        /// <summary>
        /// ID 
        /// </summary>
        public byte[] iD { get; private set; }
        /// <summary>
        /// Identifier
        /// </summary>
        public byte[] identifier { get; private set; }
    }
}