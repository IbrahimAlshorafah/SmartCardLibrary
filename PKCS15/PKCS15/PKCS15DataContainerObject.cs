using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// This class adds data container objects to the common data container object attributes class.
    /// The class decodes the following ASN.1 syntax for opaque data objects with indirect path reference:
    /// OpaqueDOAttributes::= ObjectValue {CIO-OPAQUE.&amp;Type
    /// }
    /// ObjectValue { Type } ::= CHOICE {
    ///         indirect ReferencedValue,
    ///         direct[0] Type,
    ///         ... -- For future extensions
    /// }
    /// ReferencedValue::= CHOICE {
    ///     path Path,
    ///     url URL
    ///} -- The syntax of the object is determined by the context
    /// </summary>
    public class PKCS15DataContainerObject : PKCS15CommonDataContainerObjectAttributes
    {
       /// <summary>
       /// Indirect Path
       /// </summary>
        public PKCS15Path indirectPath { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 object that contains data container</param>
        public PKCS15DataContainerObject(ASN1 tlv): base(tlv)
        {

            var t = tlv.get(2);
            Debug.Assert(t.Tag == 0xA1);
            Debug.Assert(t.elements == 1);
            t.setName("typeAttributes");
            t = t.get(0);
            t.setName("dataContainerAttributes");
            switch (tlv.Tag)
            {
                case 0x30: this.decodeOpaqueDOIndirectPath(t); break;
                default:
                    throw new PKC15Exception("### Unsupported data container object type : " + t);
            }

        }
        /// <summary>
        /// To readable string
        /// </summary>
        /// <returns>Readable string</returns>
        public override string ToString()
        {
            return $@"{base.ToString()}
            \nDataContainerObject {{\n
            {((this.indirectPath != null) ? $"indirectPath={this.indirectPath.ToString()},\n" : "")} }}";
        }
        /// <summary>
        /// Decode the Opage DOI indirect Path
        /// </summary>
        /// <param name="Obj">ASN1 object that contains Indirect path</param>
        public void decodeOpaqueDOIndirectPath(ASN1 Obj)
        {
            this.type = "OpaqueDOIndirectPath";
            Obj.Name = "OpaqueDOIndirectPath";
            this.indirectPath = new PKCS15Path(Obj);
        }
    }
}