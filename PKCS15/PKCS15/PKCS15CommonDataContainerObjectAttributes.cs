using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// CommonDataObjectAttributes ::= SEQUENCE {
    /// applicationName Label OPTIONAL,
    /// applicationOID OBJECT IDENTIFIER OPTIONAL,
    /// ... -- For future extensions
    /// } (WITH COMPONENTS {..., applicationName PRESENT }|
    /// WITH COMPONENTS {..., applicationOID PRESENT })
    /// </summary>
    public class PKCS15CommonDataContainerObjectAttributes : PKCS15CommonAuthenticationObjectAttributes
    {
        /// <summary>
        /// Application Name
        /// </summary>
        protected string applicationName { get; private set; }
        /// <summary>
        /// Optional Application OID
        /// </summary>
        protected string applicationOID { get; private set; }
        /// <summary>
        /// iD
        /// </summary>
        protected byte[] iD { get; private set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="basetlv">ASN containing the element</param>
        public PKCS15CommonDataContainerObjectAttributes(ASN1 basetlv):base(basetlv)
        {
            var tlv = basetlv.get(1);
            tlv.setName("commonDataContainerObjectAttributes");
            var i = 0;
            ASN1 t;

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.UTF8String))
            {
                t.setName("applicationName");
                this.applicationName = t.GetValue<string>();
                i++;
            }

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.OBJECT_IDENTIFIER))
            {
                t.setName("applicationOID");
                this.applicationOID = t.GetValue<string>();
                i++;
            }

            if ((i < tlv.elements) && ((t = tlv.get(i)).Tag == ASN1.OCTET_STRING))
            {
                t.setName("iD");
                this.iD = t.Value;
                i++;
            }
        }
        /// <summary>
        /// To readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"{base.ToString()}
            \nCommonDataContainerObjectAttributes {{\n
            {((!string.IsNullOrEmpty(this.applicationName)) ? $"applicationName={ this.applicationName},\n" : "")}            
            {((!string.IsNullOrEmpty(this.applicationOID)) ? $"applicationOID={this.applicationOID},\n" : "")}
            {((this.iD != null) ? $"iD={this.iD.ToHex()},\n" : "")}
            }}";
        }
    }
}