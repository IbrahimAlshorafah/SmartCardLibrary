
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using smartcardLib.Core;
using System.Text;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// CommonAuthenticationObjectAttributes ::= SEQUENCE {
    ///  authId Identifier,
    ///  authReference Reference OPTIONAL,
    ///  seIdentifier[0] Reference OPTIONAL,
    ///   ... –- For future extensions
    ///  }
    ///  </summary>
    public class PKCS15CommonAuthenticationObjectAttributes : PKCS15CommonObjectAttributes
    {
        /// <summary>
        /// Authentication ID of CommonAuthenticationObjectAttributes
        /// </summary>
        public byte[] authIdThis { get; private set; }
        /// <summary>
        /// Authentication reference
        /// </summary>
        public int authReference { get; private set; }
        /// <summary>
        /// Secure Element Identifier
        /// </summary>
        public int seIdentifier { get; private set; }
        /// <summary>
        /// constructor from ASN object
        /// </summary>
        /// <param name="tlv">ASN object</param>
        public PKCS15CommonAuthenticationObjectAttributes(ASN1 tlv) : base(tlv)
        {
            authReference = -1;
            seIdentifier = -1;
            Debug.Assert(tlv.IsConstructed);
            var i = 0;
            ASN1 t;

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.OCTET_STRING))
            {
                t.setName("authIdThis");
                this.authIdThis = t.Value;
                i++;
            }

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.INTEGER))
            {
                t.setName("authReference");
                this.authReference = t.Value.ToSigned();
                i++;
            }

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == 0x80))
            {
                t.setName("seIdentifier");
                this.seIdentifier = t.Value.ToSigned();
                i++;
            }


        }
        /// <summary>
        /// return the common authentication object attributes flags as friendly string
        /// </summary>
        /// <returns>readable string</returns>
        public override string getFlagsAsString()
        {
            return $@"{(((flags & 0x80) == 0x80) ? " private" : "") }
         { (((flags & 0x40) == 0x40) ? " modifiable" : "")}
           { (((flags & 0x20) == 0x20) ? " internal" : "")}";
        }
        /// <summary>
        /// converts to a readable string
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            var str = new StringBuilder( "CommonObjectAttributes { \n");
            if (this.authId != null)
            {
                str .Append($"authId={ this.authIdThis.ToHex()},\n");
            }
            if (this.userConsent != null)
            {
                str .Append($"authReference={ this.authReference},\n");
            }
            if (this.accessControlRules != null)
            {
                str .Append($"accessControlRules={this.seIdentifier},\n");
            }
            str .Append("}");
            return str.ToString();
        }
    }
}