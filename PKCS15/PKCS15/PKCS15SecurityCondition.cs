using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.PKCS15
{

    /// <summary>
    /// 
    /// SecurityCondition::= CHOICE {
    /// authId Identifier,
    /// not[0] SecurityCondition,
    /// and[1] SEQUENCE SIZE(2..pkcs15-ub-securityConditions) OF SecurityCondition,
    /// or[2] SEQUENCE SIZE(2..pkcs15-ub-securityConditions) OF SecurityCondition,
    /// ... -- For future extensions
    /// }
    /// </summary>
    public class PKCS15SecurityCondition
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// List of AND conditions
        /// </summary>
        public List<PKCS15SecurityCondition> AND { get; private set; }
        /// <summary>
        /// List of OR conditions
        /// </summary>
        public List<PKCS15SecurityCondition> OR { get; private set; }
        /// <summary>
        /// NOT condition
        /// </summary>
        public PKCS15SecurityCondition NOT { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlv">ASN1 that holds the security conditions</param>
        public PKCS15SecurityCondition(ASN1 tlv)
        {
            tlv.setName("securityCondition");
            tlv.type = "SecurityCondition";
            
            if(tlv.Tag == ASN1.OCTET_STRING)
            {
                Debug.Assert(tlv.elements == 0);
                Identifier = tlv.Value.ToHex();
            }
            else
            {
                switch(tlv.TagNumber)
                {
                    case 0:
                        NOT = new PKCS15SecurityCondition(tlv.get(0));
                        break;
                    case 1:
                        if (AND == null) AND = new List<PKCS15SecurityCondition>();
                        foreach (var elm in tlv.Children) AND.Add(new PKCS15SecurityCondition(elm));
                        break;
                    case 2:
                        if (OR == null) OR = new List<PKCS15SecurityCondition>();
                        foreach (var elm in tlv.Children) OR.Add(new PKCS15SecurityCondition(elm));
                        break;
                    default:
                        throw new ArgumentException("Security Condition TagNumber not defined");
                }
            }
        }
        /// <summary>
        /// Converts to readable string
        /// </summary>
        /// <returns>readable string</returns>
        public override string ToString()
        {
            return $@"{((Identifier!=null)?$"authId={Identifier},\n" :"")}{((NOT != null) ? $"Not=[{NOT.Identifier}],\n" : "")}{((AND != null) ? $"AND=[{string.Join(",", AND.Select(x=>x.Identifier).ToArray())}],\n" : "")}{((OR != null) ? $"OR=[{string.Join(",", OR.Select(x => x.Identifier).ToArray())}],\n" : "")}";
        }


    }
}
