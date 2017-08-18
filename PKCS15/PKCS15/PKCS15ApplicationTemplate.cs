using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// application template
    /// Data element, which may be present for example in a DIR file, and which contains one or more DOs relevant to    /// an application.
    /// its an optional object that shall include PKCS15 DIRRecord and DDO object as follow
    /// DIRRecord ::= [APPLICATION 1] SEQUENCE {
    /// aid[APPLICATION 15] OCTET STRING,
    /// label[APPLICATION 16] UTF8String OPTIONAL,
    /// path[APPLICATION 17] OCTET STRING,
    /// ddo[APPLICATION 19] DDO OPTIONAL
    /// }
    /// DDO ::= SEQUENCE {
    /// oid OBJECT IDENTIFIER,
    /// odfPath Path OPTIONAL,
    /// tokenInfoPath[0] Path OPTIONAL,
    /// unusedPath[1] Path OPTIONAL,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15ApplicationTemplate
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tlv">ASN1 object that contains the PKCS15 application template</param>
        public PKCS15ApplicationTemplate(ASN1 tlv)
        {
            this.aid = "";
            this.label = "";
            this.path = "";
            this.ddo = null;
            this.objlist = null;
            Debug.Assert(tlv.Tag == 0x61);
            Debug.Assert(tlv.elements > 0);
            for (var i = 0; i < tlv.elements; i++)
            {
                var t = tlv.get(i);
                switch (t.Tag)
                {
                    case 0x4F:
                        this.aid = t.Value.ToHex();
                        break;
                    case 0x50:
                        this.label = ASCIIEncoding.UTF8.GetString( t.Value);
                        break;
                    case 0x51:
                        this.path = ":" + t.Value.ToHex();
                        break;
                    case 0x73:
                        this.ddo = new PKCS15DDO(t);
                        break;
                }
            }
            if (this.ddo == null)
            {
                string value = ""; //TODO : You must add this manually then if there is no ddo 
            
                var t = new ASN1(value.ToBytes());
                this.ddo = new PKCS15DDO(t);
            }
        }
        /// <summary>
        /// Converts to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            var str = "{";
            if (!String.IsNullOrEmpty(this.aid))
            {
                str += "aid=" + this.aid + ",\n";
            }
            if (!String.IsNullOrEmpty(this.label))
            {
                str += "label=" + this.label + ",\n";
            }
            if (!String.IsNullOrEmpty(this.path))
            {
                str += "path=" + this.path + ",\n";
            }
            if (this.ddo != null)
            {
                str += "ddo=" + this.ddo.ToString() + ",\n";
            }
            str += "}";
            return str;

        }
        /// <summary>
        /// The application identifier for this application.
        /// </summary>
        public string aid { get; private set; }
        /// <summary>
        /// The PKCS#15 Directory Data Object (DDO) for this application.
        /// </summary>
        public PKCS15DDO ddo { get; private set; }
        /// <summary>
        /// The application label for this application
        /// </summary>
        public string label { get; private set; }
        /// <summary>
        /// The list of Cryptographic Information Objects (CIO).
        /// </summary>
        public List<PKCS15CommonObjectAttributes> objlist { get; set; }
        /// <summary>
        /// The path for this application.
        /// </summary>
        public string path { get; private set; }

    }
}