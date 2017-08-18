using System.Diagnostics;
using smartcardLib.Logging;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// PublicKeyType ::= CHOICE {
    ///     publicRSAKey PublicKeyObject { PublicRSAKeyAttributes },
    ///     publicECKey[0] PublicKeyObject { PublicECKeyAttributes },
    ///     publicDHKey[1] PublicKeyObject { PublicDHKeyAttributes },
    ///     publicDSAKey[2] PublicKeyObject { PublicDSAKeyAttributes },
    ///     publicKEAKey[3] PublicKeyObject { PublicKEAKeyAttributes },
    ///     … -- For future extensions
    ///     }
    /// PublicKeyObject {KeyAttributes} ::= PKCS15Object {
    /// CommonKeyAttributes, CommonPublicKeyAttributes, KeyAttributes}
    /// </summary>
    public class PKCS15PublicKey : PKCS15CommonPublicKeyAttributes
    {
        ILogger logger;
        private int modulusLength;
        private PKCS15Path value;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="basetlv">ASN1 that holds the public key</param>
        public PKCS15PublicKey(ASN1 basetlv) : base(basetlv)
        {
            this.logger = LoggerFac.GetDefaultLogger();
            var t = basetlv.get(basetlv.elements - 1);

            Debug.Assert(t.Tag == 0xA1);
            Debug.Assert(t.elements >= 1);
            t.setName("typeAttributes");

            t = t.get(0);

            switch (basetlv.Tag)
            {
                case 0x30: this.decodePublicRSAKey(t); break;
                case 0xA0: this.decodePublicECCKey(t); break;
                default:
                    this.type = "PublicKey";
                    logger.LogWarning($"Public Key Object ### Not decoded ### {t}");
                    break;
            }
        }
        /// <summary>
        /// Decode the ECC public Key
        /// </summary>
        /// <param name="tlv">ASN1 of PublicECKeyAttributes</param>
        private void decodePublicECCKey(ASN1 tlv)
        {
            this.type = "PublicECCKey";
            tlv.setName("publicECCKeyAttributes");

            var t = tlv.get(0);
            this.value = new PKCS15Path(t);
            t.setName("value");

            if (tlv.elements > 1)
            {
                t = tlv.get(1);
                if (t.Tag == ASN1.INTEGER)
                {
                    t.setName("reference");
                }
                else
                {
                    t.setName("paramsAndOps");
                }
            }
        }
        /// <summary>
        /// decode the RSA public
        /// </summary>
        /// <param name="tlv">ASN1 of PublicRSAKeyAttributes</param>
        private void decodePublicRSAKey(ASN1 tlv)
        {
            this.type = "PublicRSAKey";
            tlv.setName("publicRSAKeyAttributes");

            var t = tlv.get(0);
            this.value = new PKCS15Path(t);
            t.setName("value");

            t = tlv.get(1);
            Debug.Assert(t.Tag == ASN1.INTEGER);
            this.modulusLength = t.Value.ToSigned();
            t.setName("modulusLength");

            if (tlv.elements > 2)
            {
                t = tlv.get(2);
                if (t.Tag == ASN1.INTEGER)
                {
                    t.setName("reference");
                }
                else
                {
                    t.setName("paramsAndOps");
                }
            }
        }
    }
}