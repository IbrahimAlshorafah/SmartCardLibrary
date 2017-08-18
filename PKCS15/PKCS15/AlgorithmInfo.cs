
using System.Linq;
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// The class decodes the following ASN1:
    /// AlgorithmInfo ::= SEQUENCE {
    ///   reference Reference,
    ///   algorithm CIO-ALGORITHM.&amp;id({ AlgorithmSet}),
    ///   parameters CIO-ALGORITHM.&amp;Parameters({ AlgorithmSet}{@algorithm
    ///}),
    ///   supportedOperations CIO-ALGORITHM.&amp;Operations({ AlgorithmSet}{@algorithm}),
    ///   objId CIO-ALGORITHM.&amp;objectIdentifier({ AlgorithmSet}{@algorithm}),
    ///   algRef Reference OPTIONAL
    /// }
    /// CIO-ALGORITHM::= CLASS {
    ///   &amp;id INTEGER UNIQUE,
    ///   &amp;Parameters,
    ///   &amp;Operations Operations,
    ///   &amp;objectIdentifier OBJECT IDENTIFIER OPTIONAL
    ///   } WITH SYNTAX
    ///{
    ///    PARAMETERS &amp;Parameters OPERATIONS &amp;Operations ID &amp;id [OID &amp;objectIdentifier]
    /// }
    /// </summary>      
    public class AlgorithmInfo
    {
        /// <summary>
        /// ASN1 object
        /// </summary>
        public ASN1 tlv { get; private set; }
        /// <summary>
        /// Reference
        /// </summary>
        public int? reference { get; private set; }
        /// <summary>
        /// Algorithm
        /// </summary>
        public int? algorithm { get; private set; }
        /// <summary>
        /// Parameters
        /// </summary>
        public ASN1 parameters { get; private set; }
        /// <summary>
        /// Supported Operations
        /// </summary>
        public byte? supportedOperations { get; private set; }
        /// <summary>
        /// Object ID
        /// </summary>
        public string objId { get; private set; }
        /// <summary>
        /// Algorithm reference
        /// </summary>
        public int? algRef { get; private set; }
        /// <summary>
        /// Constructor that takes ASN1 object
        /// </summary>
        /// <param name="basetlv">ASN1 object that holds AlgorithmInfo</param>
        public AlgorithmInfo(ASN1 basetlv)
        {
            Debug.Assert(basetlv.IsConstructed);
            Debug.Assert(basetlv.elements >= 5);
            reference = null;
            algorithm = null;
            objId = null;
            algRef = null;
            supportedOperations = null;
            this.tlv = basetlv;

            var i = 0;
            ASN1 t= null;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.INTEGER);
            t.setName("reference");
            this.reference = t.Value.ToSigned();
            i++;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.INTEGER);
            t.setName("algorithm");
            this.algorithm = t.Value.ToSigned();
            i++;

            t = tlv.get(i);
            t.setName("parameters");
            this.parameters = t;
            i++;

            Debug.Assert((t = tlv.get(i)).Tag == ASN1.BIT_STRING);
            this.supportedOperations = t.Value.Skip(1).Take(1).First();
            t.setName("supportedOperations {" + this.getOperationsAsString() + " }");
            i++;

            if (i < tlv.elements)
            {
                Debug.Assert((t = tlv.get(i)).Tag == ASN1.OBJECT_IDENTIFIER);
                t.setName("objId");
                this.objId = t.GetValue<string>();
                i++;
            }

            if (i < tlv.elements)
            {
                Debug.Assert((t = tlv.get(i)).Tag == ASN1.INTEGER);
                t.setName("algRef");
                this.algRef = t.GetValue<int>();
                i++;
            }
        }
        /// <summary>
        /// Get operation as string
        /// </summary>
        /// <returns></returns>
        private string getOperationsAsString()
        {
            return $@"{(((this.supportedOperations & 0x80) != 0) ? " compute-checksum" : "")}
           {(((this.supportedOperations & 0x40) != 0) ? " compute-signature" : "") }
           {(((this.supportedOperations & 0x20)!=0) ? " verify - checksum" : "")} 
           {(((this.supportedOperations & 0x10)!=0) ? " verify-signature" : "") }
           {(((this.supportedOperations & 0x08)!=0) ? " encipher" : "") }
           {(((this.supportedOperations & 0x04)!=0) ? " decipher" : "") }
           {(((this.supportedOperations & 0x02)!=0) ? " hash" : "") }
           {(((this.supportedOperations & 0x01)!=0) ? " generate-key" : "")}";
        }

        /// <summary>
        /// returns a string representation for the algorithm information
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            var str = "AlgorithmInfo { ";
            if (this.reference != null) {
                str += "reference=" + this.reference + ",\n";
            }
            if (this.algorithm != null) {
                str += "algorithm=0x" + this.algorithm?.ToString("X2") + ",\n";
            }
            if (this.parameters != null ) {
                str += "parameters=" + this.parameters + ",\n";
            }
            if(supportedOperations != null)
                str += "supportedOperations=" + this.getOperationsAsString() + ",\n";
            
            if (!string.IsNullOrEmpty(this.objId)) {
                str += "objId=" + this.objId + ",\n";
            }
            if (this.algRef != null) {
                str += "algRef=" + this.algRef + ",\n";
            }
            str += "}";
            return str;
        }
    }
}