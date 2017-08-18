using System;
using System.Diagnostics;
using smartcardLib.Core;

namespace smartcardLib.PKCS15
{
    /// <summary>
    /// AccessControlRule ::= SEQUENCE {
    /// accessMode AccessMode,
    /// securityCondition SecurityCondition,
    /// … -- For future extensions
    /// }
    /// AccessMode::= BIT STRING
    /// {
    /// read (0),
    /// update (1),
    /// execute (2)
    /// }
    /// SecurityCondition::= CHOICE {
    /// authId Identifier,
    /// not[0] SecurityCondition,
    /// and[1] SEQUENCE SIZE(2..pkcs15-ub-securityConditions) OF SecurityCondition,
    /// or[2] SEQUENCE SIZE(2..pkcs15-ub-securityConditions) OF SecurityCondition,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15AccessControlRule
    {
        /// <summary>
        /// Access mode
        /// </summary>
        public enum AccessMode {
            /// <summary>
            /// Read Access
            /// </summary>
            read =0,
            /// <summary>
            /// Update Access
            /// </summary>
            update,
            /// <summary>
            /// Execute Access
            /// </summary>
            execute };

        private int accessMode;
        /// <summary>
        /// Security Condition
        /// </summary>
        public PKCS15SecurityCondition securityCondition { get; private set; }

        //identifier octet string
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 that holds the AccessControlRule</param>
        public PKCS15AccessControlRule(ASN1 tlv)
        {
            tlv.setName("accessControlRule");
            tlv.type = "AccessControlRule";           
            Debug.Assert(tlv.elements == 2);
            ASN1 t = tlv.get(0);
            Debug.Assert(t.Tag == ASN1.BIT_STRING);            
            this.accessMode = t.Value.GetChoiceFromBitString();
            this.securityCondition = new PKCS15SecurityCondition(tlv.get(1));            
            
        }
        /// <summary>
        /// Converts to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"AccessControlRule {{\n
                         accessMode={Enum.GetName(typeof(AccessMode),accessMode)} ,\n
                         securityCondition={{{securityCondition.ToString()}}}\n
                    }}";
        }
        /// <summary>
        /// Identifier
        /// </summary>
        public byte[] Identifier { get; private set; }


    }
}