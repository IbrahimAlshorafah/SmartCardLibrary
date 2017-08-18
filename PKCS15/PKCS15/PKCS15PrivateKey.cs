using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using smartcardLib.Logging;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{

    /// <summary>
    /// This class adds private key attributes to the common private key attribute class
    /// The class supports RSA and ECC keys.
    /// RSA keys are decoded from the following ASN.1 structure:
    /// PrivateRSAKeyAttributes::= SEQUENCE {
    /// value Path,
    /// modulusLength INTEGER, --modulus length in bits, e.g.
    /// }
    /// 
    /// PrivateECKeyAttributes ::= SEQUENCE {
    /// value ObjectValue { ECPrivateKey },
    /// keyInfo KeyInfo { Parameters, PublicKeyOperations }
    /// OPTIONAL,
    /// ... -- For future extensions
    /// }
    /// ECPrivateKey::= INTEGER
    /// </summary>
    public class PKCS15PrivateKey : PKCS15CommonPrivateKeyAttributes
    {
        private ILogger logger;
        /// <summary>
        /// path
        /// </summary>
        public PKCS15Path path { get; private set; }
        /// <summary>
        /// Modulus length
        /// </summary>
        public int modulusLength;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 hold Private Key</param>
        public PKCS15PrivateKey(ASN1 tlv) : base(tlv)
        {

            this.logger = LoggerFac.GetDefaultLogger();
            var t = tlv.get(tlv.elements - 1);
            Debug.Assert(t.Tag == 0xA1);
            Debug.Assert(t.elements >= 1);
            t.setName("typeAttributes");
            t = t.get(0);
            switch (tlv.Tag)
            {
                case 0x30: this.decodePrivateRSAKey(t); break;
                case 0xA0: this.decodePrivateECCKey(t); break;
                default:
                    this.type = "PrivateKey";
                    logger.LogWarning($"Private Key  Type ### Not decoded only RSA and ECC ### {t}");
                    break;
            }
        }
        /// <summary>
        /// Decode ECC private key
        /// </summary>
        /// <param name="basetlv">ASN1 object holding the Elliptic Curve Cryptography private key</param>
        private void decodePrivateECCKey(ASN1 basetlv)
        {
            this.type = "privateECCKey";
            basetlv.setName("privateECCKeyAttributes");
            var t = basetlv.get(0);
            this.path = new PKCS15Path(t);
            t.setName("value");
            if (basetlv.elements > 1)
            {
                t = basetlv.get(1);
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
        /// Decode RSA private key
        /// </summary>
        /// <param name="basetlv">ASN1 object holfing the RSA private key</param>
        private void decodePrivateRSAKey(ASN1 basetlv)
        {
            this.type = "privateRSAKey";
            basetlv.setName("privateRSAKeyAttributes");
            var t = basetlv.get(0);
            this.path = new PKCS15Path(t);
            t.setName("value");
            t = basetlv.get(1);
            Debug.Assert(t.Tag == ASN1.INTEGER);
            this.modulusLength = t.Value.ToSigned();
            t.setName("modulusLength");
            if (basetlv.elements > 2)
            {
                t = basetlv.get(2);
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
        /// converts the private key parameters to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"{this.type} : {{ 
            {base.ToString()}
            typeAttributes {{
                value indirect : path : {{
                   {this.path.ToString()}
                }},
                modulusLength {this.modulusLength.ToString()}
            }}";
        }
    }
}