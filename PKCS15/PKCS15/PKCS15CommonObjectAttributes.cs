using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// The class decodes the following ASN.1 syntax:
    /// CommonObjectAttributes ::= SEQUENCE {
    /// label Label OPTIONAL,
    /// flags CommonObjectFlags OPTIONAL,
    /// authId Identifier OPTIONAL,
    /// userConsent INTEGER(1..cia-ub-userConsent) OPTIONAL,
    /// accessControlRules SEQUENCE SIZE(1..MAX) OF AccessControlRule OPTIONAL,
    /// ...
    /// } (CONSTRAINED BY {-- authId should be present if flags.private is set.
    /// -- It shall equal an authID in one authentication object in the AOD -- })
    /// </summary>
    public class PKCS15CommonObjectAttributes
    {
        #region Common Authentication Object Attributes flags
        /// <summary>
        /// PRIVATE
        /// </summary>
        public static readonly int PRIVATE = 0x80;
        /// <summary>
        /// Modifiable
        /// </summary>
        public static readonly int MODIFIABLE = 0x40;
        /// <summary>
        /// Internal
        /// </summary>
        public static readonly int INTERNAL = 0x20;
        #endregion
        // protected ASN1 tlv;
        /// <summary>
        /// List of access control rules
        /// </summary>
        public List<PKCS15AccessControlRule> accessControlRules { get; private set; }
        /// <summary>
        /// Authentication object ID
        /// </summary>
        public byte[] authId { get; private set; }
        /// <summary>
        /// flags
        /// </summary>
        public uint? flags { get; private set; }
        /// <summary>
        /// Label
        /// </summary>
        public string label { get; private set; }
        /// <summary>
        /// User consent
        /// </summary>
        public int? userConsent { get; private set; }
        /// <summary>
        /// Type
        /// </summary>
        public string type { get;  set; }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="tlv">ASN1 object of PKCS15 CIO</param>
        public PKCS15CommonObjectAttributes(ASN1 tlv)
        {
            Debug.Assert(tlv.IsConstructed);
            Debug.Assert(tlv.elements >= 3);
            //this.tlv = tlv;
            var coa = tlv.get(0);
            Debug.Assert(coa.Tag == ASN1.SEQUENCE);
            coa.setName("commonObjectAttributes");
            var i = 0;
            ASN1 t = null;
            if ((i < coa.elements) && ((t = coa.get(i)).Tag == ASN1.UTF8String))
            {
                t.setName("label");
                this.label = t.GetValue<string>();
                i++;
            }
            // D-Trust card has empty bitstring element
            if ((i < coa.elements) && ((t = coa.get(i)).Tag == ASN1.BIT_STRING) && (t.Value.Length > 1))
            {
                this.flags = t.Value.Skip(1).Take(1).ToArray().ToUnsigned();
                t.setName("flags {" + this.getFlagsAsString() + " }");
                i++;
            }
            if ((i < coa.elements) && ((t = coa.get(i)).Tag == ASN1.OCTET_STRING))
            {
                t.setName("authId");
                this.authId = t.Value;
                i++;
            }
            if ((i < coa.elements) && ((t = coa.get(i)).Tag == ASN1.INTEGER))
            {
                t.setName("userConsent");
                this.userConsent = t.Value.ToSigned();
                i++;
            }
            if ((i < coa.elements) && ((t = coa.get(i)).Tag == ASN1.SEQUENCE))
            {
                t.setName("accessControlRules");
                this.accessControlRules = new List<PKCS15AccessControlRule>();
                foreach (var child in t.Children)
                    this.accessControlRules.Add( new PKCS15AccessControlRule( child));
                i++;
            }
        }
        
        /// <summary>
        /// get CIO flags as readable
        /// </summary>
        /// <returns>readable flags</returns>
        public virtual string getFlagsAsString()
        {
            return $@" {(((this.flags & 0x80) != 0) ? " private" : "") }
        {(((this.flags & 0x40) !=0) ? " modifiable" : "") }
        {(((this.flags & 0x20) != 0) ? " internal" : "")}";
        }

        /// <summary>
        /// converts to string
        /// </summary>
        /// <returns>converts the CIO to a readable string</returns>
        public override string ToString()
        {
             return $@"CommonObjectAttributes {{
            {((this.label != null)? $@"label={this.label}\n":"")}            
            {((this.flags != null)? $"flags={this.getFlagsAsString()} \n":"")}            
            {((this.authId != null)?$"authId={this.authId}\n":"")}            
            {((this.userConsent != null)?$"userConsent={this.userConsent}\n":"")}            
            {((this.accessControlRules != null)?$"accessControlRules= { string.Join( ":", this.accessControlRules.Select(x=>x.ToString())) },\n":"")}
            }}";
                        
        }
    }
}