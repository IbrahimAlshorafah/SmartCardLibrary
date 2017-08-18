using smartcardLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcardLib.PKCS15
{

    /// <summary>
    /// CredentialIdentifier {KEY-IDENTIFIER : IdentifierSet} ::= SEQUENCE {
    ///   idType KEY-IDENTIFIER.&id({ IdentifierSet}),
    ///   idValue KEY-IDENTIFIER.&Value({ IdentifierSet}{@idType
    ///   })
    ///   }
    /// </summary>
    class PKCS15CredentialIdentifier
    {

        string id;
        string value;

        /// <summary>
        /// constructor
        /// </summary>
        public PKCS15CredentialIdentifier(ASN1 tlv)
        {
            id = tlv.get(0).ToString();
            value = tlv.get(1).Value.ToHex();
        }

        /// <summary>
        /// Override to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            return $@"CredentialIdentifier {{ 
                      idType {{{id}}},
                      idValue {{{value}}}
                    }}";
        }
    }
}
