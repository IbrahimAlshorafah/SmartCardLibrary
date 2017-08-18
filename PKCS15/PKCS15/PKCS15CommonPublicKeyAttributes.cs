
using System.Diagnostics;
using smartcardLib.Logging;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// CommonPublicKeyAttributes ::= SEQUENCE {
    /// subjectName Name OPTIONAL,
    /// ...,
    /// trustedUsage[0] Usage OPTIONAL
    /// }
    /// </summary>
    public class PKCS15CommonPublicKeyAttributes : PKCS15CommonKeyAttributes
    {
        private ILogger logger;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="basetlv">ASN with common public key object</param>
        public PKCS15CommonPublicKeyAttributes(ASN1 basetlv) : base(basetlv)
        {
            this.logger = LoggerFac.GetDefaultLogger();
            var t = basetlv.get(2);
            if (t.Tag == 0xA0)
            {
                t.setName("trustedUsage");

                Debug.Assert(t.elements == 1);
                t = t.get(0);
                t.setName("KeyUsage");

                var i = 0;
                if (t.elements > i)
                {
                   logger.LogWarning($"CommonPublikKeyAttributes ### Not decoded ### {t}");
                }
            }
        }


    }
}