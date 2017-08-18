using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// This class supports password authentication objects.
    /// The class decodes the following ASN.1 syntax:
    /// PasswordAttributes::= SEQUENCE {
    /// pwdFlags PasswordFlags,
    /// pwdType PasswordType,
    /// minLength INTEGER(cia-lb-minPasswordLength..cia-ub-minPasswordLength),
    /// storedLength INTEGER(0..cia-ub-storedPasswordLength),
    /// maxLength INTEGER OPTIONAL,
    /// pwdReference[0] Reference DEFAULT 0,
    /// padChar OCTET STRING(SIZE(1)) OPTIONAL,
    /// lastPasswordChange GeneralizedTime OPTIONAL,
    /// path Path OPTIONAL,
    /// ...
    /// </summary>
    class PKCS15PasswordAuthenticationObject
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="tlv">ASN1 data object</param>
        public PKCS15PasswordAuthenticationObject(ASN1 tlv)
        {
            tlv.setName("passwordAuthenticationObject");

            var i = 0;
            ASN1 t=null;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.BIT_STRING);
            this.pwdFlags = t.Value.IntFromBitString();
            t.setName("pwdFlags {" + this.getPwdFlagsAsString() + " }");
            i++;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.ENUMERATED);
            this.pwdType = t.GetValue<int>();
            t.setName("pwdType {" + this.getPwdTypeAsString() + " }");
            i++;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.INTEGER);
            t.setName("minLength");
            this.minLength = t.GetValue<int>();
            i++;
            if (i < tlv.elements)
            {
                Debug.Assert((t = tlv.get(i)).Tag == ASN1.INTEGER);
                t.setName("storedLength");
                this.storedLength = t.GetValue<int>();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.INTEGER))
            {
                t.setName("maxLength");
                this.maxLength = t.GetValue<int>();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == 0x80))
            {
                t.setName("pwdReference");
                this.pwdReference = t.Value.ToUnsigned();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.OCTET_STRING))
            {
                t.setName("padding");
                this.padding = t.Value.ToUnsigned();
                i++;
            }
        }
        /// <summary>
        /// Password Flags
        /// </summary>
        public int pwdFlags { get; private set; }
        /// <summary>
        /// Password Type
        /// </summary>
        public int pwdType { get; private set; }
        /// <summary>
        /// Minimum  length
        /// </summary>
        public int minLength { get; private set; }
        /// <summary>
        /// Stored Length
        /// </summary>
        /// <remarks>Optional null if not available</remarks>
        public int? storedLength { get; private set; }
        /// <summary>
        /// maximum Length
        /// </summary>
        /// <remarks>Optional null if not available</remarks>
        public int? maxLength { get; private set; }
        /// <summary>
        /// Password Reference
        /// </summary>
        /// <remarks>Optional null if not available</remarks>
        public uint? pwdReference { get; private set; }
        /// <summary>
        /// Stored Length
        /// </summary>
        /// <remarks>Optional null if not available</remarks>
        public uint? padding { get; private set; }
        /// <summary>
        /// Converts password flags to readable string
        /// </summary>
        /// <returns>result</returns>
        public string getPwdFlagsAsString()
        {
            return $@" {(((this.pwdFlags & 0x800000) != 0 )? " case-sensitive" : "") }
        {(((this.pwdFlags & 0x400000) != 0) ? " local" : "") }
        {(((this.pwdFlags & 0x200000) != 0) ? " change-disabled" : "") }
        {(((this.pwdFlags & 0x100000) != 0) ? " unblock-disabled" : "") }
        {(((this.pwdFlags & 0x080000) != 0) ? " initialized" : "") }
        {(((this.pwdFlags & 0x040000) != 0) ? " needs-padding" : "") }
        {(((this.pwdFlags & 0x020000) != 0) ? " unblockingPassword" : "") }
        {(((this.pwdFlags & 0x010000) != 0) ? " soPassword" : "") }
        {(((this.pwdFlags & 0x008000) != 0) ? " disable-allowed" : "") }
        {(((this.pwdFlags & 0x004000) != 0) ? " integrity-protected" : "") }
        {(((this.pwdFlags & 0x002000) != 0) ? " confidentiality-protected" : "") }
        {(((this.pwdFlags & 0x001000) != 0) ? " exchangeRefData" : "") }
        {(((this.pwdFlags & 0x000800) != 0) ? " resetRetryCounter1" : "") }
        {(((this.pwdFlags & 0x000400) != 0) ? " resetRetryCounter2" : "") }";
        }
        /// <summary>
        /// convert password type to readable string
        /// </summary>
        /// <returns>password type as string</returns>
        public string getPwdTypeAsString()
        {
            var str = $"UnknownFormat [{ this.pwdType.ToString("X")}]";
            switch (this.pwdType)
            {
                case 0: str = "BCD"; break;
                case 1: str = "ASCII-NUMERIC"; break;
                case 2: str = "UTF8"; break;
                case 3: str = "HALF-NIBBLE-BCD"; break;
                case 4: str = "ISO9564-1"; break;
            }
            return str;
        }
        /// <summary>
        /// convert password object details to string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"Password {{\n
            {((this.pwdFlags != -1) ? $"pwdFlags={this.getPwdFlagsAsString()},\n" : "")}
            {((this.pwdType != -1) ? $"pwdType={this.getPwdTypeAsString() },\n" : "")}
            {((this.minLength != -1) ? $"minLength={ this.minLength },\n" : "")}
            {((this.storedLength != -1) ? $"storedLength={this.storedLength},\n" : "")}
            {((this.maxLength != -1) ? $"maxLength={this.maxLength}\n" : "")}
            {((this.pwdReference != uint.MaxValue) ? $"pwdReference={this.pwdReference},\n":"")} }}";
            
        }
    }
}