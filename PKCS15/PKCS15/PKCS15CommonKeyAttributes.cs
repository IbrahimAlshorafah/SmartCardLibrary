using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using smartcardLib.Core;
using System.Text;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// CommonKeyAttributes ::= SEQUENCE {
    /// iD Identifier,
    /// usage KeyUsageFlags,
    /// native BOOLEAN DEFAULT TRUE,
    /// accessFlags KeyAccessFlags OPTIONAL,
    /// keyReference Reference OPTIONAL,
    /// startDate GeneralizedTime OPTIONAL,
    /// endDate[0] GeneralizedTime OPTIONAL,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15CommonKeyAttributes : PKCS15CommonObjectAttributes
    {
        #region key usage

        /// <summary>
        /// Encipher
        /// </summary>
        public static readonly int encrypt = 0;
        /// <summary>
        /// Decipher
        /// </summary>
        public static readonly int decrypt = 1;
        /// <summary>
        /// SIGN
        /// </summary>
        public static readonly int sign = 2;
        /// <summary>
        /// SIGN Recover
        /// </summary>
        public static readonly int signRecover = 3;
        /// <summary>
        /// Key Encipher
        /// </summary>
        public static readonly int wrap = 4;
        /// <summary>
        /// Key decipher
        /// </summary>
        public static readonly int unwrap = 5;
        /// <summary>
        /// Verify
        /// </summary>
        public static readonly int verify = 6;
        /// <summary>
        /// Verify recover
        /// </summary>
        public static readonly int verifyRecover = 7;
        /// <summary>
        /// Derive key
        /// </summary>
        public static readonly int derive = 8;
        /// <summary>
        /// Non repudiation
        /// </summary>
        public static readonly int nonReputiation = 9;
        #endregion
        #region key access flags
        /// <summary>
        /// Sensitive key
        /// </summary>
        public static readonly int sensitive = 0;
        /// <summary>
        /// Extractable key
        /// </summary>
        public static readonly int extractable = 1;
        /// <summary>
        /// Always sensitive
        /// </summary>
        public static readonly int alwaysSensitive = 2;
        /// <summary>
        /// Never extractable
        /// </summary>
        public static readonly int neverExtractable = 3;
        /// <summary>
        /// Card generated
        /// </summary>
        public static readonly int local = 4;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tlv">ASN1 that holds a common key attributes</param>
        public PKCS15CommonKeyAttributes(ASN1 tlv) : base(tlv)
        {
            tlv = tlv.get(1);
            tlv.setName("commonKeyAttributes");
            var i = 0;
            ASN1 t = tlv.get(i);
            // Debug.Assert(t.Tag == ASN1.OCTET_STRING);
            t.setName("iD");
            this.iD = t.Value;
            i++;
            Debug.Assert((t = tlv.get(i)).Tag == ASN1.BIT_STRING);
            this.usage = (uint)t.Value.GetChoiceFromBitString();
            t.setName("usage {" + this.getUsageAsString() + " }");
            i++;
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.BOOLEAN))
            {
                t.setName("native");
                this.native_ = t.GetValue<Boolean>();
                i++;
            }
            else
            {
                this.native_ = true;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.BIT_STRING))
            {
                this.accessFlags = (uint)t.Value.GetChoiceFromBitString();
                t.setName("accessFlags {" + this.getAccessFlagsAsString() + " }");
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.INTEGER))
            {
                t.setName("keyReference");
                this.keyReference = t.GetValue<int>();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.GeneralizedTime))
            {
                t.setName("startDate");
                this.startDate = t.GetValue<DateTime>();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == 0x80))
            {
                t.setName("endDate");
                this.endDate = t.GetValue<DateTime>();
                i++;
            }
            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == 0xA1))
            {
                Debug.Assert(t.IsConstructed);
                t.setName("algReference");
                this.algReference = new List<int>();
                for (var j = 0; j < t.elements; j++)
                {
                    Debug.Assert(t.get(j).Tag == ASN1.INTEGER);
                    t.get(j).setName("reference");
                    this.algReference.Add(t.get(j).Value.ToSigned());
                }
                i++;
            }
        }

        /// <summary>
        /// convert access flag to readable string
        /// </summary>
        /// <returns>readable string</returns>
        private string getAccessFlagsAsString()
        {
            return $@"{((this.accessFlags == sensitive) ? " sensitive" : "") }{((this.accessFlags == extractable) ? " extractable" : "") }{((this.accessFlags == alwaysSensitive) ? " alwaysSensitive" : "") }{((this.accessFlags == neverExtractable) ? " neverExtractable" : "") }{((this.accessFlags == local) ? " local" : "")}";
        }
        /// <summary>
        /// convert usage to readable string
        /// </summary>
        /// <returns>readable string</returns>
        private string getUsageAsString()
        {
            return $@"{((this.usage == encrypt) ? " encrypt" : "") }{((this.usage == decrypt) ? " decrypt" : "") }{((this.usage == sign) ? " sign" : "") }{((this.usage == signRecover) ? " signRecover" : "") }{((this.usage == wrap) ? "wrap" : "") }{((this.usage == unwrap) ? "unwrap" : "") }{((this.usage == verify) ? " verify" : "") }{((this.usage == verifyRecover) ? " verifyRecover" : "") }{((this.usage == derive) ? " derive" : "") }{((this.usage == nonReputiation) ? " nonRepudiation" : "")}";
        }
        /// <summary>
        /// converts the PKCS15CommonKeyAttributes attributes to readable format
        /// </summary>
        /// <returns>readable format</returns>
        public override string ToString()
        {
            var str = new StringBuilder(base.ToString());
            str.Append("\ncommonKeyAttributes {\n ");
            if (this.iD != null) str.Append($"iD={this.iD.ToHex()},\n");
            if (this.usage != null) str.Append($"usage={this.getUsageAsString() },\n");
            if (this.native_ != null) str.Append($"native={this.native_},\n");
            if (this.accessFlags != null) str.Append($"accessFlags={ this.getAccessFlagsAsString() },\n");
            if (this.keyReference != null) str.Append($"keyReference=0x{this.keyReference?.ToString("X")},\n");
            if (this.startDate != null) str.Append($"startDate={this.startDate },\n");
            if (this.endDate != null) str.Append($"endDate={ this.endDate},\n");
            if (this.algReference != null) str.Append($"algReference={ string.Join(":", this.algReference.Select(x => $"0x{x.ToString("X2")}"))},\n");
            str.Append("}");
            return str.ToString();
        }
        /// <summary>
        /// Access flags
        /// </summary>
        public uint? accessFlags { get; private set; }
        /// <summary>
        /// Algorithm reference
        /// </summary>
        public List<int> algReference { get; private set; }
        /// <summary>
        /// End Date
        /// </summary>
        public DateTime? endDate { get; private set; }
        /// <summary>
        /// ID
        /// </summary>
        public byte[] iD { get; private set; }
        /// <summary>
        /// Key reference
        /// </summary>
        public int? keyReference { get; private set; }
        /// <summary>
        /// Is Native (default true)
        /// </summary>
        public bool? native_ { get; private set; }
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? startDate { get; private set; }
        /// <summary>
        /// Usage
        /// </summary>
        public uint? usage { get; private set; }
    }

}