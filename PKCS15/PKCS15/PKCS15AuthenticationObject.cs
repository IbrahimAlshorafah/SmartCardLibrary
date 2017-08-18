using System;
using smartcardLib.Core;
using System.Text;
using System.Diagnostics;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// AuthenticationType ::= CHOICE {
    /// pin AuthenticationObject { PinAttributes },
    /// ...,
    /// biometricTemplate[0] AuthenticationObject { BiometricAttributes },
    /// authKey[1] AuthenticationObject { AuthKeyAttributes },
    /// external[2] AuthenticationObject { ExternalAuthObjectAttributes }
    /// }
    /// AuthenticationObject {AuthObjectAttributes} ::= PKCS15Object {
    /// CommonAuthenticationObjectAttributes, NULL, AuthObjectAttributes}
    /// </summary>
    /// <remarks>
    /// Only support for password authentication object
    /// </remarks>
    class PKCS15AuthenticationObject : PKCS15CommonAuthenticationObjectAttributes
    {
        /// <summary>
        /// Password authentication object
        /// </summary>
        public PKCS15PasswordAuthenticationObject pwd;
        /// <summary>
        /// constructor from 
        /// </summary>
        /// <param name="basetlv"></param>
        public PKCS15AuthenticationObject(ASN1 basetlv) : base(basetlv)
        {
            var t = basetlv.get(basetlv.elements - 1);
            Debug.Assert(t.Tag == 0xA1);
            Debug.Assert(t.elements == 1);
            t.setName("typeAttribute");
            t = t.get(0);
            switch (t.Tag)
            {
                case 0x30:
                    this.pwd = new PKCS15PasswordAuthenticationObject(t);
                    break;
                default:
                    //TODO: parse different type of authentication objects
                    throw new Exception("### Unsupported authentication object type : " + t);
              
            }
        }

        /// <summary>
        /// To Readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder( ((PKCS15CommonAuthenticationObjectAttributes)this).ToString());
            str.Append("\nCommonAuthenticationObjectAttributes { ");

            if (authId != null)
            {
                str.Append($"authIdThis= {this.authId.ToHex() },\n");
            }

            if (this.authReference != -1)
            {
                str.Append($"authReference={ this.authReference },\n");
            }

            if (this.seIdentifier != -1)
            {
                str.Append($"seIdentifier={ this.seIdentifier },\n");
            }

            if (this.pwd != null)
            {
                str.Append($"pwd={ this.pwd.ToString() },\n");
            }

            str.Append("}");
            return str.ToString();
        }
    }
}