using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using smartcardLib.Core;
using smartcardLib.Logging;
namespace smartcardLib.PKCS15
{
    /// <summary>
    /// X509CertificateAttributes ::= SEQUENCE {
    ///value ObjectValue { Certificate },
    /// subject Name OPTIONAL,
    ///issuer[0] Name OPTIONAL,
    ///serialNumber CertificateSerialNumber OPTIONAL,
    ///... -- For future extensions
    ///}
    /// </summary>
    class PKCS15Certificate : PKCS15CommonCertificateAttributes
    {
        private ILogger logger;
        /// <summary>
        /// Serial number
        /// </summary>
        public byte[] serialNumber { get; private set; }
        /// <summary>
        /// Path for the certificate
        /// </summary>
        public PKCS15Path path { get; private set; }
        /// <summary>
        /// Can only read X509 Certificate currently
        /// </summary>
        /// <param name="tlv"></param>
        public PKCS15Certificate(ASN1 tlv) : base(tlv)
        {
            this.logger = LoggerFac.GetDefaultLogger();
            var t = tlv.get(2);
            Debug.Assert(t.Tag == 0xA1);
            Debug.Assert(t.elements == 1);
            t.setName("typeAttributes");
            t = t.get(0);
            t.setName("certificateAttributes");
            switch (tlv.Tag)
            {
                case 0x30: this.decodeX509Certificate(t); break;
                default:
                    this.type = "Certificate"; 
                    logger.LogWarning($"PKCS15 Certificate ### Not decoded ### {t}");
                    break;
            }
            logger.LogTrace($"Certificate: { t}");
        }
        /// <summary>
        /// decodes a x509 certificate attributes
        /// </summary>
        /// <param name="tlv">ASN object to be decoded</param>
        private void decodeX509Certificate(ASN1 tlv)
        {
            this.type = "X509Certificate";
            tlv.setName("x509CertificateAttributes");
            var t = tlv.get(0);
            t.setName("value");
            if (t.Tag == ASN1.SEQUENCE) this.path = new PKCS15Path(t);
            var i = 1;
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.SEQUENCE))
            {
                t.setName("subject");
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == 0xA0))
            {
                t.setName("issuer");
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.INTEGER))
            {
                t.setName("serialNumber");
                this.serialNumber = t.Value;
                i++;
            }
        }
    }
}